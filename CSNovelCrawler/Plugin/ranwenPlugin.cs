using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;

namespace CSNovelCrawler.Plugin
{
    [PluginInformation("ranwenDownloader", "ranwen.net插件", "Montoli", "1.0.0.0", "燃文下載插件", "http://www.biquge.com")]
    public class RanwenPlugin : IPlugin
    {
        public RanwenPlugin()
        {
            Extensions =new Dictionary<string, object>();
        }

        public IDownloader CreateDownloader()
        {
            return new RanwenDownloader();
        }

        public bool CheckUrl(string url)
        {
            Regex r = new Regex(@"(^http:\/\/\w*\.*ranwen.net\/info_(?<TID>\d+))|(^http:\/\/\w*\.*ranwen.net(\/files)*(\/article)*\/\d+\/(?<TID>\d+))");
            Match m = r.Match(url);
            if (m.Success)
            {
                return true;
            }
            return false;
        }

        public string GetHash(string url)
        {
            Regex r = new Regex(@"(^http:\/\/\w*\.*ranwen.net\/info_(?<TID>\d+))|(^http:\/\/\w*\.*ranwen.net(\/files)*(\/article)*\/\d+\/(?<TID>\d+))");
            Match m = r.Match(url);
            if (m.Success)
            {

                return "ranwen" + m.Groups["TID"].Value;
            }
            return null;
        }

        public Dictionary<string, object> Extensions { get; private set; }

        public DictionaryExtension<string, string> Configuration { get; set; }
    }
}
