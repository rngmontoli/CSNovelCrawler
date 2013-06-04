using CSNovelCrawler.Plugin;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _plugins = new Collection<IPlugin>() 
                { 
                    new eynyPlugin(), 
                    new ck101Plugin()
                };
            foreach (var plugin in _plugins)
            {
                LoadConfiguration(plugin);
            }

        }
        /// <summary>
        /// 获取指定名称的插件
        /// </summary>
        /// <param name="name">插件名称</param>
        /// <returns></returns>
        public IPlugin GetPlugin(string Url)
        {


            if (!string.IsNullOrEmpty(Url))
            {
                foreach (var Plugin in CoreManager.PluginManager.Plugins)
                {
                    try
                    {
                        if (Plugin.CheckUrl(Url)) //检查成功
                        {
                            return Plugin;
                        }
                    }
                    catch (Exception ex)
                    {
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
        /// <param name="startupPath"></param>
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
                    catch { }
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


            string path = Path.Combine(CoreManager.StartupPath, plugin.ToString()+".xml");

            return path;
        }
    }
}
