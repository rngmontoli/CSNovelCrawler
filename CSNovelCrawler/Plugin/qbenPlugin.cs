using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;

namespace CSNovelCrawler.Plugin
{
    [PluginInformation("quanBenDownloader", "quanBen插件", "Montoli", "1.0.0.0", "quanBen下載插件", "http://www.google.com")]
    public class qbenPlugin : IPlugin
    {
        public qbenPlugin()
        {
            Extensions =new Dictionary<string, object>();
        }

        public IDownloader CreateDownloader()
        {
            return new qbenDownloader();
        }

        public bool CheckUrl(string url)
        {
            Regex r = new Regex(@"(^http:\/\/\w*\.*quanben\d?.com\/n\/(?<TID>\S+)\/)");
            Match m = r.Match(url);
            if (m.Success)
            {
                return true;
            }
            return false;
        }

        public string GetHash(string url)
        {
            Regex r = new Regex(@"(^http:\/\/\w*\.*quanben\d?.com\/n\/(?<TID>\S+)\/)");
            Match m = r.Match(url);
            if (m.Success)
            {

                return "qBen" + m.Groups["TID"].Value;
            }
            return null;
        }

        public Dictionary<string, object> Extensions { get; private set; }

        public DictionaryExtension<string, string> Configuration { get; set; }
    }
}
