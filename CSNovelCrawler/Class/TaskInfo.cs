using System;
using System.Xml.Serialization;
using CSNovelCrawler.Interface;

namespace CSNovelCrawler.Class
{
    public class TaskInfo
    {
        public TaskInfo()
        {
            Title = "Unknow";
            Author = "Unknow";
            Progress = 0;
            TotalSection = 0;
            _taskid =Guid.NewGuid();
        }
        private Guid _taskid;
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid TaskId
        {
            get
            {
                return _taskid;
            }
            set
            {
                _taskid = value;
            }
        }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPage { get; set; }



        /// <summary>
        /// 文章ID
        /// </summary>
        public string Tid { get; set; }

        /// <summary>
        /// 每一頁的章數
        /// </summary>
        public int PageSection { get; set; }

        /// <summary>
        /// 是否已停止下载(可以在下载过程中进行设置，用来控制下载过程的停止)
        /// </summary>
        public bool HasStopped { get; set; }

        /// <summary>
        /// 總章數
        /// </summary>
        public int TotalSection { get; set; }

        /// <summary>
        /// 起始章數
        /// </summary>
        public int BeginSection { get { return CurrentSection + 1; } set { CurrentSection = value - 1; } }

        /// <summary>
        /// 結束章數
        /// </summary>
        public int EndSection { get; set; }

        /// <summary>
        /// 目前章數
        /// </summary>
        public int CurrentSection { get; set; }

        /// <summary>
        /// 保存目錄
        /// </summary>
        public string SaveDirectory { get; set; }

        /// <summary>
        /// 完整路徑
        /// </summary>
        public string SaveFilePath
        {
            get
            {
                return string.Format("{0}\\{1}.txt" ,SaveDirectory , Title);
            }
        }

        /// <summary>
        /// 任務URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 关联的UI Item
        /// </summary>
        [XmlIgnore]
        public Object UiItem { get; set; }

        /// <summary>
        /// 下载状态
        /// </summary>
        public DownloadStatus Status { get; set; }

        private IPlugin _basePlugin;
        /// <summary>
        /// 包装的BasePlugin对象
        /// </summary>
        public IPlugin BasePlugin { get { return _basePlugin; } }

        private IDownloader _downloader;
        /// <summary>
        /// 包装的Downloader对象
        /// </summary>
        public IDownloader Downloader { get { return _downloader; } }

        /// <summary>
        /// 销毁关联的IDownloader对象
        /// </summary>
        public void DisposeDownloader()
        {
            _downloader = null;
        }

        internal double Progress;
        public void SetPlugin(IPlugin plugin)
        {
            _basePlugin=plugin;
        }


        /// <summary>
        /// 任务下载进度
        /// </summary>
        /// <returns></returns>
        public double GetProgress()
        {
            if (_downloader != null)
            {
                Progress = CurrentSection / (double)TotalSection;
                if (Progress < 0) Progress = 0.00;
                else if (Progress > 1.00) Progress = 1.00;
            }
            return Progress;
        }

        /// <summary>
        /// 分析任務
        /// </summary>
        public bool Analysis()
        {
            if (BasePlugin == null)
            {
                Status = DownloadStatus.出現錯誤;
                throw new Exception("Plugin Not Found");
            }
            if (_downloader==null)
            {
                _downloader = BasePlugin.CreateDownloader();
                _downloader.TaskInfo = this;
            }
           
            return _downloader.Analysis();
        }

        /// <summary>
        /// 分析任務
        /// </summary>
        public bool Start()
        {
            if (BasePlugin == null)
            {
                Status = DownloadStatus.出現錯誤;
                throw new Exception("Plugin Not Found");
            }
            if (_downloader == null)
            {
                _downloader = BasePlugin.CreateDownloader();
                _downloader.TaskInfo = this;
            }
            //resourceDownloader = BasePlugin.CreateDownloader();
            //resourceDownloader.taskInfo = this;


            return _downloader.Download();
        }

        /// <summary>
        /// 停止任務
        /// </summary>
        public void Stop()
        {
            if (_downloader != null)
                _downloader.StopDownload();
        }


    }
}
                              