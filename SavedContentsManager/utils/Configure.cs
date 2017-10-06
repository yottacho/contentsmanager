using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavedContentsManager.utils
{
    class Configure
    {
        public static string LastSelectedContentsFolder
        {
            get
            {
                return ConfigureImpl.get("ContentsLastSelected");
            }
            set
            {
                addContentsHistory(value);
                ConfigureImpl.set("ContentsLastSelected", value);
            }
        }

        public static string LastSelectedSourceFolder
        {
            get
            {
                return ConfigureImpl.get("SourceLastSelected");
            }
            set
            {
                ConfigureImpl.set("SourceLastSelected", value);
            }
        }

        public static void addContentsHistory(string newValue)
        {
            string[] list = getContentsHistory();
            foreach (string item in list)
            {
                if (item.Equals(newValue))
                    return;
            }

            ConfigureImpl.set("Contents9", ConfigureImpl.get("Contents8"));
            ConfigureImpl.set("Contents8", ConfigureImpl.get("Contents7"));
            ConfigureImpl.set("Contents7", ConfigureImpl.get("Contents6"));
            ConfigureImpl.set("Contents6", ConfigureImpl.get("Contents5"));
            ConfigureImpl.set("Contents5", ConfigureImpl.get("Contents4"));
            ConfigureImpl.set("Contents4", ConfigureImpl.get("Contents3"));
            ConfigureImpl.set("Contents3", ConfigureImpl.get("Contents2"));
            ConfigureImpl.set("Contents2", ConfigureImpl.get("Contents1"));
            ConfigureImpl.set("Contents1", newValue);
        }

        public static string[] getContentsHistory()
        {
            List<string> list = new List<string>();
            for (int i = 1; i <= 9; i++)
            {
                string val = ConfigureImpl.get("Contents" + i);
                if (val != null && val.Length > 0)
                    list.Add(val);
            }

            return list.ToArray();
        }

    }
}
