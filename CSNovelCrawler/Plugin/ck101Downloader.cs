using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace CSNovelCrawler.Plugin
{
  
    class ck101Downloader : abstractDownloader
    {
        public ck101Downloader()
        {
            
            
            currentParameter = new DownloadParameter()
            {
                UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)",
            };
        }
    


        public HtmlAgilityPack.HtmlDocument GetHtmlDocument(string Url)
        {
            this.currentParameter.Url = Url;
            return Network.GetHtmlDocument(Network.GetHtmlSource(this.currentParameter, Encoding.UTF8));
        }

        /// <summary>
        /// 取得網頁上的基本資料
        /// </summary>
        /// <param name="Url"></param>
        public override bool Analysis()
        {
            
            //用HtmlAgilityPack分析
            HtmlAgilityPack.HtmlDocument HtmlRoot = GetHtmlDocument(Info.Url);


            ////取作者跟書名
            string HtmlTitle = HtmlRoot.DocumentNode.SelectSingleNode("/html/head/title").InnerText;
            Regex r = new Regex(@"(?<Title>.+)\s*作者\W\s*(?<Author>\S+)");
            Match m = r.Match(HtmlTitle);
            if (m.Success)
            {
                Info.Author = m.Groups["Author"].Value.Trim();
                Info.Title = m.Groups["Title"].Value.Trim();
            }

            //取總頁數
            HtmlNodeCollection nodeHeaders2 = HtmlRoot.DocumentNode.SelectNodes("//*[@id=\"pgt\"]/table/tr/td[1]/div/div/a");
            string s = nodeHeaders2[nodeHeaders2.Count() - 2].InnerText;
            r = new Regex(@"(?<TotalPage>\d+)");
            m = r.Match(s);
            if (m.Success)
            {
                Info.TotalPage = CommonTools.TryParse(m.Groups["TotalPage"].Value, 0);
            }

            //取TID
            r = new Regex(@"^http:\/\/\w*\.*ck101.com\/thread-(?<TID>\d+)-(?<CurrentPage>\d+)-\w+\.html");
            m = r.Match(Info.Url);
            if (m.Success)
            {
                //Info.CurrentPage = CommonTools.TryParse(m.Groups["CurrentPage"].Value, 0);
                Info.TID = m.Groups["TID"].Value;
            }

            Info.PageSection = GetSection(
                    GetHtmlDocument(Regex.Replace(Info.Url, @"(?!^http:\/\/\w*\.*ck101.com\/thread-\d+-)(?<CurrentPage>\d+)(?=-\w+\.html)", (Info.TotalPage - 1).ToString()))
               );
            Info.TotalSection = Info.PageSection * (Info.TotalPage - 1) +
                GetSection(
                    GetHtmlDocument(Regex.Replace(Info.Url, @"(?!^http:\/\/\w*\.*ck101.com\/thread-\d+-)(?<CurrentPage>\d+)(?=-\w+\.html)", Info.TotalPage.ToString()))
               );
            Info.BeginSection = 1;
            Info.EndSection = Info.TotalSection;
            return true;

            
        }
        /// <summary>
        /// 取頁的樓層數
        /// </summary>
        /// <param name="HtmlRoot"></param>
        /// <returns></returns>
        public int GetSection(HtmlAgilityPack.HtmlDocument HtmlRoot)
        {
            return HtmlRoot.DocumentNode.SelectNodes("//*[@id=\"postlist\"]/div/table/tr[2]/td[1]/div[1]/div[1]/div[1]/table[1]/tr[1]/td[1]").Count();
        }
        
        public override bool Download()
        {
            currentParameter.IsStop = false;

            Regex r = new Regex(@"(?<Head>^http:\/\/\w*\.*ck101.com\/thread-\d+-)(?<CurrentPage>\d+)(?<Tail>-\w+\.html)");
            Match m = r.Match(Info.Url);
            string UrlHead = string.Empty, UrlTail = string.Empty;
            if (m.Success)
            {
                UrlHead = m.Groups["Head"].Value;
                UrlTail = m.Groups["Tail"].Value;
            }


            HtmlAgilityPack.HtmlDocument HtmlRoot = null;
            HtmlNodeCollection nodeHeaders = null;
            int NewCurrentPage = 0;
            int LastPage = 0;
            //排版插件
            var TypeSetting = new Collection<ITypeSetting>() 
                { 
                    new RemoveSpecialCharacters(), 
                    new UniformFormat()
                };
            int NumberErrors = 0;//記錄錯誤次數

            for (; Info.BeginSection <= Info.EndSection && !currentParameter.IsStop; Info.BeginSection++)
            {
                //要下載的頁數
                NewCurrentPage = (Info.BeginSection + Info.PageSection - 1) / Info.PageSection;

                if (LastPage != NewCurrentPage)//之前下載的頁數跟當前要下載的頁數
                {
                    LastPage = NewCurrentPage;//記錄下載頁數，下次如果一樣就不用重抓
                    string Url = UrlHead + LastPage.ToString() + UrlTail;//組合網址

                    if (LastPage == 1)//卡提諾第一頁的特別處理
                    {
                        switch (NumberErrors%2)//常常取不到完整資料，用兩個網址取
                        {
                            case 0:
                                Url = string.Format("http://ck101.com/forum.php?mod=viewthread&tid={0}&r=findpost&page=1", Info.TID);
                                break;

                            case 1:
                                Url = string.Format("http://m.ck101.com/forum.php?mod=redirect&ptid={0}&authorid=0&postno=1", Info.TID);
                                break;
                        }
                    }

                    HtmlRoot = GetHtmlDocument(Url);//取得Html文件

                    if (HtmlRoot != null)
                    {
                        if (LastPage != 1)
                        {
                            nodeHeaders = HtmlRoot.DocumentNode.SelectNodes("//*[@id=\"postlist\"]/div/table/tr[2]/td[1]/div[1]/div[1]/div[1]/table[1]/tr[1]/td[1]");
                        }
                        else
                        {
                            nodeHeaders = HtmlRoot.DocumentNode.SelectNodes("//*[@id=\"postlist\"]/div/table[1]/tr/td[1]/div[1]/div[1]/div[1]/table[1]/tr[1]/td[1]");
                        }
                    }
                }

                

                try
                {
                    //計算要取的區塊在第幾個
                    int PartSection = Info.BeginSection - ((LastPage - 1) * Info.PageSection) - 1;
                    if (nodeHeaders != null)
                    {
                        string TempTxt = string.Empty;


                        TempTxt = nodeHeaders[PartSection].InnerText;


                        foreach (var item in TypeSetting)
                        {

                            item.Set(ref TempTxt);

                        }
                        FileWrite.TxtWrire(TempTxt, Info.SaveFilePath);

                    }
                }
                catch (Exception ex)
                {
                    //發生錯誤，當前區塊重取
                    Info.BeginSection--;
                    NumberErrors++;
                    LastPage = 0;

                    continue;
                }

                Info.HasStopped = currentParameter.IsStop;
            }

            bool finish = false;
            if (Info.CurrentSection == Info.EndSection)
            {
                finish = true;
            }
            return finish;
        }



        

    }
}
