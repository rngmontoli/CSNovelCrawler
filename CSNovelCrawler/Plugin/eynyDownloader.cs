using System.Globalization;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;
using EncryptAES;
using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;

using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace CSNovelCrawler.Plugin
{
   
    public class EynyDownloader : AbstractDownloader
    {

        public EynyDownloader(IPlugin plugin)
        {
            var aes = new EncryptAes();
            string url = string.Format("http://www02.eyny.com/member.php?mod=logging&action=login&loginsubmit=yes&handlekey=login&loginhash=LiKaw&inajax=1");

            ServicePointManager.Expect100Continue = false;

            string postdata = "jNLWAPIFsJ0iWz7D00C09Fy1nAmQepY1y5cHlwqy0+75fQ1bfPELaZdYi/OKhAghQA0TiEVPd0wsFNCzNcVQNpqObZuZyl3DE18XX+Gwn0WBD7ARSRyDoyl8n0HpXAPIEuJgubT+X9mDY0ncZ5Tl7BnTKl0gJ79WwfclPChuPPU+S3MhyyLx2M/ugEgjDm8BrG7dRNRcXhzMBU6PhqqGLwASVuRjwg4wSvdORanK3GA=";
            if (plugin.Configuration.ContainsKey("PostData"))
            {
                if (!string.IsNullOrEmpty(plugin.Configuration["PostData"].Trim()))
                {
                    postdata = plugin.Configuration["PostData"];
                }
            }

            byte[] data = Encoding.UTF8.GetBytes(aes.DecryptAes256(postdata));
            //建立請求
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
            req.Method = "POST";
            req.CookieContainer = new CookieContainer();

            using (var outstream = req.GetRequestStream())
            {
                outstream.Write(data, 0, data.Length);
                outstream.Flush();
            }
            //關閉請求
            req.GetResponse().Close();
            CurrentParameter = new DownloadParameter
                {
                UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)",
                Cookies = req.CookieContainer
            };
        }
       
        public HtmlDocument GetHtmlDocument(string url)
        {
            CurrentParameter.Url = url;
            return Network.GetHtmlDocument(Network.GetHtmlSource(CurrentParameter, Encoding.UTF8));
        }


        public override bool Analysis()
        {
            //用HtmlAgilityPack分析
            HtmlDocument htmlRoot = GetHtmlDocument(TaskInfo.Url);

            //取作者跟書名
            string htmlTitle = htmlRoot.DocumentNode.SelectSingleNode("/html/head/title").InnerText;
            Regex r = new Regex(@"(?<Author>\S+)\s*-\s*【(?<Title>\S+)】");
            Match m = r.Match(htmlTitle);
            if (m.Success)
            {
                TaskInfo.Author = m.Groups["Author"].Value;
                TaskInfo.Title = m.Groups["Title"].Value;
            }

            //取總頁數
            HtmlNodeCollection nodeHeaders2 = htmlRoot.DocumentNode.SelectNodes("//*[@id=\"pgt\"]/div[1]/div/a");
            string s = nodeHeaders2[nodeHeaders2.Count - 3].InnerText;
            r = new Regex(@"(?<TotalPage>\d+)");
            m = r.Match(s);
            if (m.Success)
            {
                TaskInfo.TotalPage = CommonTools.TryParse(m.Groups["TotalPage"].Value, 0);
            }

            //取TID
            r = new Regex(@"^http:\/\/\w*\.eyny.com\/thread-(?<TID>\d+)-(?<CurrentPage>\d+)-\w+\.html");
            m = r.Match(TaskInfo.Url);
            if (m.Success)
            {
                //taskInfo.CurrentPage = CommonTools.TryParse(m.Groups["CurrentPage"].Value, 0);
                TaskInfo.Tid = m.Groups["TID"].Value;
            }

            TaskInfo.PageSection = GetSection(htmlRoot);
            TaskInfo.TotalSection = TaskInfo.PageSection * (TaskInfo.TotalPage - 1) +
                GetSection(
                    GetHtmlDocument(Regex.Replace(TaskInfo.Url, @"(?!^http:\/\/\w*\.eyny.com\/thread-\d+-)(?<CurrentPage>\d+)(?=-\w+\.html)", TaskInfo.TotalPage.ToString(CultureInfo.InvariantCulture)))
               );
            if (TaskInfo.BeginSection == 0)
            { TaskInfo.BeginSection = 1; }
            if (TaskInfo.EndSection == 0)
            { TaskInfo.EndSection = TaskInfo.TotalSection; }
            return true;
        }

        public int GetSection(HtmlDocument htmlRoot)
        {
            return htmlRoot.DocumentNode.SelectNodes("//*[@id=\"postlist\"]/div/table/tr[1]/td[2]/div[2]/div[2]/div[1]/table[1]/tr[1]/td[1]").Count;
        }


        public override bool Download()
        {
            Regex r = new Regex(@"(?<Head>^http:\/\/\w*\.eyny.com\/thread-\d+-)(?<CurrentPage>\d+)(?<Tail>-\w+\.html)");
            Match m = r.Match(TaskInfo.Url);
            string urlHead = string.Empty, urlTail = string.Empty;
            if (m.Success)
            {
                urlHead = m.Groups["Head"].Value;
                urlTail = m.Groups["Tail"].Value;
            }

            
            HtmlNodeCollection nodeHeaders = null;
            
            int lastPage = 0;
            //排版插件
            var typeSetting = new Collection<ITypeSetting>
                { 
                    new RemoveSpecialCharacters(), 
                    new UniformFormat()
                };
            int numberErrors=0;



            for (; TaskInfo.BeginSection <= TaskInfo.EndSection && !CurrentParameter.IsStop; TaskInfo.BeginSection++)
            {
                //要下載的頁數
                int newCurrentPage = (TaskInfo.BeginSection + TaskInfo.PageSection - 1) / TaskInfo.PageSection;

                if (lastPage != newCurrentPage)//之前下載的頁數跟當前要下載的頁數
                {
                    lastPage = newCurrentPage;//記錄下載頁數，下次如果一樣就不用重抓
                    string url = urlHead + lastPage + urlTail;//組合網址

                    HtmlDocument htmlRoot = GetHtmlDocument(url);

                    if (htmlRoot != null)
                    {
                        nodeHeaders = htmlRoot.DocumentNode.SelectNodes("//*[@id=\"postlist\"]/div/table/tr[1]/td[2]/div[2]/div[2]/div[1]/table[1]/tr[1]/td[1]");
                    }
                }

                try
                {
                    //計算要取的區塊在第幾個
                    int partSection = TaskInfo.BeginSection - ((lastPage - 1) * TaskInfo.PageSection) - 1;
                    if (nodeHeaders != null)
                    {
                        string tempTxt = nodeHeaders[partSection].InnerText;


                        foreach (var item in typeSetting)
                        {

                            item.Set(ref tempTxt);

                        }
                        FileWrite.TxtWrire(tempTxt, TaskInfo.SaveFullPath);

                    }
                }
                catch (Exception)
                {
                    //CoreManager.LoggingManager.Debug(ex.ToString());
                    //發生錯誤，當前區塊重取
                    TaskInfo.BeginSection--;
                    numberErrors++;
                    lastPage = 0;

                    continue;
                }

               
                TaskInfo.HasStopped = CurrentParameter.IsStop;
            }

            bool finish = TaskInfo.CurrentSection == TaskInfo.EndSection;
            return finish;

        }



        
    }
}
