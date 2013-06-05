using CSNovelCrawler.Class;

namespace CSNovelCrawler.Interface
{

    /// <summary>
    /// 下載狀態
    /// </summary>
    public enum DownloadStatus
    {
        任務分析中=0,
        出現錯誤=1,
        分析完畢=2,
        分析失敗=3 ,
        正在下載=4,
        下載完成=5,
        正在停止=6,
        任務暫停=7,
        正在刪除=8,
        下載失敗 = 9,
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
