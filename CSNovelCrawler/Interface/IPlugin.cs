using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CSNovelCrawler
{
    public interface IPlugin
    {
        /// <summary>
        /// 创建新的下载器实例
        /// </summary>
        /// <returns></returns>
        IDownloader CreateDownloader();
        /// <summary>
        /// 检查指定的Url是否符合能够被当前插件解析
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        bool CheckUrl(string url);
        /// <summary>
        /// 取得指定Url的Hash值
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        string GetHash(string url);

        /// <summary>
        /// 插件支持的UI特性
        /// </summary>
        Dictionary<String, Object> Feature { get; } //AutoAnswer(List<AutoAnswer>) ExampleUrl(String[]) ConfigurationForm(MethodInvoker)
        /// <summary>
        /// 插件独立存储
        /// </summary>
        DictionaryExtension<String, String> Configuration { get; set; }
    }

    /// <summary>
    /// AcDown插件信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class PluginInformationAttribute : Attribute
    {
        /// <summary>
        /// 內部名稱
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 易記名稱
        /// </summary>
        public string FriendlyName { get; private set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; private set; }
        /// <summary>
        /// 版本（0.0.0.0）
        /// </summary>
        public Version Version { get; private set; }
        /// <summary>
        /// 詳細描述
        /// </summary>
        public string Describe { get; private set; }
        /// <summary>
        /// 網址
        /// </summary>
        public string SupportUrl { get; private set; }

        /// <summary>
        /// 建立PluginInformationAttribute的實體
        /// </summary>
        /// <param name="name">內部名稱</param>
        /// <param name="friendlyName">易記名稱</param>
        /// <param name="author">作者</param>
        /// <param name="version">版本</param>
        /// <param name="describe">詳細描述</param>
        /// <param name="supportUrl">網址</param>
        public PluginInformationAttribute(string name, string friendlyName, string author, string version, string describe, string supportUrl)
        {
            Name = name;
            FriendlyName = friendlyName;
            Author = author;
            Version = new Version(version);
            Describe = describe;
            SupportUrl = supportUrl;
        }
    }
}
