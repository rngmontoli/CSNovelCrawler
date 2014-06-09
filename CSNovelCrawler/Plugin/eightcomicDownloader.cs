using System;
using System.Collections.Generic;
using System.Text;
using CSNovelCrawler.Class;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace CSNovelCrawler.Plugin
{
    public class eightcomicDownloader : AbstractDownloader
    {
        public eightcomicDownloader()
        {
            CurrentParameter = new DownloadParameter
            {
                UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)",
                Timeout = 10000
            };
        }
        private List<string> ImgUrl;
        public override bool Analysis()
        {
            string ch = string.Empty;
            Regex r = new Regex("^http:\\/\\/new\\.comicvip\\.com\\/show\\/(?<TID>\\D.*)\\.\\S*\\?ch=(?<ch>\\d*)");
            Match m = r.Match(TaskInfo.Url);
            if (m.Success)
            {
                TaskInfo.Tid = m.Groups["TID"].Value;
                ch = m.Groups["ch"].Value;
            }
            try
            {
                //HttpWebRequest webres = (HttpWebRequest)WebRequest.Create(TaskInfo.Url);
                //webres.Timeout = 3000;               
                //webres.UserAgent = this.CurrentParameter.UserAgent;
                //HttpWebResponse response = (HttpWebResponse)webres.GetResponse();                
                //System.IO.Stream stream = response.GetResponseStream();
                //System.IO.StreamReader streamreader = new System.IO.StreamReader(stream, Encoding.GetEncoding("BIG5"));

                CurrentParameter.Url = TaskInfo.Url;


                string sHTML_CODE = Network.GetHtmlSource(CurrentParameter, Encoding.GetEncoding("BIG5"));
                r = new Regex("<title>(?<title>\\S*).*<\\/title>");
                m = r.Match(sHTML_CODE);
                if (m.Success)
                {
                    TaskInfo.Title = m.Groups["title"].Value;
                }
                
                string itemid = string.Empty;
                string chs = string.Empty;
                string allcodes = string.Empty;
                r = new Regex("var\\schs=(?<chs>\\d*);var\\sitemid=(?<itemid>\\d*);var\\sallcodes=\"(?<allcodes>.*)\";");
                m = r.Match(sHTML_CODE);
                if (m.Success)
                {
                    itemid = m.Groups["itemid"].Value;
                    chs = m.Groups["chs"].Value;
                    allcodes = m.Groups["allcodes"].Value;
                    ImgUrl = getImgUrl(itemid, chs, allcodes, ch);
                    if (ImgUrl.Count != 0)
                    {                        
                        TaskInfo.TotalSection = ImgUrl.Count;
                        TaskInfo.BeginSection = 1;
                        TaskInfo.CurrentSection = 1;
                        TaskInfo.EndSection = TaskInfo.TotalSection;                        
                        //TaskInfo.TaskType = TaskInfo.DownType.Cartoon;
                        return true;
                    }
                }                
            }
            catch(Exception ex)
            {
                
            }            
            
            return false;
        }

        private List<string> getImgUrl(string itemid, string chs, string allcodes,string ch)
        {
            List<string> ImgUrl = new List<string>();
            string[] Codes = allcodes.Split('|');
            string[] Code = new string[5];
            foreach (string i in Codes)
            {
                if (i.IndexOf(ch + " ") == 0)
                {
                    Code = i.Split(' ');
                    break;
                }
            }
            for (int i = 1; i <= int.Parse(Code[3]); i++)
            {
                int idx = (((i - 1) / 10) % 10) + (((i - 1) % 10) * 3);
                ImgUrl.Add("http://img" + Code[1] + ".8comic.com/" + Code[2] + "/" + itemid + "/" + Code[0] + "/" + i.ToString("000") + "_" + Code[4].Substring(idx, 3) + ".jpg");
            }
            return ImgUrl;
        }

       public override bool Download()
       {
           try
           {
               for (; TaskInfo.CurrentSection <= TaskInfo.TotalSection; TaskInfo.CurrentSection++)
               {
                   string Url = ImgUrl[TaskInfo.CurrentSection - 1];
                   CurrentParameter.Url = Url;


                   string sHTML_CODE = Network.GetHtmlSource(CurrentParameter, Encoding.Default);
                   //FileWrite.TxtWrire(sHTML_CODE, TaskInfo.SaveFullPath, Encoding.Default);
                   //byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(sHTML_CODE);
                   //using (FileStream lxFS = new FileStream(TaskInfo.SaveFullPath, FileMode.Create))
                   //{
                   //    lxFS.Write(byteArray, 0, byteArray.Length);
                   //}

                   //WebClient GetImg = new WebClient();
                   //GetImg.DownloadFile(Url, TaskInfo.SaveDirectoryName + "\\" + TaskInfo.Title + (TaskInfo.CurrentSection).ToString("000") + ".jpg");
               }
           }
           catch
           {
               return false;
           }
           return true;
       }
    }
}
