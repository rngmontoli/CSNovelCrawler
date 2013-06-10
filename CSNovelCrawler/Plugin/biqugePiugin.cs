using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;

namespace CSNovelCrawler.Plugin
{
    [PluginInformation("biqugeDownloader", "biquge.com插件", "Montoli", "1.0.1.0", "笔趣阁下載插件", "http://www.biquge.com")]
    public class BiqugePiugin : IPlugin
    {
        public BiqugePiugin()
        {
            Extensions =new Dictionary<string, object>();
        }

        public IDownloader CreateDownloader()
        {
            return new BiqugeDownloader();
        }

        public bool CheckUrl(string url)
        {
            Regex r = new Regex(@"^http:\/\/\w*\.*biquge.com\/(?<TID>\d+_\d+)");
            Match m = r.Match(url);
            if (m.Success)
            {
                return true;
            }
            return false;
        }

        public string GetHash(string url)
        {
            Regex r = new Regex(@"^http:\/\/\w*\.*biquge.com\/(?<TID>\d+_\d+)");
            Match m = r.Match(url);
            if (m.Success)
            {

                return "biquge" + m.Groups["TID"].Value;
            }
            return null;
        }

        public Dictionary<string, object> Extensions { get; private set; }

        public DictionaryExtension<string, string> Configuration { get; set; }
    }
}
