using System;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;

namespace CSNovelCrawler.Plugin
{
    [Serializable]
    public abstract class AbstractDownloader : IDownloader
    {
        public TaskInfo TaskInfo { get; set; }

      
        public DownloadParameter CurrentParameter { get; set; }

        public abstract bool Analysis();


        public abstract bool Download();


        public void StopDownload()
        {
            if (CurrentParameter != null)
            {
                //將停止旗標設為true
                CurrentParameter.IsStop = true;
            }
        }
       
    }
}
