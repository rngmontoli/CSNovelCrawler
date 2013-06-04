using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSNovelCrawler
{
    public class eynyParser
    {

        public void Parser(string Url)
        {

            
            
            //TaskInfo Info = new TaskInfo();
            //Info.Url = Url;
            //eynyDownloader eynyDown = new eynyDownloader();

            ////用HtmlAgilityPack分析
            //HtmlAgilityPack.HtmlDocument HtmlRoot = eynyDown.GetHtmlDocument(Info.Url);


            //////取作者跟書名
            //string HtmlTitle = HtmlRoot.DocumentNode.SelectSingleNode("/html/head/title").InnerText;
            //Regex r = new Regex(@"(?<Author>\S+)\s*-\s*【(?<Title>\S+)】");
            //Match m = r.Match(HtmlTitle);
            //if (m.Success)
            //{
            //    Info.Author = m.Groups["Author"].Value;
            //    Info.Title = m.Groups["Title"].Value;
            //}
           
            ////取總頁數
            //HtmlNodeCollection nodeHeaders2 = HtmlRoot.DocumentNode.SelectNodes("//*[@id=\"pgt\"]/div[1]/div/a");
            //string s = nodeHeaders2[nodeHeaders2.Count()-3].InnerText;
            //r = new Regex(@"(?<TotalPage>\d+)");
            //m = r.Match(s);
            //if (m.Success)
            //{
            //    Info.TotalPage = CommonTools.TryParse(m.Groups["TotalPage"].Value, 0);
            //}

            ////取目前頁數
            //r = new Regex(@"^(http:\/\/)(www\w*\.)eyny.com\/thread-\d+-(?<CurrentPage>\d+)-\w+\.html");
            //m = r.Match(Url);
            //if (m.Success)
            //{
            //    Info.CurrentPage = CommonTools.TryParse(m.Groups["CurrentPage"].Value, 0);
            //}
            
            //Info.PageSection = GetSection(HtmlRoot);
            //Info.TotalSection = Info.PageSection*(Info.TotalPage-1)+
            //    GetSection(
            //        eynyDown.GetHtmlDocument(Regex.Replace(Url, @"(?!^(http:\/\/)(www\w*\.)eyny.com\/thread-\d+-)(?<CurrentPage>\d+)(?=-\w+\.html)", Info.TotalPage.ToString()))
            //   );



            //return Info;
        }


        public int GetSection(HtmlAgilityPack.HtmlDocument HtmlRoot)
        {
            return HtmlRoot.DocumentNode.SelectNodes("//*[@id=\"postlist\"]/div/table/tr[1]/td[2]/div[2]/div[2]/div[1]/table[1]/tr[1]/td[1]").Count();
        }
    }
}
