using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;
using CSNovelCrawler.Plugin;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace CSNovelCrawler.Core
{
    public class PluginManager
    {
        private Collection<IPlugin> _plugins;
        public Collection<IPlugin> Plugins
        {
            get
            {
                return _plugins;
            }
        }

        public PluginManager()
        {
            _plugins = new Collection<IPlugin>
                { 
                    new EynyPlugin(), 
                    new Ck101Plugin()
                };
            foreach (var plugin in _plugins)
            {
                LoadConfiguration(plugin);
            }

        }

        /// <summary>
        /// 获取指定名称的插件
        /// </summary>
        /// <returns></returns>
        public IPlugin GetPlugin(string url)
        {


            if (!string.IsNullOrEmpty(url))
            {
                foreach (var plugin in CoreManager.PluginManager.Plugins)
                {
                    try
                    {
                        if (plugin.CheckUrl(url)) //检查成功
                        {
                            return plugin;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            return null;
            //foreach (var item in _plugins)
            //{
            //    try
            //    {
            //        var attrib = GetAttr(item);
            //        if (attrib.Name == name)
            //            return item;
            //    }
            //    catch { }
            //}
            //return null;
        }

        /// <summary>
        /// 保存插件配置
        /// </summary>
        /// <param name="plugin">需要保存配置的插件引用</param>
        /// <returns>如果保存成功返回true，失败为false</returns>
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
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 读取插件配置
        /// </summary>
        /// <param name="plugin"></param>
        private void LoadConfiguration(IPlugin plugin)
        {
            string path = GetSettingFilePath(plugin);
            //如果文件存在则反序列化设置
            if (File.Exists(path))
            {
                //反序列化插件设置
                XmlSerializer oFileStream = new XmlSerializer(typeof(DictionaryExtension<string, string>));
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    try
                    {
                        plugin.Configuration = (DictionaryExtension<string, string>)oFileStream.Deserialize(fs);
                    }
                    catch (Exception)
                    { }
                }
            }
            plugin.Configuration = plugin.Configuration ?? new DictionaryExtension<string, string>();
            //设置启动路径
            plugin.Configuration["StartupPath"] = Path.GetDirectoryName(path);
        }

        /// <summary>
        /// 取得插件配置文件所在的地址
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
