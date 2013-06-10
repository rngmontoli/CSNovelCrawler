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
    public class EightnovelDownloader : AbstractDownloader
    {
        public EightnovelDownloader()
        {
            CurrentParameter = new DownloadParameter
                {
                    UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)",
                };
        }

        public HtmlDocument GetHtmlDocument(string url)
        {
            CurrentParameter.Url = url;
            return Network.GetHtmlDocument(Network.GetHtmlSource(CurrentParameter, Encoding.GetEncoding("BIG5")));
        }

        /// <summary>
        /// 取得網頁上的基本資料
        /// </summary>
        public override bool Analysis()
        {

            //取TID
            Regex r = new Regex(@"^http:\/\/\w*\.*8novel.com(\/books)*(\/novelbook_)(?<TID>\d+)");
            Match m = r.Match(TaskInfo.Url);
            if (m.Success)
            {
                TaskInfo.Tid = m.Groups["TID"].Value;
            }

            TaskInfo.Url = string.Format("http://8novel.com/books/novelbook_{0}.html", TaskInfo.Tid);

            

            //用HtmlAgilityPack分析
            HtmlDocument htmlRoot = GetHtmlDocument(TaskInfo.Url);
            //*[@id="form1"]/div[3]/table[7]/tbody/tr[1]/td/h1

            ////取作者跟書名
            TaskInfo.Title = htmlRoot.DocumentNode.SelectSingleNode("/body/table[2]/tr[2]/td[3]/table[2]/tr/td[2]/table/tr[1]/td[2]/font[1]").InnerText;
            TaskInfo.Author = htmlRoot.DocumentNode.SelectSingleNode("/body/table[2]/tr[2]/td[3]/table[2]/tr/td[2]/table/tr[2]/td/font[1]").InnerText;


            
            //var htmlTitle = htmlRoot.DocumentNode.SelectSingleNode("/html/head/title").InnerText;
            //r = new Regex(@"(?<Title>\S+)?\/(?<Author>\S+)\/");
            //m = r.Match(htmlTitle);
            //if (m.Success)
            //{
            //    TaskInfo.Author = m.Groups["Author"].Value.Trim();
                
            //}

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
                if (_sectionNames == null || _sectionNames.Count == 0)
                {
                    _sectionNames=new List<string>();
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
            Regex r = new Regex(@"<a href=\S(\/readbook)*\/(?<SectionName>\d+\/\d+\/\d+)");
            MatchCollection matchs = r.Matches(htmlRoot.DocumentNode.SelectSingleNode("/body/table[2]/tr[2]/td[3]/table[3]").InnerHtml);
            foreach (Match m in matchs)
            {
                string temp = m.Groups["SectionName"].Value.Trim();
                if (!_sectionNames.Contains(temp))
                {
                    _sectionNames.Add(temp);
                }
            }
            //_sectionNames.Sort();
        }

        public override bool Download()
        {
            CurrentParameter.IsStop = false;

           



          

            //排版插件
            var typeSetting = new Collection<ITypeSetting>
                { 
                    new BrRegex(),
                    new HtmlDecode(),
                    new UniformFormat(),
                    
                    
                };


            for (; TaskInfo.BeginSection <= TaskInfo.EndSection && !CurrentParameter.IsStop; TaskInfo.BeginSection++)
            {
                


    
                
                try
                {
                    string url = string.Format("http://8novel.com/readbook/{0}.html",
                    SectionNames[TaskInfo.CurrentSection].ToString(CultureInfo.InvariantCulture));//組合網址

                    HtmlDocument htmlRoot = GetHtmlDocument(url);
                    var htmlNode = htmlRoot.DocumentNode.SelectSingleNode("//*[@id=\"contentview\"]/tr[2]/td[2]/table[4]");
     
                    string tempTextFile = htmlNode.SelectSingleNode("tr[1]/td[1]").InnerText + "\r\n" + htmlNode.SelectSingleNode("tr[2]/td[1]/div/p").InnerHtml;
                       
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

