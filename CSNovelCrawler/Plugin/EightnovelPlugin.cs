using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;

namespace CSNovelCrawler.Plugin
{

     [PluginInformation("8novelDownloader", "8novel.com插件", "Montoli", "1.0.0.0", "無限小說下載插件", "http://www.biquge.com")]
    public class EightnovelPlugin : IPlugin
    {
         public EightnovelPlugin()
        {
            Extensions =new Dictionary<string, object>();
        }

        public IDownloader CreateDownloader()
        {
            return new EightnovelDownloader();
        }

        public bool CheckUrl(string url)
        {
            Regex r = new Regex(@"^http:\/\/\w*\.*8novel.com(\/books)*(\/novelbook_)(?<TID>\d+)");
            Match m = r.Match(url);
            if (m.Success)
            {
                return true;
            }
            return false;
        }

        public string GetHash(string url)
        {
            Regex r = new Regex(@"^http:\/\/\w*\.*8novel.com(\/books)*(\/novelbook_)(?<TID>\d+)");
            Match m = r.Match(url);
            if (m.Success)
            {

                return "8novel" + m.Groups["TID"].Value;
            }
            return null;
        }

        public Dictionary<string, object> Extensions { get; private set; }

        public DictionaryExtension<string, string> Configuration { get; set; }
    }
}
