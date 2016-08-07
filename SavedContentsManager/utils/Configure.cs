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

    }
}
