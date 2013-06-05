using System.Text.RegularExpressions;

namespace CSNovelCrawler.Class
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
            txt = Regex.Replace(txt, "&#65279;", string.Empty);
        }
    }
    public class UniformFormat : ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, @"(^\s+)", string.Empty,RegexOptions.Multiline);
            txt = Regex.Replace(txt, @"^(?=\S+)", @"　　", RegexOptions.Multiline);
            txt = Regex.Replace(txt, @"$[\r\n]*", "\r\n\r\n", RegexOptions.Multiline);
        }
    }
}
