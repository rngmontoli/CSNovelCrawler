using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using CSNovelCrawler.Class;
using HtmlAgilityPack;

namespace CSNovelCrawler.Plugin
{

    public class LknovelDownloader : AbstractDownloader
    {
        public LknovelDownloader()
        {
            CurrentParameter = new DownloadParameter
                {
                    UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)",
                };
        }



        public HtmlDocument GetHtmlDocument(string url)
        {
            CurrentParameter.Url = url;
            return Network.GetHtmlDocument(Network.GetHtmlSource(CurrentParameter, Encoding.UTF8));
        }

        /// <summary>
        /// 取得網頁上的基本資料
        /// </summary>
        public override bool Analysis()
        {




            //取TID
            Regex r = new Regex(@"^http:\/\/\w*\.*lknovel\.lightnovel.cn\/main\/vollist\/(?<TID>\d+)");
            Match m = r.Match(TaskInfo.Url);
            if (m.Success)
            {
                TaskInfo.Tid = m.Groups["TID"].Value;
            }

            TaskInfo.Url = string.Format("http://lknovel.lightnovel.cn/main/vollist/{0}.html?charset=big5", TaskInfo.Tid);



            //用HtmlAgilityPack分析
            HtmlDocument htmlRoot = GetHtmlDocument(TaskInfo.Url);


            ////取作者跟書名
            var htmlNode = htmlRoot.DocumentNode.SelectSingleNode("/html/body/div[3]/div[1]/div[1]/div[1]/div[2]");
            TaskInfo.Title =
               htmlNode.SelectSingleNode("h1[1]/strong[1]")
                        .InnerHtml;
            TaskInfo.Author =
                htmlNode.SelectSingleNode(
                    "table[1]/tr[1]/td[2]/a[1]").InnerHtml;
 

            TaskInfo.TotalSection = SectionNames.Count;


            if (TaskInfo.BeginSection == 0)
            {
                TaskInfo.BeginSection = 1;
            }
            if (TaskInfo.EndSection == 0)
            {
                TaskInfo.EndSection = TaskInfo.TotalSection;
            }

            return true;

            //htmlRoot.DocumentNode.SelectSingleNode("//*[@id=\"list\"]/dl/dd").InnerHtml;

        }
        private List<int> _sectionNames;

        private List<int> SectionNames
        {
            get
            {
                if (_sectionNames == null || _sectionNames.Count==0)
                {
                    _sectionNames = new List<int>();
                    GetTotalSection();

                }
                return _sectionNames;

            }
        }

        /// <summary>
        /// 取目錄
        /// </summary>
        public void GetTotalSection()
        {
            HtmlDocument htmlRoot = GetHtmlDocument(TaskInfo.Url);
            Regex r = new Regex(@"<a href=\Shttp:\/\/\w*\.*lknovel\.lightnovel.cn\/main\/view\/(?<SectionName>\d+)");
            MatchCollection matchs = r.Matches(htmlRoot.DocumentNode.SelectSingleNode("html/body/div[3]/div").InnerHtml);
            foreach (Match m in matchs)
            {
                int temp = CommonTools.TryParse(m.Groups["SectionName"].Value, 0);
                if (!_sectionNames.Contains(temp))
                {
                    _sectionNames.Add(temp);
                }
            }
            _sectionNames.Sort();
        }


        public override bool Download()
        {
            CurrentParameter.IsStop = false;





            string urlHead = "http://lknovel.lightnovel.cn/main/view/";
            string urlTail = ".html?charset=big5";

            //排版插件
            var typeSetting = new Collection<ITypeSetting>
                { 
                    new HtmlDecode(),
                    new UniformFormat()
                };


            for (; TaskInfo.BeginSection <= TaskInfo.EndSection && !CurrentParameter.IsStop; TaskInfo.BeginSection++)
            {
                string url = urlHead + SectionNames[TaskInfo.CurrentSection].ToString(CultureInfo.InvariantCulture) + urlTail;//組合網址

                HtmlDocument htmlRoot = GetHtmlDocument(url);


                try
                {
                    var nodeHeaders =
                        htmlRoot.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div/div[1]/div[1]/div[2]");
                    string tempTextFile = nodeHeaders.InnerText;

                    foreach (var item in typeSetting)
                    {
                        item.Set(ref tempTextFile);
                    }
                    FileWrite.TxtWrire(tempTextFile, TaskInfo.SaveFullPath, TaskInfo.TextEncoding);


                }
                catch (Exception)
                {
                    //CoreManager.LoggingManager.Debug(ex.ToString());
                    //發生錯誤，當前區塊重取
                    TaskInfo.BeginSection--;
                    TaskInfo.FailTimes++;

                    continue;
                }

                TaskInfo.HasStopped = CurrentParameter.IsStop;
            }

            bool finish = TaskInfo.CurrentSection == TaskInfo.EndSection;
            return finish;
        }
    }
}
