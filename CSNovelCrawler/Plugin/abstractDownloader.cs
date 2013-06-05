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
                //将停止flag设置为true
                CurrentParameter.IsStop = true;
            }
        }

        //public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        //{
        //    info.AddValue("Total", Total);
        //    info.AddValue("Total", Done);
        //    info.AddValue("Total", taskInfo);
        //    info.AddValue("Total", currentParameter);
        //}
    }
}
