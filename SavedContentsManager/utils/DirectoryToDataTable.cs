using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SavedContentsManager.utils
{

    class DirectoryToDataTable
    {
        const string TABLE_NAME = "SCM_CACHE";

        public delegate void ReportProgress(int p);
        public ReportProgress reportProgressMethod;

        private string sourceDirectory = null;

        private DataSet dataSet = null;
        private DataTable directoryInfo = null;

        public DataView DirectoryInfoView { get; set; } = null;

        public DirectoryToDataTable(string sourceDirectory)
        {
            this.sourceDirectory = sourceDirectory.Replace('\\', '/');

            init();

            if (directoryInfo != null && DirectoryInfoView == null)
            {
                DirectoryInfoView = new DataView(directoryInfo);
                DirectoryInfoView.Sort = "[TITLE_NAME] ASC";
            }

        }

        private void init()
        {
            dataSet = new DataSet();
            string sql = "SELECT " +
                "TITLE_NAME, LAST_UPDATED, SUB_FOLDERS, PARENT, CHECKED, BACKUP_YN" +
                " FROM " + TABLE_NAME +
                " WHERE FOLDER_PATH = '" + sourceDirectory + "'" +
                " ORDER BY TITLE_NAME";

            // Open database
            using (SQLiteConnection conn = new SQLiteConnection(DataBaseUtils.getConnectionString()))
            {
                conn.Open();

                if (!DataBaseUtils.checkTable(conn, TABLE_NAME))
                {
                    using (IDbCommand cmd = conn.CreateCommand())
                        _newDatabase(cmd);
                }

                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(sql, conn);

                dataAdapter.Fill(dataSet, TABLE_NAME);
                directoryInfo = dataSet.Tables[TABLE_NAME];

                directoryInfo.Columns["TITLE_NAME"].Caption = "Title Name";
                directoryInfo.Columns["LAST_UPDATED"].Caption = "Last Updated";
                directoryInfo.Columns["SUB_FOLDERS"].Caption = "Sub Folders";
                directoryInfo.Columns["PARENT"].Caption = "Parent";
                directoryInfo.Columns["CHECKED"].Caption = "Check";
                directoryInfo.Columns["BACKUP_YN"].Caption = "Backup";
            }
        }

        private void _newDatabase(IDbCommand cmd)
        {
            string sql = "CREATE TABLE " + TABLE_NAME + " (" +
                "TITLE_NAME varchar(200), " +
                "LAST_UPDATED varchar(30), " +
                "SUB_FOLDERS INTEGER, " +
                "PARENT varchar(10), " +
                "CHECKED INTEGER, " +
                "FOLDER_PATH varchar(1000), " +
                "BACKUP_YN varchar(1), " +
                "PRIMARY KEY (TITLE_NAME, FOLDER_PATH, BACKUP_YN)" +
                ")";

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public void WriteCache()
        {
            // Open database
            using (SQLiteConnection conn = new SQLiteConnection(DataBaseUtils.getConnectionString()))
            {
                conn.Open();

                if (!DataBaseUtils.checkTable(conn, TABLE_NAME))
                {
                    using (IDbCommand cmd = conn.CreateCommand())
                        _newDatabase(cmd);
                }

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    SQLiteCommand selectCommand = conn.CreateCommand();
                    SQLiteCommand insertCommand = conn.CreateCommand();
                    SQLiteCommand updateCommand = conn.CreateCommand();
                    SQLiteCommand deleteCommand = conn.CreateCommand();

                    selectCommand.CommandText = "SELECT TITLE_NAME, LAST_UPDATED, SUB_FOLDERS, PARENT, CHECKED, BACKUP_YN" +
                        " FROM " + TABLE_NAME +
                        " WHERE TITLE_NAME = @TITLE_NAME" +
                        " AND FOLDER_PATH = '" + sourceDirectory + "'" +
                        " AND BACKUP_YN = @BACKUP_YN";
                    selectCommand.Parameters.Add("@TITLE_NAME", DbType.String, 200, "TITLE_NAME");
                    selectCommand.Parameters.Add("@BACKUP_YN", DbType.String, 1, "BACKUP_YN");
                    selectCommand.Transaction = transaction;

                    insertCommand.CommandText = "INSERT INTO " + TABLE_NAME +
                        " (TITLE_NAME, LAST_UPDATED, SUB_FOLDERS, PARENT, CHECKED, FOLDER_PATH, BACKUP_YN) VALUES" +
                        " (@TITLE_NAME, @LAST_UPDATED, @SUB_FOLDERS, @PARENT, @CHECKED, '" + sourceDirectory + "', @BACKUP_YN)";

                    insertCommand.Parameters.Add("@TITLE_NAME", DbType.String, 200, "TITLE_NAME");
                    insertCommand.Parameters.Add("@LAST_UPDATED", DbType.String, 30, "LAST_UPDATED");
                    insertCommand.Parameters.Add("@SUB_FOLDERS", DbType.Int16, 5, "SUB_FOLDERS");
                    insertCommand.Parameters.Add("@PARENT", DbType.String, 10, "PARENT");
                    insertCommand.Parameters.Add("@CHECKED", DbType.Int16, 5, "CHECKED");
                    insertCommand.Parameters.Add("@BACKUP_YN", DbType.String, 1, "BACKUP_YN");
                    insertCommand.Transaction = transaction;

                    updateCommand.CommandText = "UPDATE " + TABLE_NAME +
                        " SET LAST_UPDATED = @LAST_UPDATED, SUB_FOLDERS = @SUB_FOLDERS," +
                        " PARENT = @PARENT, CHECKED = @CHECKED" +
                        " WHERE TITLE_NAME = @TITLE_NAME" +
                        " AND FOLDER_PATH = '" + sourceDirectory + "'" +
                        " AND BACKUP_YN = @BACKUP_YN";

                    updateCommand.Parameters.Add("@LAST_UPDATED", DbType.String, 30, "LAST_UPDATED");
                    updateCommand.Parameters.Add("@SUB_FOLDERS", DbType.Int16, 5, "SUB_FOLDERS");
                    updateCommand.Parameters.Add("@PARENT", DbType.String, 10, "PARENT");
                    updateCommand.Parameters.Add("@CHECKED", DbType.Int16, 5, "CHECKED");
                    updateCommand.Parameters.Add("@TITLE_NAME", DbType.String, 200, "TITLE_NAME");
                    updateCommand.Parameters.Add("@BACKUP_YN", DbType.String, 1, "BACKUP_YN");
                    updateCommand.Transaction = transaction;

                    deleteCommand.CommandText = "DELETE FROM " + TABLE_NAME +
                        " WHERE TITLE_NAME = @TITLE_NAME" +
                        " AND FOLDER_PATH = '" + sourceDirectory + "'" +
                        " AND BACKUP_YN = @BACKUP_YN";
                    deleteCommand.Parameters.Add("@TITLE_NAME", DbType.String, 200, "TITLE_NAME");
                    deleteCommand.Parameters.Add("@BACKUP_YN", DbType.String, 1, "BACKUP_YN");
                    deleteCommand.Transaction = transaction;

                    SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter();
                    dataAdapter.SelectCommand = selectCommand;
                    dataAdapter.InsertCommand = insertCommand;
                    dataAdapter.UpdateCommand = updateCommand;
                    dataAdapter.DeleteCommand = deleteCommand;

                    //dataAdapter.Update(directoryInfo);
                    dataAdapter.Update(dataSet, TABLE_NAME);

                    transaction.Commit();
                }
            }
        }

        public void Refresh()
        {
            directoryInfo.Rows.Clear();

            // Open database
            using (SQLiteConnection conn = new SQLiteConnection(DataBaseUtils.getConnectionString()))
            {
                conn.Open();

                if (!DataBaseUtils.checkTable(conn, TABLE_NAME))
                {
                    using (IDbCommand cmd = conn.CreateCommand())
                        _newDatabase(cmd);
                }

                // delete all data
                SQLiteCommand deleteCommand = conn.CreateCommand();
                deleteCommand.CommandText = "DELETE FROM " + TABLE_NAME +
                    " WHERE FOLDER_PATH = '" + sourceDirectory + "'";
                deleteCommand.ExecuteNonQuery();
            }

            DifferentCheck();
        }

        public void DifferentCheck()
        {
            DifferentCheck("", sourceDirectory);
            WriteCache();
        }

        private void DifferentCheck(string parent, string startDir)
        {
            //System.IO.DirectoryNotFoundException
            DirectoryInfo[] dirList = null;

            try
            {
                dirList = new DirectoryInfo(startDir).GetDirectories();
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("지정한 폴더를 찾을 수 없습니다");
                return;
            }

            // new or updated directory to cache
            int total = dirList.Length;
            int current = 0;
            int progress = 0;
            foreach (DirectoryInfo dir in dirList)
            {
                current++;
                progress = (int)((double)current / (double)total * 100.0);
                if (progress < 0)
                    progress = 0;
                if (progress > 100)
                    progress = 100;

                try
                {
                    if (reportProgressMethod != null)
                        reportProgressMethod(progress);
                }
                catch (InvalidOperationException) { }

                DateTime lastWriteTime = dir.LastWriteTime;
                string lastDate = lastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");

                string dirName = dir.Name.Replace("'", "''");
                DataRow[] foundRows = directoryInfo.Select("[TITLE_NAME] = '" + dirName + "'");
                // 동일한 이름의 디렉터리가 없는 경우
                if (foundRows.Length == 0)
                {
                    DetailDirectory detail = new DetailDirectory(dir.FullName);
                    int subDir = detail.Directories.Count;
                    int checkCount = detail.Directories.Where(item => !item.isPrefix).Count();

                    DataRow row = directoryInfo.NewRow();
                    row.BeginEdit();
                    row["TITLE_NAME"] = (string)dir.Name;
                    row["LAST_UPDATED"] = (string)lastDate;
                    row["SUB_FOLDERS"] = subDir;
                    row["PARENT"] = parent;
                    row["CHECKED"] = checkCount;
                    row["BACKUP_YN"] = "N";
                    row.EndEdit();

                    directoryInfo.Rows.Add(row);
                }
                // 테이블에 동일한 이름의 디렉터리가 있는 경우
                else
                {
                    if (foundRows.Length > 1)
                    {
                        throw new Exception("Duplicated entry!");
                    }

                    if (!lastDate.Equals(foundRows[0]["LAST_UPDATED"]))
                    {
                        //int subDir = dir.GetDirectories().Length;
                        DetailDirectory detail = new DetailDirectory(dir.FullName);
                        int subDir = detail.Directories.Count;
                        int checkCount = detail.Directories.Where(item => !item.isPrefix).Count();

                        foundRows[0].BeginEdit();

                        //foundRows[0]["TITLE_NAME"] = dir.Name;
                        foundRows[0]["LAST_UPDATED"] = lastDate;
                        foundRows[0]["SUB_FOLDERS"] = subDir;
                        //foundRows[0]["PARENT"] = parent;
                        foundRows[0]["CHECKED"] = checkCount;
                        foundRows[0]["BACKUP_YN"] = "N";
                        foundRows[0].EndEdit();
                    }
                }
            }

            // deleted directory update
            List<DataRow> del = new List<DataRow>();
            foreach (DataRow item in directoryInfo.Rows)
            {
                string itemName = "";
                if (item["TITLE_NAME"] != DBNull.Value)
                    itemName = (string)item["TITLE_NAME"];

                var q = from d in dirList where d.Name == itemName select d;
                if (q.Count() == 0)
                {
                    // not found!
                    del.Add(item);
                }
            }

            foreach (DataRow item in del)
            {
                directoryInfo.Rows.Remove(item);
            }
        }

    }
}
