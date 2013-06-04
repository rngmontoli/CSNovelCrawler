using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CSNovelCrawler
{

    /// <summary>
    /// 下载状态
    /// </summary>
    public enum DownloadStatus
    {
        任務分析中 = 0,
        出現錯誤 = 1,
        分析完畢 = 2,
        正在下載=3,
        下載完成=4,
        正在停止,
        任務暫停,
        正在刪除
        //等待开始 = 0,
        //正在下载 = 1,
        //已经停止 = 2,
        //下载完成 = 3,
        //出现错误 = 4,
        //正在停止 = 5,
        //已删除 = 6,
        //部分完成 = 7
    }

    /// <summary>
    /// 下载适配器接口
    /// </summary>
    public interface IDownloader
    {
        ///// <summary>
        ///// 任务管理器委托
        ///// </summary>
        //DelegateContainer delegates { get; set; }
        /// <summary>
        /// 获取或设置与此任务相关联的信息
        /// </summary>
        TaskInfo Info { get; set; }
        ///// <summary>
        ///// 此任务总长度
        ///// </summary>
        //int Total { get; }
        ///// <summary>
        ///// 此任务已经完成的长度
        ///// </summary>
        //int Done { get; }
        /// <summary>
        /// 下載參數
        /// </summary>
        DownloadParameter currentParameter { get; set; }


        /// <summary>
        /// 分析頁面資訊
        /// </summary>
        /// <returns></returns>
        bool Analysis();
        /// <summary>
        /// 开始下载
        /// </summary>
        /// <returns></returns>
        bool Download();
        /// <summary>
        /// 停止下载
        /// </summary>
        void StopDownload();
    }
}
