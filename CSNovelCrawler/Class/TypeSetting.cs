using System.Text.RegularExpressions;

namespace CSNovelCrawler.Class
{
    public interface ITypeSetting
    {
        void Set(ref string txt);
    }
    public class BrRegex : ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, "<BR>", "\r\n", RegexOptions.IgnoreCase);
        }
    }

    public class HjwzwRegex : ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, "在搜索引擎輸入(.)*返回書頁", string.Empty, RegexOptions.Singleline);
        }
    }

    public class RemoveSpecialCharacters:ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, "&quot;", "\"");
            txt = Regex.Replace(txt, "&nbsp;", " ");
            txt = Regex.Replace(txt, "&#65279;", string.Empty);
        }
    }
    public class UniformFormat : ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, @"(^\s+)", string.Empty,RegexOptions.Multiline);
            txt = Regex.Replace(txt, @"^(?=\S+)", @"　　", RegexOptions.Multiline);
            txt = Regex.Replace(txt, @"[\r\n]*$[\r\n]*", "\r\n\r\n", RegexOptions.Multiline);
        }
    }

    public class Traditional : ITypeSetting
    {
        public void Set(ref string txt)
        {
             txt = CharSetConverter.ToTraditional(txt);
        }
    }

     

}
