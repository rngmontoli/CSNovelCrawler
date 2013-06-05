using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using System.Threading;
using System.Xml.Serialization;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;

namespace CSNovelCrawler.Core
{
    public class TaskManager
    {
        //存放任務
        public List<TaskInfo> TaskInfos = new List<TaskInfo>();
        //TaskInfos的全域鎖定
        public object TaskInfosLock = new object();
        public DelegateContainer PreDelegates = new DelegateContainer();

        /// <summary>
        /// 增加任務
        /// </summary>
        /// <param name="plugin">任務的插件</param>
        /// <param name="url">任務網址</param>
        /// <returns></returns>
        public TaskInfo AddTask(IPlugin plugin, string url)
        {
            //建立TaskInfo物件
            var taskInfo = new TaskInfo
                {
                    SaveDirectory = CoreManager.ConfigManager.Settings.DefaultSaveFolder,
                    Url = url
                };
            taskInfo.SetPlugin(plugin);
            taskInfo.Status = DownloadStatus.任務分析中;

            //向任務集合增加新任務
            Monitor.Enter(TaskInfosLock);
            TaskInfos.Add(taskInfo);
            Monitor.Exit(TaskInfosLock);
            //重新整理UI
            if (PreDelegates.Refresh != null)
                PreDelegates.Refresh(new ParaRefresh(taskInfo));
            return taskInfo;
        }

        public void AnalysisTask(TaskInfo taskInfo)
        {
            var t = new Thread(() =>
            {
                try
                {
                    taskInfo.Status = DownloadStatus.任務分析中;
                    PreDelegates.Refresh(new ParaRefresh(taskInfo));
                   

                    if (taskInfo.Analysis())
                    {
                        taskInfo.Status = DownloadStatus.分析完畢;
                        PreDelegates.Refresh(new ParaRefresh(taskInfo));
                    }
                    else
                    {
                        taskInfo.Status = DownloadStatus.分析失敗;
                        PreDelegates.Refresh(new ParaRefresh(taskInfo));
                    }

                }
                catch (Exception)
                {
                    taskInfo.Status = DownloadStatus.出現錯誤;
                    PreDelegates.Refresh(new ParaRefresh(taskInfo));
                }

            }) {IsBackground = true};
            
            t.Start();
            //重新整理UI
            PreDelegates.Refresh(new ParaRefresh(taskInfo));
        }

        public void StartTask(TaskInfo taskInfo)
        {
            var t = new Thread(() =>
            {
                try
                {
                    taskInfo.Status = DownloadStatus.正在下載;
                    PreDelegates.Refresh(new ParaRefresh(taskInfo));
                    if (taskInfo.Start())
                    {
                        taskInfo.Status = DownloadStatus.下載完成;
                        PreDelegates.Refresh(new ParaRefresh(taskInfo));
                    }
                    else
                    {
                        taskInfo.Status = DownloadStatus.下載失敗;
                        PreDelegates.Refresh(new ParaRefresh(taskInfo));
                    }

                }
                catch (Exception ) //如果出现错误
                {
                    taskInfo.Status = DownloadStatus.出現錯誤;
                    PreDelegates.Refresh(new ParaRefresh(taskInfo));
                }

            }) {IsBackground = true};

            t.Start();
            //重新整理UI
            PreDelegates.Refresh(new ParaRefresh(taskInfo));
        }
        /// <summary>
        /// 停止任務
        /// </summary>
        /// <param name="taskInfo"></param>
        public void StopTask(TaskInfo taskInfo)
        {
            //只有已開始的任務才能停止
            switch (taskInfo.Status)
            {
              
                case DownloadStatus.正在下載:
                    taskInfo.Status = DownloadStatus.正在停止;
                    
                    break;
                default:
                    taskInfo.Status = DownloadStatus.任務暫停;
                    return;
            }

            //重新整理UI
            PreDelegates.Refresh(new ParaRefresh(taskInfo));
            //停止任務
            taskInfo.Stop();

            if (taskInfo.Status != DownloadStatus.任務暫停)
            {
                //啟動新執行緒等待任務完全停止
                var t = new Thread(() =>
                    {
                        //等待時間 (10秒)
                        int timeout = 10000;
                        //等待停止
                        while (taskInfo.Status == DownloadStatus.正在停止)
                        {
                            Thread.Sleep(500);
                            timeout -= 500;
                            if (timeout < 0 || taskInfo.HasStopped) //如果到时仍未停止
                            {
                                break;
                            }
                        }

                        taskInfo.Status = DownloadStatus.任務暫停;
                        //重新整理UI
                        PreDelegates.Refresh(new ParaRefresh(taskInfo));
                    }) {IsBackground = true};
                t.Start();
            }
            //釋放Downloader
            taskInfo.DisposeDownloader();
        }
        /// <summary>
        /// 刪除任任務(自動停止進行中的任務)
        /// </summary>
        /// <param name="taskInfo"></param>
        public void DeleteTask(TaskInfo taskInfo)
        {
            //停止任務
            StopTask(taskInfo);

            //啟動新執行緒等待任務完全停止
            ThreadPool.QueueUserWorkItem(o =>
                {
                    try
                    {
                        while (taskInfo.Status == DownloadStatus.正在停止 || taskInfo.Status == DownloadStatus.正在下載)
                        {
                            Thread.Sleep(50);
                        }

                        TaskInfos.Remove(taskInfo);

                        //重新整理UI
                        PreDelegates.Refresh(new ParaRefresh(taskInfo));
                    }
                    catch (Exception)
                    {
                        taskInfo.Status = DownloadStatus.出現錯誤;
                        PreDelegates.Refresh(new ParaRefresh(taskInfo));
                    }
                });
        }



