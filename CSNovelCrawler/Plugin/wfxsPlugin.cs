using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;

namespace CSNovelCrawler.Plugin
{
    [PluginInformation("wfxsDownloader", "wfxs插件", "Montoli", "1.0.0.0", "wfxs下載插件", "http://www.google.com")]
    public class wfxsPlugin : IPlugin
    {
        public wfxsPlugin()
        {
            Extensions =new Dictionary<string, object>();
        }

        public IDownloader CreateDownloader()
        {
            return new wfxsDownloader();
        }

        public bool CheckUrl(string url)
        {
            Regex r = new Regex(@"(^https?:\/\/\w*\.*wfxs\d?.org\/(html|book)\/(?<TID>\d+))");
            Match m = r.Match(url);
            if (m.Success)
            {
                return true;
            }
            return false;
        }

        public string GetHash(string url)
        {
            Regex r = new Regex(@"(^https?:\/\/\w*\.*wfxs\d?.org\/(html|book)\/(?<TID>\d+))");
            Match m = r.Match(url);
            if (m.Success)
            {

                return "wfxs" + m.Groups["TID"].Value;
            }
            return null;
        }

        public Dictionary<string, object> Extensions { get; private set; }

        public DictionaryExtension<string, string> Configuration { get; set; }
    }
}
