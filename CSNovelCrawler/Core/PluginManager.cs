
using System.Collections.Generic;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;
using CSNovelCrawler.Plugin;
using System;
using System.IO;
using System.Xml.Serialization;

namespace CSNovelCrawler.Core
{
    public class PluginManager
    {
        private List<IPlugin> _plugins;
        public List<IPlugin> Plugins
        {
            get
            {
                return _plugins;
            }
        }

        public PluginManager()
        {
            _plugins = new List<IPlugin>
                { 
                    new EynyPlugin(), 
                    new Ck101Plugin(),
                    new BiqugePiugin(),
                    new LknovelPlugin(),
                    new RanwenPlugin(),
                    new HjwzwPlugin(),
                    new EightnovelPlugin(),
                    new SfacgPlugin(),
                    new qbenPlugin(),
                    new wfxsPlugin(),
                    //new eightcomicPlugin()
                    
                };
            foreach (var plugin in _plugins)
            {
                LoadConfiguration(plugin);
            }

        }

        /// <summary>
        /// 尋找對應網址的插件
        /// </summary>
        /// <returns></returns>
        public IPlugin GetPlugin(string url)
        {

            if (!string.IsNullOrEmpty(url))
            {

                return CoreManager.PluginManager.Plugins.Find(plugin => plugin.CheckUrl(url));
            }

            return null;
        }

        /// <summary>
        /// 儲存插件設定
        /// </summary>
        /// <param name="plugin">需要儲存的插件</param>
        /// <returns>儲存成功為True，反之False</returns>
        public bool SaveConfiguration(IPlugin plugin)
        {
            try
            {
                string path = GetSettingFilePath(plugin);
                //建立文件夹
                if (!Directory.Exists(path)) Directory.CreateDirectory(Path.GetDirectoryName(path));

                //反序列化插件设置
                XmlSerializer oXmlSerializer = new XmlSerializer(typeof(DictionaryExtension<string, string>));
                using (FileStream oFileStream = new FileStream(path, FileMode.Create))
                {
                    oXmlSerializer.Serialize(oFileStream, plugin.Configuration);
                }
                return true;
            }
            catch (Exception ex)
            {
                CoreManager.LogManager.Debug(ex.ToString());
                return false;
            }

        }

        /// <summary>
        /// 讀取插件設定
        /// </summary>
        /// <param name="plugin"></param>
        private void LoadConfiguration(IPlugin plugin)
        {
            string path = GetSettingFilePath(plugin);
            //如果檔案存在
            if (File.Exists(path))
            {
                //反序列化插件設定
                XmlSerializer oFileStream = new XmlSerializer(typeof(DictionaryExtension<string, string>));
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    try
                    {
                        plugin.Configuration = (DictionaryExtension<string, string>) oFileStream.Deserialize(fs);
                    }
                    catch (Exception ex)
                    {
                        CoreManager.LogManager.Debug(ex.ToString());
                    }
                }
            }
            plugin.Configuration = plugin.Configuration ?? new DictionaryExtension<string, string>();
           
        }

        /// <summary>
        /// 取得插件設定的檔案路徑
        /// </summary>
        /// <param name="plugin"></param>
        /// <returns></returns>
        private string GetSettingFilePath(IPlugin plugin)
        {
            string path = Path.Combine(CoreManager.StartupPath, plugin+".xml");
            return path;
        }
    }
}
