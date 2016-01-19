using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using CSNovelCrawler.Class;
using HtmlAgilityPack;

namespace CSNovelCrawler.Plugin
{
    internal class Ck101Downloader : AbstractDownloader
    {
        public Ck101Downloader()
        {
            CurrentParameter = new DownloadParameter
                {
                    UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)",
                    Timeout = 10000
                };
        }

        public HtmlDocument GetHtmlDocument(string url)
        {
            CurrentParameter.Url = url;
            var HtmlSource = Network.GetHtmlSource(CurrentParameter, Encoding.UTF8);
            return Network.GetHtmlDocument(HtmlSource);
        }

        public HtmlDocument GetHtmlDocumentReplaceDivToEmpty(string url)
        {
            CurrentParameter.Url = url;
            var HtmlSource = Network.GetHtmlSource(CurrentParameter, Encoding.UTF8);
            HtmlSource = Regex.Replace(HtmlSource, @"<\/?div[^<]*>", string.Empty, RegexOptions.Singleline);
            HtmlSource = Regex.Replace(HtmlSource, @"<!--[^<]*-->", string.Empty, RegexOptions.Singleline);
            //HtmlSource = Regex.Replace(HtmlSource, @"\(adsbygoogle .*\);", string.Empty, RegexOptions.Singleline);
            return Network.GetHtmlDocument(HtmlSource);
        }

        /// <summary>
        /// 取得網頁上的基本資料
        /// </summary>
        public override bool Analysis()
        {
            //取TID
            Regex r = new Regex(@"^http:\/\/\w*\.*ck101.com\/thread-(?<TID>\d+)-(?<CurrentPage>\d+)-\w+\.html");
            Match m = r.Match(TaskInfo.Url);
            if (m.Success)
            {
                //taskInfo.CurrentPage = CommonTools.TryParse(m.Groups["CurrentPage"].Value, 0);
                TaskInfo.Tid = m.Groups["TID"].Value;
            }

            TaskInfo.Url = string.Format("http://ck101.com/thread-{0}-1-1.html", TaskInfo.Tid);

            //用HtmlAgilityPack分析
            HtmlDocument htmlRoot = GetHtmlDocument(TaskInfo.Url);

            ////取作者跟書名
            string htmlTitle = htmlRoot.DocumentNode.SelectSingleNode("/html/head/title").InnerText;
            r = new Regex(@"(?<Title>(.(?!【))+)[^\u4e00-\u9fa5a-zA-Z0-9]*作者[^\u4e00-\u9fa5a-zA-Z0-9]*(?<Author>[\u0800-\u9fa5\x3130-\x318Fa-zA-Z0-9]+)");
            m = r.Match(htmlTitle);
            if (m.Success)
            {
                TaskInfo.Author = m.Groups["Author"].Value.Trim();
                TaskInfo.Title = m.Groups["Title"].Value.Trim();
            }

            //取總頁數
            HtmlNodeCollection nodeHeaders2 = htmlRoot.DocumentNode.SelectNodes("//*[@id=\"pgt\"]/div[1]/div/a");
            if (nodeHeaders2 != null)
            {
                string s = nodeHeaders2[nodeHeaders2.Count - 2].InnerText;
                r = new Regex(@"(?<TotalPage>\d+)");
                m = r.Match(s);
                if (m.Success)
                {
                    TaskInfo.TotalPage = CommonTools.TryParse(m.Groups["TotalPage"].Value, 0);
                }

                TaskInfo.PageSection = GetSection(
                        GetHtmlDocument(Regex.Replace(TaskInfo.Url, @"(?!^http:\/\/\w*\.*ck101.com\/thread-\d+-)(?<CurrentPage>\d+)(?=-\w+\.html)", (TaskInfo.TotalPage - 1).ToString(CultureInfo.InvariantCulture)))
                   );
                TaskInfo.TotalSection = TaskInfo.PageSection * (TaskInfo.TotalPage - 1) +
                    GetSection(
                        GetHtmlDocument(Regex.Replace(TaskInfo.Url, @"(?!^http:\/\/\w*\.*ck101.com\/thread-\d+-)(?<CurrentPage>\d+)(?=-\w+\.html)", TaskInfo.TotalPage.ToString(CultureInfo.InvariantCulture)))
                   );
            }

            if (TaskInfo.BeginSection == 0)
            { TaskInfo.BeginSection = 1; }
            if (TaskInfo.EndSection == 0)
            { TaskInfo.EndSection = TaskInfo.TotalSection; }

            return true;
        }

