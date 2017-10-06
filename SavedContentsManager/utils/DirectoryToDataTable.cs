using System;
using System.Collections.Generic;
using System.Data;
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
        //const string CACHE_FILE_NAME = "scm_cache.xml";
        const string CACHE_FILE_NAME = "scm_cache.db";
        public delegate void ReportProgress(int p);
        public ReportProgress reportProgressMethod;

        private string cacheFileName = null;
        private string sourceDirectory = null;

        private DataTable directoryInfo = null;

        public DataView DirectoryInfoView { get; set; } = null;

        public DirectoryToDataTable(string sourceDirectory)
        {
            this.sourceDirectory = sourceDirectory;
            this.cacheFileName = sourceDirectory + Path.DirectorySeparatorChar + CACHE_FILE_NAME;

            directoryInfo = new DataTable("DirectoryToDataTable");
            directoryInfo.CaseSensitive = false;
            directoryInfo.Columns.Add("Title Name", typeof(string));
            directoryInfo.Columns.Add("Last Updated", typeof(string));
            directoryInfo.Columns.Add("Sub Folders", typeof(int));
            directoryInfo.Columns.Add("Parent", typeof(string));
            directoryInfo.Columns.Add("Check", typeof(int));

            DirectoryInfoView = new DataView(directoryInfo);
            DirectoryInfoView.Sort = "[Title Name] ASC";

            init();
        }

        private void init()
        {
            //if (File.Exists(cacheFileName))
            //{
            //    // load cache
            //    try
            //    {
            //        directoryInfo.ReadXml(cacheFileName);
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.ToString());
            //    }
            //}

            // Open database
            string strConn = "Data Source=" + cacheFileName.Replace('\\', '/') + ";Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(strConn))
            {
                conn.Open();
                //
                DataTable dtSchema = conn.GetSchema("Tables");

                DataRow[] rows = dtSchema.Select("TABLE_NAME='SCM_CACHE'");
                if (rows.Length == 0)
                {
                    _newDatabase(conn);
                }

                using (SQLiteCommand cmd = new SQLiteCommand("SELECT TITLE_NAME, LAST_UPDATED, SUB_FOLDERS, PARENT, CHK FROM SCM_CACHE ORDER BY 1", conn))
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        //MessageBox.Show(rdr["name"] + " " + rdr["age"]);

                        DataRow row = directoryInfo.NewRow();
                        row["Title Name"] = rdr["TITLE_NAME"];
                        row["Last Updated"] = rdr["LAST_UPDATED"];
                        row["Sub Folders"] = rdr["SUB_FOLDERS"];
                        row["Parent"] = rdr["PARENT"];
                        row["Check"] = rdr["CHK"];

                        directoryInfo.Rows.Add(row);
                    }
                }
            }
        }

        private void _newDatabase(SQLiteConnection conn)
        {
            string sql = "CREATE TABLE SCM_CACHE (TITLE_NAME varchar(200), LAST_UPDATED varchar(30), SUB_FOLDERS INTEGER, PARENT varchar(10), CHK INTEGER, PRIMARY KEY (TITLE_NAME))";

            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void WriteCache()
        {
            //File.Delete(cacheFileName);
            //try
            //{
            //    directoryInfo.WriteXml(cacheFileName);
            //}
            //catch (Exception)
            //{
            //    directoryInfo.WriteXml(cacheFileName);
            //}

            string strConn = "Data Source=" + cacheFileName.Replace('\\', '/') + ";Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(strConn))
            {
                conn.Open();
                //
                DataTable dtSchema = conn.GetSchema("Tables");

                DataRow[] rows = dtSchema.Select("TABLE_NAME='SCM_CACHE'");
                if (rows.Length == 0)
                {
                    _newDatabase(conn);
                }

                using (SQLiteTransaction transaction = conn.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM SCM_CACHE", conn))
                    {
                        cmd.Transaction = transaction;
                        cmd.ExecuteNonQuery();
                    }

                    string sql = "INSERT INTO SCM_CACHE (TITLE_NAME, LAST_UPDATED, SUB_FOLDERS, PARENT, CHK) VALUES (@TITLE_NAME, @LAST_UPDATED, @SUB_FOLDERS, @PARENT, @CHK)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.Add("@TITLE_NAME", DbType.String);
                        cmd.Parameters.Add("@LAST_UPDATED", DbType.String);
                        cmd.Parameters.Add("@SUB_FOLDERS", DbType.Int16);
                        cmd.Parameters.Add("@PARENT", DbType.String);
                        cmd.Parameters.Add("@CHK", DbType.Int16);
                        cmd.Transaction = transaction;
                        cmd.Prepare();

                        foreach (DataRow row in directoryInfo.Rows)
                        {
                            //directoryInfo.Columns.Add("Title Name", typeof(string));
                            //directoryInfo.Columns.Add("Last Updated", typeof(string));
                            //directoryInfo.Columns.Add("Sub Folders", typeof(int));
                            //directoryInfo.Columns.Add("Parent", typeof(string));
                            //directoryInfo.Columns.Add("Check", typeof(int));

                            cmd.Parameters["@TITLE_NAME"].Value = row["Title Name"].ToString();
                            cmd.Parameters["@LAST_UPDATED"].Value = row["Last Updated"].ToString();
                            cmd.Parameters["@SUB_FOLDERS"].Value = row["Sub Folders"].ToString();
                            cmd.Parameters["@PARENT"].Value = row["Parent"].ToString();
                            cmd.Parameters["@CHK"].Value = row["Check"].ToString();

                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        public void Refresh()
        {
            directoryInfo.Rows.Clear();
            DifferentCheck();
        }

        public void DifferentCheck()
        {
            DifferentCheck("", sourceDirectory);
            WriteCache();
        }

        private void DifferentCheck(string parent, string startDir)
        {
            DirectoryInfo[] dirList = new DirectoryInfo(startDir).GetDirectories();

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
                DataRow[] foundRows = directoryInfo.Select("[Title Name] = '" + dirName + "'");
                if (foundRows.Length == 0)
                {
                    //int subDir = dir.GetDirectories().Length;
                    DetailDirectory detail = new DetailDirectory(dir.FullName);
                    int subDir = detail.Directories.Count;
                    int checkCount = detail.Directories.Where(item => !item.isPrefix).Count();

                    DataRow row = directoryInfo.NewRow();
                    row["Title Name"] = dir.Name;
                    row["Last Updated"] = lastDate;
                    row["Sub Folders"] = subDir;
                    row["Parent"] = parent;
                    row["Check"] = checkCount;

                    directoryInfo.Rows.Add(row);
                }
                else
                {
                    if (foundRows.Length > 1)
                    {
                        throw new Exception("Duplicated entry!");
                    }

                    if (!lastDate.Equals(foundRows[0]["Last Updated"]))
                    {
                        //int subDir = dir.GetDirectories().Length;
                        DetailDirectory detail = new DetailDirectory(dir.FullName);
                        int subDir = detail.Directories.Count;
                        int checkCount = detail.Directories.Where(item => !item.isPrefix).Count();

                        foundRows[0].BeginEdit();
                        foundRows[0]["Last Updated"] = lastDate;
                        foundRows[0]["Sub Folders"] = subDir;
                        foundRows[0]["Check"] = checkCount;
                        foundRows[0].EndEdit();
                    }
                }
            }

            // deleted directory update
            List<DataRow> del = new List<DataRow>();
            foreach (DataRow item in directoryInfo.Rows)
            {
                var q = from d in dirList where d.Name == (string)item["Title Name"] select d;
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
