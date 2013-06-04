using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace CSNovelCrawler.Plugin
{
    [PluginInformation("eynyDownloader", "eyny.com插件", "Montoli", "1.0.0.0", "伊莉下載插件", "http://www.eyny.com/")]
    public class eynyPlugin : IPlugin
    {
        public eynyPlugin()
		{
            Feature = new Dictionary<string, object>();
            //ConfigForm 属性设置窗口
            Feature.Add("ConfigForm", new MethodInvoker(() =>
            {
                new eynySettingsForm(Configuration).ShowDialog();
            }));
        }

        public IDownloader CreateDownloader()
        {
            return new eynyDownloader();
        }

        public bool CheckUrl(string url)
        {
            Regex r = new Regex(@"(^http:\/\/www\w*\.eyny.com\/thread-\d+-\d+-\w+\.html)");
            Match m = r.Match(url);
            if (m.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
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
