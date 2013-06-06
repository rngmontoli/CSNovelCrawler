using System.ComponentModel;
using CSNovelCrawler.Class;

namespace CSNovelCrawler.Interface
{


    /// <summary>
    /// 下載狀態
    /// </summary>
    public enum DownloadStatus
    {
        [Description("任務分析中")]
        TaskAnalysis,
        [Description("出現錯誤")]
        Error,
        [Description("分析完畢")]
        AnalysisComplete,
        [Description("分析失敗")]
        AnalysisFailed,
        [Description("正在下載")]
        Downloading,
        [Description("下載完成")]
        DownloadComplete,
        [Description("正在停止")]
        Stopping,
        [Description("任務暫停")]
        TaskPause,
        [Description("正在刪除")]
        Deleting,
        [Description("下載中斷")]
        Downloadblocked,
        [Description("檢查訂閱")]
        SubscribeCheck,
        [Description("更新訂閱")]
        SubscribeUpdate
    }

    


    /// <summary>
    /// 下载介面
    /// </summary>
    public interface IDownloader
    {
      
        /// <summary>
        /// 下載的任務訊息
        /// </summary>
        TaskInfo TaskInfo { get; set; }
        /// <summary>
        /// 下載參數
        /// </summary>
        DownloadParameter CurrentParameter { get; set; }
        /// <summary>
        /// 分析頁面資訊
        /// </summary>
        /// <returns></returns>
        bool Analysis();

        /// <summary>
        /// 開始下载
        /// </summary>
        /// <returns></returns>
        bool Download();
        /// <summary>
        /// 停止下载
        /// </summary>
        void StopDownload();
    }
}
