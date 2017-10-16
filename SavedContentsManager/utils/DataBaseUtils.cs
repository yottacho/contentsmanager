using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavedContentsManager.utils
{
    class DataBaseUtils
    {
        const string DATABASE_FILE_NAME = "scm_cache.db";

        public static string getDatabaseFilePath(string sourceDirectory)
        {
            string cacheFileName = DATABASE_FILE_NAME;

            if (sourceDirectory != null && sourceDirectory.Length > 0)
                cacheFileName = sourceDirectory + Path.DirectorySeparatorChar + DATABASE_FILE_NAME;

            return cacheFileName;
        }

        public static string getDatabaseFilePath()
        {
            return getDatabaseFilePath(AppDomain.CurrentDomain.BaseDirectory);
        }

        /// <summary>
        /// Create SQLIte3 Connection string
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <returns></returns>
        public static string getConnectionString(string sourceDirectory)
        {
            string cacheFileName = getDatabaseFilePath(sourceDirectory);

            string strConn = "Data Source=" + cacheFileName.Replace('\\', '/') + ";Version=3;";

            return strConn;
        }

        /// <summary>
        /// Create SQLite3 Connection string (Application directory)
        /// </summary>
        /// <returns></returns>
        public static string getConnectionString()
        {
            return getConnectionString(AppDomain.CurrentDomain.BaseDirectory);
        }

        /// <summary>
        /// Check table exists
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static bool checkTable(IDbConnection conn, string tableName)
        {
            DataTable dtSchema = ((DbConnection)conn).GetSchema("Tables");
            DataRow[] rows = dtSchema.Select("TABLE_NAME='" + tableName + "'");
            if (rows.Length == 0)
                return false;
            return true;
        }
    }
}
