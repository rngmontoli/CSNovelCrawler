using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSNovelCrawler
{
    public class Network
    {

        public static HtmlAgilityPack.HtmlDocument GetHtmlDocument(string HtmlSource)
        {
            HtmlAgilityPack.HtmlDocument HtmlRoot = new HtmlAgilityPack.HtmlDocument();
            HtmlRoot.LoadHtml(HtmlSource);
            return HtmlRoot;
        }

        /// <summary>
        /// 获取网页源代码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string GetHtmlSource( DownloadParameter para, System.Text.Encoding encode)
        {
            return GetHtmlSource( para, encode, new WebProxy());
        }

        /// <summary>
        /// 获取网页源代码
        /// </summary>
        /// <param name="request"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string GetHtmlSource(HttpWebRequest request, System.Text.Encoding encode)
        {
            string sline = "";
            bool needRedownload = false;
            int remainTimes = 3;
            //当需要重试下载时
            do
            {
                try
                {
                    //获取服务器回应
                    HttpWebResponse res = (HttpWebResponse)request.GetResponse();
                    if (res.ContentEncoding == "gzip")
                    {
                        //Gzip解压缩
                        using (GZipStream gzip = new GZipStream(res.GetResponseStream(), CompressionMode.Decompress))
                        {
                            using (StreamReader reader = new StreamReader(gzip, encode))
                            {
                                sline = reader.ReadToEnd();
                            }
                        }
                    }
                    else if (res.ContentEncoding == "deflate")
                    {
                        //deflate解压缩
                        using (DeflateStream deflate = new DeflateStream(res.GetResponseStream(), CompressionMode.Decompress))
                        {
                            using (StreamReader reader = new StreamReader(deflate, encode))
                            {
                                sline = reader.ReadToEnd();
                            }
                        }
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(res.GetResponseStream(), encode))
                        {
                            sline = reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex) //发生错误
                {
                    //重试次数-1
                    remainTimes--;
                    //如果重试次数小于0，抛出错误
                    if (remainTimes < 0)
                    {
                        needRedownload = false;
                        throw;
                    }
                    else
                    {
                        //等待时间
                        Thread.Sleep(1000);
                        needRedownload = true;
                    }
                }
            } while (needRedownload);
            return sline;
        }

        /// <summary>
        /// 取得网页源代码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string GetHtmlSource(DownloadParameter para, System.Text.Encoding encode, WebProxy proxy)
        {

            //再來建立你要取得的Request
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(para.Url);
            //webReq.ContentType = "application/x-www-form-urlencoded";
            //webReq.Accept = "text/html, application/xhtml+xml, */*";
            //webReq.Headers.Set("Accept-Language", "zh-TW");
            webReq.UserAgent = para.UserAgent;
            webReq.Headers.Set("Accept-Encoding", "gzip, deflate");
            //webReq.Host = "www09.eyny.com";
            webReq.KeepAlive = true;
            //將剛剛取得的cookie加上去
            webReq.CookieContainer = para.Cookies;

            //webReq.Proxy = proxy;
            return GetHtmlSource(webReq, encode);
        }
       


    }


    /// <summary>
    /// 下载参数
    /// </summary>
    public class DownloadParameter
    {
        /// <summary>
        /// 资源的网络位置
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 要创建的本地文件位置
        /// </summary>
        public string FilePath { get; set; }


        /// <summary>
        /// 是否停止下载(可以在下载过程中进行设置，用来控制下载过程的停止)
        /// </summary>
        public bool IsStop { get; set; }

        /// <summary>
        /// 读取或设置发出请求时使用的Cookie
        /// </summary>
        public CookieContainer Cookies { get; set; }

        /// <summary>
        /// 读取或设置下载请求所使用的Referer值
        /// </summary>
        public string Referer { get; set; }
        /// <summary>
        /// 读取或设置下载请求所使用的User-Agent值
        /// </summary>
        public string UserAgent { get; set; }
    }
}
