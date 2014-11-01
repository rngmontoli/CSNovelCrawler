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
    public class HjwzwDownloader : AbstractDownloader
    {
        public HjwzwDownloader()
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
            Regex r = new Regex(@"^http:\/\/\w*\.*hjwzw.com(\/Book)*(\/Chapter)*\/(?<TID>\d+)");
            Match m = r.Match(TaskInfo.Url);
            if (m.Success)
            {
                TaskInfo.Tid = m.Groups["TID"].Value;
            }

            TaskInfo.Url = string.Format("http://tw.hjwzw.com/Book/Chapter/{0}", TaskInfo.Tid);



            //用HtmlAgilityPack分析
            HtmlDocument htmlRoot = GetHtmlDocument(TaskInfo.Url);
            //*[@id="form1"]/div[3]/table[7]/tbody/tr[1]/td/h1

            ////取作者跟書名
            var htmlTitle = htmlRoot.DocumentNode.SelectSingleNode("/html/head/title").InnerText;
            r = new Regex(@"(?<Title>\S+)?\/(?<Author>\S+)\/");
            m = r.Match(htmlTitle);
            if (m.Success)
            {
                TaskInfo.Author = m.Groups["Author"].Value.Trim();
                TaskInfo.Title = m.Groups["Title"].Value.Trim();
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
            Regex r = new Regex(@"<a href=\S(\/Book)*(\/Read)*\/\d+,(?<SectionName>\d+)");
            MatchCollection matchs = r.Matches(htmlRoot.DocumentNode.SelectSingleNode("//*[@id=\"tbchapterlist\"]").InnerHtml);
            foreach (Match m in matchs)
            {
                int temp = CommonTools.TryParse(m.Groups["SectionName"].Value, 0);
                if (!_sectionNames.Contains(temp))
                {
                    _sectionNames.Add(temp);
                }
            }

            //33244會有問題
            //_sectionNames.Sort();
        }

        public override bool Download()
        {
            CurrentParameter.IsStop = false;

           



          

            //排版插件
            var typeSetting = new Collection<ITypeSetting>
                { 
                    
                    new HtmlDecode(),
                    new UniformFormat(),
                    new HjwzwRegex()
                };


            for (; TaskInfo.BeginSection <= TaskInfo.EndSection && !CurrentParameter.IsStop; TaskInfo.BeginSection++)
            {
                


    
                
                try
                {
                    string url = string.Format("http://tw.hjwzw.com/Book/Read/{0},{1}",
                    TaskInfo.Tid,
                    SectionNames[TaskInfo.CurrentSection].ToString(CultureInfo.InvariantCulture));//組合網址

                    HtmlDocument htmlRoot = GetHtmlDocument(url);

                    string tempTextFile = htmlRoot.DocumentNode.SelectSingleNode("/html/body/table[7]/tr/td/div[5]").InnerText;
                       
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
