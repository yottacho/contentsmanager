using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavedContentsManager.utils
{
    class DetailDirectory
    {
        private string basePath;

        public List<NameStructure> Directories { get; private set; }

        public DetailDirectory(string path)
        {
            basePath = path;
            makeList();
        }

        public void makeList()
        {
            Directories = new List<NameStructure>();

            DirectoryInfo[] dirList = new DirectoryInfo(basePath).GetDirectories();
            Array.Sort(dirList, (x, y) =>
            {
                DirectoryInfo xx = x as DirectoryInfo;
                DirectoryInfo yy = y as DirectoryInfo;

                return xx.Name.CompareTo(yy.Name);
            });

            foreach (DirectoryInfo dir in dirList)
            {
                NameStructure detail = new NameStructure(dir.Name);
                detail.LastTime = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");

                Directories.Add(detail);
            }
        }

        public void moveUp(int index)
        {
            if (Directories == null)
                return;

            if (index <= 0)
                return;

            NameStructure item = Directories[index];
            Directories.RemoveAt(index);
            Directories.Insert(index - 1, item);
        }

        public void moveDown(int index)
        {
            if (Directories == null)
                return;

            if (index >= Directories.Count)
                return;

            NameStructure item = Directories[index];
            Directories.RemoveAt(index);
            Directories.Insert(index + 1, item);
        }

        public void remapName()
        {
            int cnt = 1;
            foreach (NameStructure item in Directories)
            {
                string sidx = string.Format("{0:D3}", cnt);

                string newName = sidx + " " + item.Name;
                if (!item.FullName.Equals(newName))
                    item.TargetFullName = newName;

                cnt++;
            }
        }

        public int renameCount()
        {
            int count = 0;
            foreach (NameStructure item in Directories)
            {
                if (item.TargetFullName != null && item.TargetFullName.Length > 0 && !item.FullName.Equals(item.TargetFullName))
                {
                    count++;
                }
            }
            return count;
        }

        public void renameAll()
        {
            foreach (NameStructure item in Directories)
            {
                if (item.TargetFullName != null && item.TargetFullName.Length > 0 && !item.FullName.Equals(item.TargetFullName))
                {
                    Console.WriteLine("Rename [" + item.FullName + "] => [" + item.TargetFullName + "]");
                    string fromName = basePath + Path.DirectorySeparatorChar + item.FullName;
                    string toName = basePath + Path.DirectorySeparatorChar + item.TargetFullName;

                    new DirectoryInfo(fromName).MoveTo(toName);
                }
            }
        }
    }
}
