using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;

namespace CSNovelCrawler.Plugin
{

    [PluginInformation("sfDownloader", "sf插件", "Montoli", "1.0.0.0", "sf下載插件", "http://www.google.com")]
    public class SfacgPlugin : IPlugin
    {
        public SfacgPlugin()
        {
            Extensions =new Dictionary<string, object>();
        }

        public IDownloader CreateDownloader()
        {
            return new SfacgDownloader();
        }

        public bool CheckUrl(string url)
        {
            Regex r = new Regex(@"^http:\/\/\.*book\.sfacg\.com\/Novel\/(?<TID>\d+)");
            Match m = r.Match(url);
            if (m.Success)
            {
                return true;
            }
            return false;
        }

        public string GetHash(string url)
        {
            Regex r = new Regex(@"^http:\/\/\.*book\.sfacg\.com\/Novel\/(?<TID>\d+)");
            Match m = r.Match(url);
            if (m.Success)
            {

                return "lknovel" + m.Groups["TID"].Value;
            }
            return null;
        }

        public Dictionary<string, object> Extensions { get; private set; }

        public DictionaryExtension<string, string> Configuration { get; set; }
    }
}
