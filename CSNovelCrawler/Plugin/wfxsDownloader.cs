using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using CSNovelCrawler.Class;
using HtmlAgilityPack;
using System.Globalization;
using System.Net;

namespace CSNovelCrawler.Plugin
{
    public class wfxsDownloader : AbstractDownloader
    {
        public wfxsDownloader()
        {
            CurrentParameter = new DownloadParameter
                {
                    UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36",
                };
        }



        public HtmlDocument GetHtmlDocument(string url)
        {
            CurrentParameter.Url = url;
            return Network.GetHtmlDocument(Network.GetHtmlSource(CurrentParameter, Encoding.GetEncoding("big5")));
        }


        public string GetHtmlString(string url)
        {
            CurrentParameter.Url = url;
            return Network.GetHtmlSource(CurrentParameter, Encoding.GetEncoding("big5"));
        }


        /// <summary>
        /// 取得網頁上的基本資料
        /// </summary>
        public override bool Analysis()
        {

            
            //取TID
            Regex r = new Regex(@"(^https?:\/\/\w*\.*wfxs\d?.org\/(html|book)\/(?<TID>\d+))");
            Match m = r.Match(TaskInfo.Url);
            if (m.Success)
            {
                TaskInfo.Tid = m.Groups["TID"].Value;
            }

            TaskInfo.Url = string.Format("https://www.wfxs.org/html/{0}/", TaskInfo.Tid);



            //用HtmlAgilityPack分析
            HtmlDocument htmlRoot = GetHtmlDocument(string.Format("https://www.wfxs.org/book/{0}.html", TaskInfo.Tid));


            ////取作者跟書名
            TaskInfo.Title =
               htmlRoot.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div[1]/h1/a[2]")
                        .InnerText.Trim();
            TaskInfo.Author =
                htmlRoot.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[2]/div[2]/table/tr/td/table/tbody/tr[1]/td/table/tr[2]/td[2]/table/tr[2]/td[6]")
                .InnerText.Trim();

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
           
            HtmlDocument htmlRoot = GetHtmlDocument(string.Format("https://www.wfxs.org/html/{0}/", TaskInfo.Tid));
            Regex r = new Regex(string.Format(@"<dd><a href=""\/html\/{0}\/(?<SectionName>\d+)\.html\"">.+?<\/a><\/dd>", TaskInfo.Tid));
            MatchCollection matchs = r.Matches(htmlRoot.DocumentNode.InnerHtml);
            foreach (Match m in matchs)
            {
                int temp = CommonTools.TryParse(m.Groups["SectionName"].Value, 0);
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
                    //new AnnotationRegex(),
                    new Remove0007(),
                    new BrRegex(),
                    //new PRegex(),
                    new HtmlDecode(),
                    new UniformFormat(),
                };

            for (; TaskInfo.BeginSection <= TaskInfo.EndSection && !CurrentParameter.IsStop; TaskInfo.BeginSection++)
            {
                string url = string.Format("https://www.wfxs.org/html/{0}/{1}.html",
                    TaskInfo.Tid, 
                    SectionNames[TaskInfo.CurrentSection].ToString(CultureInfo.InvariantCulture));//組合網址


                try
                {
                    string htmlstring = GetHtmlString(url);
                    string content = "";
                    Regex r = new Regex(@"<a href=""\/html\/\d+\/"">.+?<\/a>(?<content>.+?)<script>style_4\(\);<\/script>", RegexOptions.Singleline);
                    Match m = r.Match(htmlstring);
                    if (m.Success)
                    {
                        content = m.Groups["content"].Value;
                    }
                    HtmlDocument htmlRoot = Network.GetHtmlDocument(content);

                    var node = htmlRoot.DocumentNode;
                    Network.RemoveSubHtmlNode(node, "div");

                    string tempTextFile = node.InnerHtml + "\r\n";
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
