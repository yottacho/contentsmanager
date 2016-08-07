using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SavedContentsManager.utils
{
    public class ConfigureImpl
    {
        private static string INI_FILE_NAME = AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.DirectorySeparatorChar + "scm.ini";
        private static Dictionary<string, string> cache = new Dictionary<string, string>();

        // ---- ini 파일 의 읽고 쓰기를 위한 kernel32.dll API 함수 선언 ----
        // Note  This function is provided only for compatibility with 16-bit Windows-based applications. Applications should store initialization information in the registry.
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(    // ini Read 함수
            string section,
            string key,
            string def,
            StringBuilder retVal,
            int size,
            string filePath);

        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(  // ini Write 함수
            string section,
            string key,
            string val,
            string filePath);

        /// <summary>
        /// ini파일에서 읽어 오기
        /// </summary>
        /// <param name="Key">Access key</param>
        /// <param name="avsPath">파일 경로, 특별히 지정하지 않으면 프로그램이 설치된 디렉터리의 기본 파일명을 읽는다</param>
        /// <returns>값, 없을 경우 ""</returns>
        public static string get(string Key, string avsPath = null)
        {
            if (cache.ContainsKey(Key))
                return cache[Key];
            else
            {
                if (avsPath == null)
                    avsPath = INI_FILE_NAME;

                StringBuilder temp = new StringBuilder(2000);
                GetPrivateProfileString("Init", Key, "", temp, 2000, avsPath);

                string value = temp.ToString();
                cache[Key] = value;

                return value;
            }
        }

        /// <summary>
        /// ini파일에 쓰기
        /// </summary>
        /// <param name="Key">Access key</param>
        /// <param name="Value">내용</param>
        /// <param name="avsPath">파일 경로, 특별히 지정하지 않으면 프로그램이 설치된 디렉터리의 기본 파일에 작성한다</param>
        public static void set(string Key, string Value, string avsPath = null)
        {
            cache[Key] = Value;

            if (avsPath == null)
                avsPath = INI_FILE_NAME;
            WritePrivateProfileString("Init", Key, Value, avsPath);
        }
    }
}