        /// <summary>
        /// 用GUID找對應的任務
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public TaskInfo GetTask(Guid guid)
        {

            return TaskInfos.Find(taskInfo => taskInfo.TaskId == guid);
        }


        private bool _bgWorkerContinue;
        private Timer _bgWorker;
        /// <summary>
        /// 啟動背景自動儲存任務
        /// </summary>
        public void StartSaveBackgroundWorker()
        {
            _bgWorkerContinue = true;
            if (_bgWorker == null)
            {
                //每60秒儲存任務資訊
                _bgWorker = new Timer(SaveBackgroundWorker, null, 60000, 60000);
            }
        }

        /// <summary>
        /// 停止背景自動儲存任務
        /// </summary>
        public void EndSaveBackgroundWorker()
        {
            _bgWorkerContinue = false;
        }

        /// <summary>
        /// 背景自動儲存任務
        /// </summary>
        /// <param name="o"></param>
        private void SaveBackgroundWorker(object o)
        {
            if (_bgWorkerContinue)
            {
                SaveAllTasks();
            }
            else
            {
                _bgWorker.Dispose();
            }
        }

        private string _TaskFolderPath { get { return CoreManager.StartupPath; } }
        private string _TaskFileName { get { return "Task.xml"; } }
        private string TaskFullFileName { get { return Path.Combine(_TaskFolderPath, _TaskFileName); } }

        /// <summary>
        /// 儲存任務列表到xml
        /// </summary>
        public void SaveAllTasks()
        {
            Monitor.Enter(TaskInfosLock);
            using (FileStream oFileStream = new FileStream(TaskFullFileName, FileMode.Create))
            {
                XmlSerializer oXmlSerializer = new XmlSerializer(typeof(List<TaskInfo>));
                oXmlSerializer.Serialize(oFileStream, TaskInfos);
                oFileStream.Close();
            }
            Monitor.Exit(TaskInfosLock);
        }

        /// <summary>
        /// 從xml讀取任務列表
        /// </summary>
        public void LoadAllTasks()
        {

            //如果文件存在
            if (File.Exists(TaskFullFileName))
            {
                using (FileStream oFileStream = new FileStream(TaskFullFileName, FileMode.Open))
                {
                    XmlSerializer oXmlSerializer = new XmlSerializer(typeof(List<TaskInfo>));
                    TaskInfos = (List<TaskInfo>)oXmlSerializer.Deserialize(oFileStream);
                    oFileStream.Close();
                }
            }

            foreach (TaskInfo taskInfo in TaskInfos)
            {
                //尋找對應插件
                if (taskInfo.BasePlugin == null)
                {
                    taskInfo.SetPlugin(CoreManager.PluginManager.GetPlugin(taskInfo.Url));
                }
            }
        }


       

    }
}
