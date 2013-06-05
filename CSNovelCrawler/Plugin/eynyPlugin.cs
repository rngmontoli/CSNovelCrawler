using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;

namespace CSNovelCrawler.Plugin
{
    [PluginInformation("eynyDownloader", "eyny.com插件", "Montoli", "1.0.0.0", "伊莉下載插件", "http://www.eyny.com/")]
    public class EynyPlugin : IPlugin
    {
        public EynyPlugin()
		{
            Feature = new Dictionary<string, object>
                {
                    {"ConfigForm", new MethodInvoker(() => new EynySettingsForm(Configuration).ShowDialog())}
                };
            //ConfigForm 属性设置窗口
		}

        public IDownloader CreateDownloader()
        {
            return new EynyDownloader(this);
        }

        public bool CheckUrl(string url)
        {
            Regex r = new Regex(@"(^http:\/\/www\w*\.eyny.com\/thread-\d+-\d+-\w+\.html)");
            Match m = r.Match(url);
            if (m.Success)
            {
                return true;
            }
            return false;
        }

        public string GetHash(string url)
        {
            Regex r = new Regex(@"^http:\/\/www\w*\.eyny.com\/thread-(?<TID>\d)+-\d+-\w+\.html");
            Match m = r.Match(url);
            if (m.Success)
            {

                return "eyny" + m.Groups["TID"].Value;
            }
            return null;
        }


        public Dictionary<string, object> Feature { get; private set; }

        public DictionaryExtension<string, string> Configuration { get; set; }
    }
}
