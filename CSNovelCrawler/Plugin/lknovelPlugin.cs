using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;

namespace CSNovelCrawler.Plugin
{

    [PluginInformation("LknovelDownloader", "lknovel.lightnovel.cn插件", "Montoli", "1.1.0.0", "轻之国度轻小说文库下載插件", "http://lknovel.lightnovel.cn/")]
    public class LknovelPlugin : IPlugin
    {
        public LknovelPlugin()
        {
            Extensions =new Dictionary<string, object>();
        }

        public IDownloader CreateDownloader()
        {
            return new LknovelDownloader();
        }

        public bool CheckUrl(string url)
        {
            Regex r = new Regex(@"^http:\/\/\w*\.*lknovel\.lightnovel.cn\/main\/vollist\/(?<TID>\d+)");
            Match m = r.Match(url);
            if (m.Success)
            {
                return true;
            }
            return false;
        }

        public string GetHash(string url)
        {
            Regex r = new Regex(@"^http:\/\/\w*\.*lknovel\.lightnovel.cn\/main\/vollist\/(?<TID>\d+)");
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
