using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;

namespace CSNovelCrawler.Plugin
{

    [PluginInformation("hjwzwDownloader", "hjwzw.com插件", "Montoli", "1.0.0.0", "黃金屋下載插件", "http://www.biquge.com")]
    public class HjwzwPlugin : IPlugin
    {
        public HjwzwPlugin()
        {
            Extensions =new Dictionary<string, object>();
        }

        public IDownloader CreateDownloader()
        {
            return new HjwzwDownloader();
        }

        public bool CheckUrl(string url)
        {
            Regex r = new Regex(@"^http:\/\/\w*\.*hjwzw.com(\/Book)*(\/Chapter)*\/(?<TID>\d+)");
            Match m = r.Match(url);
            if (m.Success)
            {
                return true;
            }
            return false;
        }

        public string GetHash(string url)
        {
            Regex r = new Regex(@"^http:\/\/\w*\.*hjwzw.com(\/Book)*(\/Chapter)*\/(?<TID>\d+)");
            Match m = r.Match(url);
            if (m.Success)
            {

                return "hjwzw" + m.Groups["TID"].Value;
            }
            return null;
        }

        public Dictionary<string, object> Extensions { get; private set; }

        public DictionaryExtension<string, string> Configuration { get; set; }
    }
}
