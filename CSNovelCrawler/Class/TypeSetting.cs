using System.Text.RegularExpressions;
using System.Web;

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

    public class PRegex : ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, "<p>", "", RegexOptions.IgnoreCase);
            txt = Regex.Replace(txt, "</p>", "\r\n", RegexOptions.IgnoreCase);
        }
    }

    public class AnnotationRegex : ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, "<!--PAGE.+?-->", "\r\n", RegexOptions.IgnoreCase);
        }
    }


    public class HjwzwRegex : ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, "在搜索引擎輸入(.)*返回書頁", string.Empty, RegexOptions.Singleline);
        }
    }

    //public class RemoveSpecialCharacters:ITypeSetting
    //{
    //    public void Set(ref string txt)
    //    {
    //        txt = Regex.Replace(txt, "&quot;", "\"");
    //        txt = Regex.Replace(txt, "&nbsp;", " ");
    //        txt = Regex.Replace(txt, "&#65279;", string.Empty);
    //    }
    //}
    public class UniformFormat : ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, @"(^\s+)", string.Empty,RegexOptions.Multiline);
            txt = Regex.Replace(txt, @"^(?=\S+)", @"　　", RegexOptions.Multiline);
            txt = Regex.Replace(txt, @"[\r|\n]*$[\r|\n]*", "\r\n\r\n", RegexOptions.Multiline);
        }
    }

    public class Remove0007 : ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, @"([\x00-\x07])", string.Empty);
        }
    }

    public class EynyTag : ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, @"...<div class='locked'><em>瀏覽完整內容，請先 <a href='member.php\?mod=register'>註冊<\/a> 或 <a href='javascript:;' onclick=""lsSubmit\(\)"">登入會員<\/a><\/em><\/div>", string.Empty);
            txt = Regex.Replace(txt, @"<div><\/div>", string.Empty);
        }
    }

    public class Traditional : ITypeSetting
    {
        public void Set(ref string txt)
        {
             txt = CharSetConverter.ToTraditional(txt);
        }
    }

    public class HtmlDecode : ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, @"&#?\w+;", m => HttpUtility.HtmlDecode(m.Value));
        }
    }


    public class SfacgToIndent : ITypeSetting
    {
        public void Set(ref string txt)
        {
            txt = Regex.Replace(txt, "&nbsp; &nbsp; ", "\r\n");
            txt = Regex.Replace(txt, @"<BR>|<br>|<\/?p>", "\r\n");
        }
    }

}
