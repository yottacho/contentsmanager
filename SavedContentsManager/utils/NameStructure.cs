using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavedContentsManager.utils
{
    class NameStructure
    {
        private string fullName = "";

        /// <summary>
        /// 전체 이름
        /// </summary>
        public string FullName
        {
            get
            {
                return fullName;
            }
            set
            {
                fullName = value;
                setPrefix();
            }
        }

        /// <summary>
        /// 프리픽스 여부
        /// </summary>
        public bool isPrefix { get; set; } = false;

        /// <summary>
        /// 프리픽스
        /// </summary>
        public string Prefix { get; set; } = "";

        /// <summary>
        /// 프리픽스를 제외한 이름
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// 최종 생성 시각
        /// </summary>
        public string LastTime { get; set; } = "";

        /// <summary>
        /// 변경이 일어날 경우 변경 후 전체 이름
        /// </summary>
        public string TargetFullName { get; set; } = "";

        public NameStructure(string fullName)
        {
            this.FullName = fullName;
        }

        public bool setPrefix()
        {
            isPrefix = false;
            Name = fullName;

            // 이름에 공백이 하나 있는지 확인
            int idx = fullName.IndexOf(' ');
            if (idx <= 0)
                return isPrefix;

            // 숫자로 변환이 가능한 지 확인
            string chk = fullName.Substring(0, idx);
            int toInt = 0;
            if (int.TryParse(chk, out toInt))
            {
                Prefix = fullName.Substring(0, idx);
                Name = fullName.Substring(idx + 1);
                isPrefix = true;
            }

            Console.WriteLine(isPrefix + " [" + Prefix + "][" + Name + "]");
            return isPrefix;
        }
    }
}
