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
    public class RanwenDownloader : AbstractDownloader
    {
        public RanwenDownloader()
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
            Regex r = new Regex(@"(^http:\/\/\w*\.*ranwen.net\/info_(?<TID>\d+))|(^http:\/\/\w*\.*ranwen.net(\/files)*(\/article)*\/\d+\/(?<TID>\d+))");
            Match m = r.Match(TaskInfo.Url);
            if (m.Success)
            {
                TaskInfo.Tid = m.Groups["TID"].Value;
            }

            TaskInfo.Url = string.Format("http://www.ranwen.net/files/article/{0}/{1}/index.html", (CommonTools.TryParse(TaskInfo.Tid, 0) / 1000).ToString(CultureInfo.InvariantCulture), TaskInfo.Tid);



            //用HtmlAgilityPack分析
            HtmlDocument htmlRoot = GetHtmlDocument(TaskInfo.Url);


            ////取作者跟書名
            TaskInfo.Title =
               htmlRoot.DocumentNode.SelectSingleNode("/html/html/body/h1")
                        .InnerText;
            TaskInfo.Author =
                htmlRoot.DocumentNode.SelectSingleNode("/html/html/body/div[3]").InnerText.Substring(3);

            
      
            
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
            Regex r = new Regex(@"<a href=\S(?<SectionName>\d+)\.html\S");
            MatchCollection matchs = r.Matches(htmlRoot.DocumentNode.SelectSingleNode("//*[@id=\"defaulthtml4\"]/table").InnerHtml);
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
                { new BrRegex(),
                    new RemoveSpecialCharacters(),
                    new UniformFormat(),
                    new Traditional()
                };


            for (; TaskInfo.BeginSection <= TaskInfo.EndSection && !CurrentParameter.IsStop; TaskInfo.BeginSection++)
            {
                string url = string.Format("http://www.ranwen.net/files/article/{0}/{1}/{2}.html", 
                    (CommonTools.TryParse(TaskInfo.Tid, 0) / 1000).ToString(CultureInfo.InvariantCulture), 
                    TaskInfo.Tid,
                    SectionNames[TaskInfo.CurrentSection].ToString(CultureInfo.InvariantCulture));//組合網址

                HtmlDocument htmlRoot = GetHtmlDocument(url);

                try
                {

                    string tempTextFile = htmlRoot.DocumentNode.SelectSingleNode("//*[@id=\"bgdiv\"]/table[2]/tbody/tr[1]/td/div[1]/h1").InnerText
                    +"\r\n";

                    var node=htmlRoot.DocumentNode.SelectSingleNode("//*[@id=\"content\"]");
                    Network.RemoveSubHtmlNode(node, "div");

                    tempTextFile += node.InnerHtml+ "\r\n";
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
