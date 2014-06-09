using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;

namespace CSNovelCrawler.Plugin
{

    [PluginInformation("8comicDownloader", "8comic.cn插件", "Qoo", "1.0.1.0", "無限漫畫下載插件", "http://www.8comic.com/")]
    public class eightcomicPlugin : IPlugin
    {
        public bool CheckUrl(string url)
        {
            Regex r = new Regex(@"^http:\/\/new\.comicvip\.com\/show");
            Match m = r.Match(url);
            if (m.Success)
            {                
                return true;
            }
            return false;
        }

        public IDownloader CreateDownloader()
        {
            return new eightcomicDownloader();
        }

        public string GetHash(string url)
        {
            return "";
                
        }

        public eightcomicPlugin()
        {
            Extensions = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Extensions
        {
            get;
            set;
        }

        public DictionaryExtension<string, string> Configuration { get; set; }
    }
}
