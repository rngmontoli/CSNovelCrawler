using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using CSNovelCrawler.Core;
using HtmlAgilityPack;

namespace CSNovelCrawler.Class
{
    public class Network
    {

        public static HtmlDocument GetHtmlDocument(string htmlSource)
        {

            var htmlRoot = new HtmlDocument();
            htmlRoot.LoadHtml(htmlSource);

            return htmlRoot;
        }

        public static void RemoveSubHtmlNode(HtmlNode curHtmlNode, string subNodeToRemove)
        {
            
            try
            {
                var foundAllSub = curHtmlNode.SelectNodes(subNodeToRemove);
                if (foundAllSub != null)
                {
                    foreach (HtmlNode subNode in foundAllSub)
                    {
                        curHtmlNode.RemoveChild(subNode);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            //return curHtmlNode;
        }

        /// <summary>
        /// 取得網頁網始碼
        /// </summary>
        /// <param name="para"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string GetHtmlSource( DownloadParameter para, System.Text.Encoding encode)
        {
            return GetHtmlSource( para, encode, new WebProxy());
        }

        /// <summary>
        /// 取得網頁網始碼
        /// </summary>
        /// <param name="request"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string GetHtmlSource(HttpWebRequest request, System.Text.Encoding encode)
        {
            string sline = "";
            bool needRedownload = false;
            int remainTimes = 3;
            
            do
            {
                try
                {
                    //接收 HTTP 回應
                    var res = (HttpWebResponse)request.GetResponse();
                    var responseStream = res.GetResponseStream();
                    if (responseStream == null) throw new Exception();
                    StreamReader reader;
                    switch (res.ContentEncoding)
                    {
                        case "gzip":
                            //Gzip解壓縮
                            using (var gzip = new GZipStream(responseStream, CompressionMode.Decompress))
                            {
                                reader = new StreamReader(gzip, encode);

                                sline = reader.ReadToEnd();

                            }
                            break;
                        case "deflate":
                            //deflate解壓縮
                            using (var deflate = new DeflateStream(responseStream, CompressionMode.Decompress))
                            {
                                reader = new StreamReader(deflate, encode);

                                sline = reader.ReadToEnd();

                            }
                            break;
                        default:
                            reader = new StreamReader(responseStream, encode);
                        
                            sline = reader.ReadToEnd();
                            break;
                    }

                }
                catch (Exception ex) 
                {
                    //重試等待時間
                    Thread.Sleep(3000);
                    needRedownload = true;
                  
                    //重試次數-1
                    remainTimes--;
                    //如果重試次數小於0，拋出錯誤
                    if (remainTimes < 0)
                    {
                        needRedownload = false;
                        CoreManager.LogManager.Debug(ex.ToString());
                        
                    }
                    
                   
                }
            } while (needRedownload);
            return sline;
        }

        /// <summary>
        /// 取得網頁網始碼
        /// </summary>
        /// <param name="para"></param>
        /// <param name="encode"></param>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public static string GetHtmlSource(DownloadParameter para, System.Text.Encoding encode, WebProxy proxy)
        {

            //再來建立你要取得的Request
            var webReq = (HttpWebRequest)WebRequest.Create(para.Url);
            webReq.ContentType = "application/x-www-form-urlencoded";
            webReq.Accept = "text/html, application/xhtml+xml, */*";
            webReq.Headers.Set("Accept-Language", "zh-TW");
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
