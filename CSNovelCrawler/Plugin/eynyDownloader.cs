using HtmlAgilityPack;
using System;
using System.Collections.Generic;
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

        public eynyDownloader()
        {
            
            string Url = string.Format("http://www02.eyny.com/member.php?mod=logging&action=login&loginsubmit=yes&handlekey=login&loginhash=LiKaw&inajax=1");

            ServicePointManager.Expect100Continue = false;
            string postdata = "formhash=3b765c67&referer=http%3A%2F%2Fwww02.eyny.com%2F&loginfield=username&username=CSNovelDown&password=V5Bx7ucG+KHP&questionid=0&answer=&cookietime=2592000";
            byte[] data = Encoding.UTF8.GetBytes(postdata);
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
            
            
            //eynyDownloader eynyDown = new eynyDownloader(Info);

            //用HtmlAgilityPack分析
            HtmlAgilityPack.HtmlDocument HtmlRoot = GetHtmlDocument(Info.Url);


            ////取作者跟書名
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
            return true;
        }

        public int GetSection(HtmlAgilityPack.HtmlDocument HtmlRoot)
        {
            return HtmlRoot.DocumentNode.SelectNodes("//*[@id=\"postlist\"]/div/table/tr[1]/td[2]/div[2]/div[2]/div[1]/table[1]/tr[1]/td[1]").Count();
        }


        public override bool Download()
        {
            //Regex r = new Regex(@"(?<Head>^http:\/\/\w*\.eyny.com\/thread-\d+-)(?<CurrentPage>\d+)(?<Tail>-\w+\.html)");
            //Match m = r.Match(Info.Url);
            //string UrlHead = string.Empty, UrlTail = string.Empty;
            //if (m.Success)
            //{
            //    UrlHead = m.Groups["Head"].Value;
            //    UrlTail = m.Groups["Tail"].Value;
            //}
            //StringBuilder SB = new StringBuilder();
            //for (; Info.CurrentPage <= Info.TotalPage; Info.CurrentPage++)
            //{
                
            //    HtmlAgilityPack.HtmlDocument HtmlRoot = GetHtmlDocument(UrlHead + Info.CurrentPage.ToString() + UrlTail);
            //    HtmlNodeCollection nodeHeaders = HtmlRoot.DocumentNode.SelectNodes("//*[@id=\"postlist\"]/div/table/tr[1]/td[2]/div[2]/div[2]/div[1]/table[1]/tr[1]/td[1]");
                
            //    foreach (HtmlNode nodeHeader in nodeHeaders)
            //    {

            //        //SB.Append(nodeHeader.InnerText);
            //        string fileName = @"C:\Users\Montoli\Desktop\測試檔案.txt";
            //        FileWrite.TxtWrire(nodeHeader.InnerText, fileName);


            //    }

            //}

            //using (StreamWriter sw = new StreamWriter(@"C:\Users\Montoli\Desktop\測試檔案2.txt"))
            //{

            //    sw.Write(SB.ToString());
            //}
            return true;
        }



        
    }
}
