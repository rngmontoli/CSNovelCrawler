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

            
            
            //TaskInfo taskInfo = new TaskInfo();
            //taskInfo.Url = Url;
            //eynyDownloader eynyDown = new eynyDownloader();

            ////用HtmlAgilityPack分析
            //HtmlAgilityPack.HtmlDocument HtmlRoot = eynyDown.GetHtmlDocument(taskInfo.Url);


            //////取作者跟書名
            //string HtmlTitle = HtmlRoot.DocumentNode.SelectSingleNode("/html/head/title").InnerText;
            //Regex r = new Regex(@"(?<Author>\S+)\s*-\s*【(?<Title>\S+)】");
            //Match m = r.Match(HtmlTitle);
            //if (m.Success)
            //{
            //    taskInfo.Author = m.Groups["Author"].Value;
            //    taskInfo.Title = m.Groups["Title"].Value;
            //}
           
            ////取總頁數
            //HtmlNodeCollection nodeHeaders2 = HtmlRoot.DocumentNode.SelectNodes("//*[@id=\"pgt\"]/div[1]/div/a");
            //string s = nodeHeaders2[nodeHeaders2.Count()-3].InnerText;
            //r = new Regex(@"(?<TotalPage>\d+)");
            //m = r.Match(s);
            //if (m.Success)
            //{
            //    taskInfo.TotalPage = CommonTools.TryParse(m.Groups["TotalPage"].Value, 0);
            //}

            ////取目前頁數
            //r = new Regex(@"^(http:\/\/)(www\w*\.)eyny.com\/thread-\d+-(?<CurrentPage>\d+)-\w+\.html");
            //m = r.Match(Url);
            //if (m.Success)
            //{
            //    taskInfo.CurrentPage = CommonTools.TryParse(m.Groups["CurrentPage"].Value, 0);
            //}
            
            //taskInfo.PageSection = GetSection(HtmlRoot);
            //taskInfo.TotalSection = taskInfo.PageSection*(taskInfo.TotalPage-1)+
            //    GetSection(
            //        eynyDown.GetHtmlDocument(Regex.Replace(Url, @"(?!^(http:\/\/)(www\w*\.)eyny.com\/thread-\d+-)(?<CurrentPage>\d+)(?=-\w+\.html)", taskInfo.TotalPage.ToString()))
            //   );



            //return taskInfo;
        }


        public int GetSection(HtmlAgilityPack.HtmlDocument HtmlRoot)
        {
            return HtmlRoot.DocumentNode.SelectNodes("//*[@id=\"postlist\"]/div/table/tr[1]/td[2]/div[2]/div[2]/div[1]/table[1]/tr[1]/td[1]").Count();
        }
    }
}
