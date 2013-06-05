using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSNovelCrawler
{
    public interface ITypeSetting
    {
        void Set(ref string txt);
    }

    public class RemoveSpecialCharacters:ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, "google_.*", string.Empty);
            txt = Regex.Replace(txt, "&nbsp;", string.Empty);
        }
    }
    public class UniformFormat : ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, @"(^\s+)", string.Empty,RegexOptions.Multiline);
            txt = Regex.Replace(txt, @"^(?=\S+)", @"　　", RegexOptions.Multiline);
            txt = Regex.Replace(txt, @"$", "\r\n\r\n", RegexOptions.Multiline);
        }
    }
}
