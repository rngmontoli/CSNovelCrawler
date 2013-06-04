using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSNovelCrawler
{
    public class CommonTools
    {
        public static int TryParse(string str,int DefaultNum)
        {
            int.TryParse(str, out DefaultNum);
            return DefaultNum;
        }
       
    }
}
