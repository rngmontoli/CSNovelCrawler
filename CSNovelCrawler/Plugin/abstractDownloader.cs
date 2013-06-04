using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSNovelCrawler.Plugin
{
    [Serializable]
    public abstract class abstractDownloader : IDownloader
    {
        public TaskInfo Info { get; set; }

        /// <summary>
        /// 文件总长度
        /// </summary>
        public int Total
        {
            get
            {
                if (Info != null)
                {
                    return Info.EndSection - Info.BeginSection;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 文件总长度
        /// </summary>
        public int Done
        {
            get
            {
                if (Info != null)
                {
                    return Info.CurrentSection - Info.BeginSection;
                }
                else
                {
                    return 0;
                }
            }
        }

        public DownloadParameter currentParameter { get; set; }

        public abstract bool Analysis();


        public abstract bool Download();


        public void StopDownload()
        {
            if (currentParameter != null)
            {
                //将停止flag设置为true
                currentParameter.IsStop = true;
            }
        }

        //public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        //{
        //    info.AddValue("Total", Total);
        //    info.AddValue("Total", Done);
        //    info.AddValue("Total", Info);
        //    info.AddValue("Total", currentParameter);
        //}
    }
}
