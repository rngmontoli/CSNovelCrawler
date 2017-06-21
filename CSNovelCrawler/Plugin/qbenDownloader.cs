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
    public class qbenDownloader : AbstractDownloader
    {
        public qbenDownloader()
        {
            CurrentParameter = new DownloadParameter
                {
                    UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)",
                };
        }



        public HtmlDocument GetHtmlDocument(string url)
        {
            CurrentParameter.Url = url;
            return Network.GetHtmlDocument(Network.GetHtmlSource(CurrentParameter, Encoding.GetEncoding("UTF-8")));
        }


        public HtmlDocument PostHtmlDocument(string formData)
        {
            CurrentParameter.Url = "http://big5.quanben5.com/index.php?c=book&a=ajax_content";
            return Network.GetHtmlDocument(Network.PostHtmlSource(CurrentParameter, Encoding.GetEncoding("UTF-8"), formData));
        }


        /// <summary>
        /// 取得網頁上的基本資料
        /// </summary>
        public override bool Analysis()
        {

            
            //取TID
            Regex r = new Regex(@"(^http:\/\/\w*\.*quanben\d?.com\/n\/(?<TID>\S+)\/)");
            Match m = r.Match(TaskInfo.Url);
            if (m.Success)
            {
                TaskInfo.Tid = m.Groups["TID"].Value;
            }

            TaskInfo.Url = string.Format("http://big5.quanben5.com/n/{0}/", TaskInfo.Tid);



            //用HtmlAgilityPack分析
            HtmlDocument htmlRoot = GetHtmlDocument(TaskInfo.Url);


            ////取作者跟書名
            TaskInfo.Title =
               htmlRoot.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[2]/div[1]/h3/span")
                        .InnerText;
            TaskInfo.Author =
                htmlRoot.DocumentNode.SelectSingleNode("/html/body/div[3]/div/div[2]/div[1]/p[1]/span").InnerText;



            TaskInfo.Url = string.Format("http://big5.quanben5.com/n/{0}/xiaoshuo.html", TaskInfo.Tid);
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
            Regex r = new Regex(string.Format(@"<a href=\S\/n\/{0}\/(?<SectionName>\d+)\.html\S", TaskInfo.Tid));
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
                    new AnnotationRegex(),
                    new BrRegex(),
                    new PRegex(),
                    new HtmlDecode(),
                    new UniformFormat(),
                };


            for (; TaskInfo.BeginSection <= TaskInfo.EndSection && !CurrentParameter.IsStop; TaskInfo.BeginSection++)
            {
                string url = string.Format("http://big5.quanben5.com/n/{0}/{1}.html",
                    TaskInfo.Tid, 
                    SectionNames[TaskInfo.CurrentSection].ToString(CultureInfo.InvariantCulture));//組合網址

                HtmlDocument htmlRoot = GetHtmlDocument(url);

                try
                {
                    Regex r = new Regex(@"ajax_post\('book','ajax_content','pinyin','(?<pinyin>\S+)','content_id','(?<content_id>\d+)','sky','(?<sky>\S+)','t','(?<t>\d+)'\)");
                    Match m = r.Match(htmlRoot.DocumentNode.InnerHtml);
                    string formData = "";
                    if (m.Success)
                    {
                        string timestamp = Convert.ToInt32(DateTime.UtcNow.AddHours(8).Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString() + "000";
                        formData = string.Format("pinyin={0}&content_id={1}&sky={2}&t={3}&_type=ajax&rndval={4}",
                              m.Groups["pinyin"].Value,
                              m.Groups["content_id"].Value,
                              m.Groups["sky"].Value,
                              m.Groups["t"].Value,
                              timestamp
                              );
                    }

                    htmlRoot = PostHtmlDocument(formData);

                    //var node = htmlRoot.DocumentNode.SelectSingleNode("//*[@id=\"content\"]");
                    //Network.RemoveSubHtmlNode(node, "div");

                    string tempTextFile = htmlRoot.DocumentNode.InnerHtml + "\r\n";
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
