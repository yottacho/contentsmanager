using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavedContentsManager.utils
{

    class DirectoryToDataTable
    {
        const string CACHE_FILE_NAME = "scm_cache.xml";
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
            if (File.Exists(cacheFileName))
            {
                // load cache
                Console.WriteLine("Load from cache ...");
                try
                {
                    directoryInfo.ReadXml(cacheFileName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            //DifferentCheck();
        }

        public void WriteCache()
        {
            Console.WriteLine("WriteCache.");

            File.Delete(cacheFileName);
            directoryInfo.WriteXml(cacheFileName);
        }

        public void Refresh()
        {
            Console.WriteLine("Refresh.");

            directoryInfo.Rows.Clear();
            DifferentCheck();
        }

        public void DifferentCheck()
        {
            Console.WriteLine("DifferentCheck.");

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
                    Console.WriteLine("New: " + dir.Name);
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
                        Console.WriteLine("ERROR! duplicated entry found.");
                        throw new Exception("Duplicated entry!");
                    }

                    if (!lastDate.Equals(foundRows[0]["Last Updated"]))
                    {
                        Console.WriteLine("Update: " + dir.Name);
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
                    Console.WriteLine("Delete: " + item["Title Name"]);
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