        /// <summary>
        /// 取頁的樓層數
        /// </summary>
        /// <param name="htmlRoot"></param>
        /// <returns></returns>
        public int GetSection(HtmlDocument htmlRoot)
        {
            return htmlRoot.DocumentNode.SelectNodes("//*[@class=\"t_f\"]").Count;
        }

        public override bool Download()
        {
            CurrentParameter.IsStop = false;

            Regex r = new Regex(@"(?<Head>^http:\/\/\w*\.*ck101.com\/thread-\d+-)(?<CurrentPage>\d+)(?<Tail>-\w+\.html)");
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
                    new HtmlDecode(),
                    new UniformFormat()
                };

            for (; TaskInfo.BeginSection <= TaskInfo.EndSection && !CurrentParameter.IsStop; TaskInfo.BeginSection++)
            {
                //要下載的頁數

                try
                {
                    int newCurrentPage = (TaskInfo.BeginSection + TaskInfo.PageSection - 1) / TaskInfo.PageSection;

                    if (lastPage != newCurrentPage)//之前下載的頁數跟當前要下載的頁數
                    {
                        lastPage = newCurrentPage;//記錄下載頁數，下次如果一樣就不用重抓
                        string url = urlHead + lastPage.ToString(CultureInfo.InvariantCulture) + urlTail;//組合網址

                        if (lastPage == 1)//卡提諾第一頁的特別處理
                        {
                            switch (TaskInfo.FailTimes % 2)//常常取不到完整資料，用多個網址取
                            {
                                case 0:
                                    url = string.Format("http://ck101.com/thread-{0}-1-1.html", TaskInfo.Tid);
                                    break;

                                case 1:
                                    url = string.Format("http://m.ck101.com/forum.php?mod=redirect&ptid={0}&authorid=0&postno=1", TaskInfo.Tid);
                                    break;

                                case 2:
                                    url = string.Format("http://m.ck101.com/forum.php?mod=redirect&ptid={0}&authorid=0&postno=1", TaskInfo.Tid);
                                    break;
                            }
                        }

                        HtmlDocument htmlRoot = GetHtmlDocumentReplaceDivToEmpty(url);

                        if (htmlRoot != null)
                        {
                            nodeHeaders = htmlRoot.DocumentNode.SelectNodes("//*[@class=\"t_f\"]");
                        }
                    }

                    //計算要取的區塊在第幾個
                    int partSection = TaskInfo.BeginSection - ((lastPage - 1) * TaskInfo.PageSection) - 1;
                    if (nodeHeaders == null)
                    {
                        throw new Exception("下載資料為空的");
                    }
                    Network.RemoveSubHtmlNode(nodeHeaders[partSection], "div");
                    Network.RemoveSubHtmlNode(nodeHeaders[partSection], "ignore_js_op");
                    Network.RemoveSubHtmlNode(nodeHeaders[partSection], "i");
                    Network.RemoveSubHtmlNode(nodeHeaders[partSection], "script");
                    string tempTxt = nodeHeaders[partSection].InnerText;

                    foreach (var item in typeSetting)
                    {
                        item.Set(ref tempTxt);
                    }
                    FileWrite.TxtWrire(tempTxt, TaskInfo.SaveFullPath, TaskInfo.TextEncoding);
                }
                catch (Exception)
                {
                    //CoreManager.LoggingManager.Debug(ex.ToString());
                    //發生錯誤，當前區塊重取
                    TaskInfo.BeginSection--;
                    TaskInfo.FailTimes++;
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
