using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using CSNovelCrawler.Class;
using HtmlAgilityPack;
using System.Globalization;

namespace CSNovelCrawler.Plugin
{
    public class BiqugeDownloader : AbstractDownloader
    {
        public BiqugeDownloader()
        {
            CurrentParameter = new DownloadParameter
                {
                    UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)",
                };
        }



        public HtmlDocument GetHtmlDocument(string url)
        {
            CurrentParameter.Url = url;
            return Network.GetHtmlDocument(Network.GetHtmlSource(CurrentParameter, Encoding.GetEncoding("GBK")));
        }

        /// <summary>
        /// 取得網頁上的基本資料
        /// </summary>
        public override bool Analysis()
        {

            //取TID
            Regex r = new Regex(@"^http:\/\/\w*\.*biquge.com\/(?<TID>\d+_\d+)");
            Match m = r.Match(TaskInfo.Url);
            if (m.Success)
            {
                TaskInfo.Tid = m.Groups["TID"].Value;
            }

            TaskInfo.Url = string.Format("http://www.biquge.com/{0}/", TaskInfo.Tid);



            //用HtmlAgilityPack分析
            HtmlDocument htmlRoot = GetHtmlDocument(TaskInfo.Url);


            ////取作者跟書名
            var htmlNode = htmlRoot.DocumentNode.SelectNodes("//*[@id=\"info\"]");
            if (htmlNode.Count>0)
            {
                TaskInfo.Title = htmlNode[0].SelectSingleNode("h1").InnerText;
                r = new Regex(@"者：(?<Author>\S+)");
                m = r.Match(htmlNode[0].SelectSingleNode("p").InnerText);
                if (m.Success)
                {
                    TaskInfo.Author = m.Groups["Author"].Value.Trim();
                }
                
            }

            
      
            
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
                if (_sectionNames == null || _sectionNames.Count == 0)
                {
                    _sectionNames=new List<int>();
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
            Regex r = new Regex(@"<a href=\S\/\d+_\d+\/(?<SectionName>\d+)\.html\S>");
            MatchCollection matchs = r.Matches(htmlRoot.DocumentNode.SelectSingleNode("//*[@id=\"list\"]/dl").InnerHtml);
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

           



            //排版插件
            var typeSetting = new Collection<ITypeSetting>
                {  new BrRegex(),
                    new RemoveSpecialCharacters(),
                    new UniformFormat(),
                    new Traditional()
                };


            for (; TaskInfo.BeginSection <= TaskInfo.EndSection && !CurrentParameter.IsStop; TaskInfo.BeginSection++)
            {
                

    
                
                try
                {
                    string url = string.Format("http://www.biquge.com/{0}/{1}.html", TaskInfo.Tid, SectionNames[TaskInfo.CurrentSection].ToString(CultureInfo.InvariantCulture));//組合網址

                    HtmlDocument htmlRoot = GetHtmlDocument(url);


                    string tempTextFile = htmlRoot.DocumentNode.SelectSingleNode("//*[@id=\"wrapper\"]/div[3]/div[1]/div[2]/h1").InnerText 
                        + "\r\n" +htmlRoot.DocumentNode.SelectSingleNode("//*[@id=\"content\"]").InnerHtml + "\r\n";
                    

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
