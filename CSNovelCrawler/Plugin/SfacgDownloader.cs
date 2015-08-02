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

    public class SfacgDownloader : AbstractDownloader
    {
        public SfacgDownloader()
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
            Regex r = new Regex(@"^http:\/\/\.*book\.sfacg\.com\/Novel\/(?<TID>\d+)");
            Match m = r.Match(TaskInfo.Url);
            if (m.Success)
            {
                TaskInfo.Tid = m.Groups["TID"].Value;
            }

            TaskInfo.Url = string.Format("http://book.sfacg.com/Novel/{0}/MainIndex/", TaskInfo.Tid);

            string introductionUrl = string.Format("http://book.sfacg.com/Novel/{0}", TaskInfo.Tid);

            //用HtmlAgilityPack分析
            HtmlDocument introductionHtml = GetHtmlDocument(introductionUrl);


            ////取作者跟書名
            //var htmlNode = htmlRoot.DocumentNode.SelectSingleNode("/html/body/div[3]/div[1]/div[1]/div[1]/div[2]");
            TaskInfo.Title =
               introductionHtml.DocumentNode.SelectSingleNode("/html/body/div[2]/h1").InnerText;
            TaskInfo.Author =
                introductionHtml.DocumentNode.SelectSingleNode(
                    "/html/body/div[2]/div[3]/ul[2]/li[2]/a[2]").InnerText;



            //HtmlDocument htmlRoot = GetHtmlDocument(TaskInfo.Url);

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
        private List<string> _sectionNames;

        private List<string> SectionNames
        {
            get
            {
                if (_sectionNames == null || _sectionNames.Count==0)
                {
                    _sectionNames = new List<string>();
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
            //Regex r = new Regex(@"<a href=\Shttp:\/\/\w*\.*lknovel\.lightnovel.cn\/main\/view\/(?<SectionName>\d+)");
            Regex r = new Regex(string.Format(@"<a href=\S\/Novel\/{0}(?<SectionName>\/\d+\/\d+)\S", TaskInfo.Tid));

            
            MatchCollection matchs = r.Matches(htmlRoot.DocumentNode.InnerHtml);
            foreach (Match m in matchs)
            {
                //int temp = CommonTools.TryParse(m.Groups["SectionName"].Value, 0);
                if (!_sectionNames.Contains(m.Groups["SectionName"].Value))
                {
                    _sectionNames.Add(m.Groups["SectionName"].Value);
                }
            }
            //_sectionNames.Sort();
        }


        public override bool Download()
        {
            CurrentParameter.IsStop = false;





            string urlHead = string.Format("http://book.sfacg.com/Novel/{0}", TaskInfo.Tid);
            //string urlTail = ".html?charset=big5";

            //排版插件
            var typeSetting = new Collection<ITypeSetting>
                { 
                    new SfacgToIndent(),
                    new HtmlDecode(),
                    new UniformFormat(),
                    new Traditional()
                };


            for (; TaskInfo.BeginSection <= TaskInfo.EndSection && !CurrentParameter.IsStop; TaskInfo.BeginSection++)
            {
                string url = urlHead + SectionNames[TaskInfo.CurrentSection].ToString(CultureInfo.InvariantCulture);//組合網址

                HtmlDocument htmlRoot = GetHtmlDocument(url);


                try
                {
                    var nodeHeaders =
                        htmlRoot.DocumentNode.SelectSingleNode(@"//*[@id=""ChapterBody""]");
                    Network.RemoveSubHtmlNode(nodeHeaders, "img");
                    string tempTextFile = nodeHeaders.InnerHtml;
                    
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
