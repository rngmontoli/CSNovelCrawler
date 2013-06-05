using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSNovelCrawler.Plugin
{
   
    public class eynyDownloader : abstractDownloader
    {

        public eynyDownloader(IPlugin Plugin)
        {
            EncryptAES.EncryptAES AES = new EncryptAES.EncryptAES();
            string Url = string.Format("http://www02.eyny.com/member.php?mod=logging&action=login&loginsubmit=yes&handlekey=login&loginhash=LiKaw&inajax=1");

            ServicePointManager.Expect100Continue = false;

            string postdata = "jNLWAPIFsJ0iWz7D00C09Fy1nAmQepY1y5cHlwqy0+75fQ1bfPELaZdYi/OKhAghQA0TiEVPd0wsFNCzNcVQNpqObZuZyl3DE18XX+Gwn0WBD7ARSRyDoyl8n0HpXAPIEuJgubT+X9mDY0ncZ5Tl7IjD7xFtsoIPo69qjcdqRQlqzRZDscqED++/VRu1n6EbKqcyOisxN23RpROXOhKPGVs13Drn2bZBC0gh31EHXI0=";
            if (Plugin.Configuration.ContainsKey("PostData"))
            {
                postdata = Plugin.Configuration["PostData"];
            }
            //postdata = "formhash=3b765c67&referer=http%3A%2F%2Fwww02.eyny.com%2F&loginfield=username&username=CSNovelDown&password=V5Bx7ucG+KHP&questionid=0&answer=&cookietime=2592000";
            byte[] data = Encoding.UTF8.GetBytes(AES.DecryptAES256(postdata));
            //生成请求
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Url);
            req.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
            req.Method = "POST";
            req.CookieContainer = new CookieContainer();

            using (var outstream = req.GetRequestStream())
            {
                outstream.Write(data, 0, data.Length);
                outstream.Flush();
            }
            //关闭请求
            req.GetResponse().Close();
            currentParameter = new DownloadParameter()
            {
                UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)",
                Cookies = req.CookieContainer
            };
        }
       
        public HtmlAgilityPack.HtmlDocument GetHtmlDocument(string Url)
        {
            this.currentParameter.Url = Url;
            return Network.GetHtmlDocument(Network.GetHtmlSource(this.currentParameter, Encoding.UTF8));
        }


        public override bool Analysis()
        {
            //用HtmlAgilityPack分析
            HtmlAgilityPack.HtmlDocument HtmlRoot = GetHtmlDocument(Info.Url);

            //取作者跟書名
            string HtmlTitle = HtmlRoot.DocumentNode.SelectSingleNode("/html/head/title").InnerText;
            Regex r = new Regex(@"(?<Author>\S+)\s*-\s*【(?<Title>\S+)】");
            Match m = r.Match(HtmlTitle);
            if (m.Success)
            {
                Info.Author = m.Groups["Author"].Value;
                Info.Title = m.Groups["Title"].Value;
            }

            //取總頁數
            HtmlNodeCollection nodeHeaders2 = HtmlRoot.DocumentNode.SelectNodes("//*[@id=\"pgt\"]/div[1]/div/a");
            string s = nodeHeaders2[nodeHeaders2.Count() - 3].InnerText;
            r = new Regex(@"(?<TotalPage>\d+)");
            m = r.Match(s);
            if (m.Success)
            {
                Info.TotalPage = CommonTools.TryParse(m.Groups["TotalPage"].Value, 0);
            }

            //取TID
            r = new Regex(@"^http:\/\/\w*\.eyny.com\/thread-(?<TID>\d+)-(?<CurrentPage>\d+)-\w+\.html");
            m = r.Match(Info.Url);
            if (m.Success)
            {
                //Info.CurrentPage = CommonTools.TryParse(m.Groups["CurrentPage"].Value, 0);
                Info.TID = m.Groups["TID"].Value;
            }

            Info.PageSection = GetSection(HtmlRoot);
            Info.TotalSection = Info.PageSection * (Info.TotalPage - 1) +
                GetSection(
                    GetHtmlDocument(Regex.Replace(Info.Url, @"(?!^http:\/\/\w*\.eyny.com\/thread-\d+-)(?<CurrentPage>\d+)(?=-\w+\.html)", Info.TotalPage.ToString()))
               );
            Info.BeginSection = 1;
            Info.EndSection = Info.TotalSection;
            return true;
        }

        public int GetSection(HtmlAgilityPack.HtmlDocument HtmlRoot)
        {
            return HtmlRoot.DocumentNode.SelectNodes("//*[@id=\"postlist\"]/div/table/tr[1]/td[2]/div[2]/div[2]/div[1]/table[1]/tr[1]/td[1]").Count();
        }


        public override bool Download()
        {
            Regex r = new Regex(@"(?<Head>^http:\/\/\w*\.eyny.com\/thread-\d+-)(?<CurrentPage>\d+)(?<Tail>-\w+\.html)");
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

                    HtmlRoot = GetHtmlDocument(Url);//取得Html文件

                    if (HtmlRoot != null)
                    {
                        nodeHeaders = HtmlRoot.DocumentNode.SelectNodes("//*[@id=\"postlist\"]/div/table/tr[1]/td[2]/div[2]/div[2]/div[1]/table[1]/tr[1]/td[1]");
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

                //HtmlAgilityPack.HtmlDocument HtmlRoot = GetHtmlDocument(UrlHead + Info.CurrentPage.ToString() + UrlTail);
                //HtmlNodeCollection nodeHeaders = HtmlRoot.DocumentNode.SelectNodes("//*[@id=\"postlist\"]/div/table/tr[1]/td[2]/div[2]/div[2]/div[1]/table[1]/tr[1]/td[1]");

                //foreach (HtmlNode nodeHeader in nodeHeaders)
                //{

                //    //SB.Append(nodeHeader.InnerText);
                //    string fileName = @"C:\Users\Montoli\Desktop\測試檔案.txt";
                //    FileWrite.TxtWrire(nodeHeader.InnerText, fileName);


                //}
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
