using System;
using System.Collections.Generic;
using CSNovelCrawler.Class;

namespace CSNovelCrawler.Interface
{
    public interface IPlugin
    {
        /// <summary>
        /// 建立IDownloader物件
        /// </summary>
        /// <returns></returns>
        IDownloader CreateDownloader();
        /// <summary>
        /// 檢查url能使用哪個插件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        bool CheckUrl(string url);

        /// <summary>
        /// 取得url的唯一碼
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        string GetHash(string url);

        /// <summary>
        ///插件的擴充功能
        /// </summary>
        Dictionary<String, Object> Extensions { get; } //AutoAnswer(List<AutoAnswer>) ExampleUrl(String[]) ConfigurationForm(MethodInvoker)
        /// <summary>
        /// 插件儲存設定
        /// </summary>
        DictionaryExtension<String, String> Configuration { get; set; }
    }

    /// <summary>
    /// 插件訊息
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
