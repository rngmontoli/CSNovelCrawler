﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CSNovelCrawler.Core
{
    public class TaskManager
    {
        //所有任务
        public List<TaskInfo> TaskInfos = new List<TaskInfo>();
        //TaskInfos对象的全局锁
        public object TaskInfosLock = new object();
        public UIDelegateContainer preDelegates = new UIDelegateContainer();
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="plugin">任务所属的插件引用</param>
        /// <param name="url">任务Url</param>
        /// <param name="proxySetting">代理服务器设置</param>
        /// <param name="downSub">下载字幕文件设置</param>
        /// <returns></returns>
        public TaskInfo AddTask(IPlugin plugin, string url)
        {
            //新建TaskInfo对象
            TaskInfo Info = new TaskInfo();
            Info.SaveDirectory = CoreManager.ConfigManager.Settings.DefaultSaveFolder;
            Info.Url = url;
            Info.SetPlugin(plugin);
            Info.Status = DownloadStatus.任務分析中;
       
            //向集合中添加对象
            Monitor.Enter(TaskInfosLock);
            TaskInfos.Add(Info);
            Monitor.Exit(TaskInfosLock);
            //提示UI刷新信息
            //if (delegates.Refresh != null)
            //	delegates.Refresh.Invoke(new ParaRefresh(task.TaskId));
            return Info;
        }

        public void AnalysisTask(TaskInfo Info)
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    Info.Status = DownloadStatus.任務分析中;
                    this.preDelegates.Refresh(new ParaRefresh(Info));
                   

                    if (Info.Analysis())
                    {
                        Info.Status = DownloadStatus.分析完畢;
                        this.preDelegates.Refresh(new ParaRefresh(Info));
                    }

                }
                catch (Exception ex) //如果出现错误
                {
                    Info.Status = DownloadStatus.出現錯誤;
                    //preDelegates.Error(new ParaError(task, ex));
                }

            });
            t.IsBackground = true;
            //开始
            t.Start();
            //刷新UI
            this.preDelegates.Refresh(new ParaRefresh(Info));
        }

        public void StartTask(TaskInfo Info)
        {
            Thread t = new Thread(() =>
            {
                try
                {
                    Info.Status = DownloadStatus.正在下載;
                    this.preDelegates.Refresh(new ParaRefresh(Info));
                    if (Info.Start())
                    {
                        Info.Status = DownloadStatus.下載完成;
                        this.preDelegates.Refresh(new ParaRefresh(Info));
                    }

                }
                catch (Exception ex) //如果出现错误
                {
                    Info.Status = DownloadStatus.出現錯誤;
                    //preDelegates.Error(new ParaError(task, ex));
                }

            });
            t.IsBackground = true;
            //开始下载
            t.Start();
            //刷新UI
            this.preDelegates.Refresh(new ParaRefresh(Info));
        }
        /// <summary>
        /// 停止任务
        /// </summary>
        /// <param name="Info"></param>
        public void StopTask(TaskInfo Info)
        {
            //只有已开始的任务才可停止
            switch (Info.Status)
            {
              
                case DownloadStatus.正在下載: //已经开始的任务启动新线程停止
                    Info.Status = DownloadStatus.正在停止;
                    break;
                default:
                    return;
            }

            //刷新信息
            preDelegates.Refresh(new ParaRefresh(Info));
            //停止任务
            Info.Stop();

            if (Info.Status != DownloadStatus.任務暫停)
            {
                //启动新线程等待任务完全停止
                Thread t = new Thread(new ThreadStart(() =>
                {
                    //超时时长 (10秒钟)
                    int timeout = 10000;
                    //等待停止
                    while (Info.Status == DownloadStatus.正在停止)
                    {
                        Thread.Sleep(500);
                        timeout -= 500;
                        if (timeout < 0) //如果到时仍未停止
                        {
                            Info.Status = DownloadStatus.任務暫停;
                            break;
                        }
                    }
                    //刷新信息
                    preDelegates.Refresh(new ParaRefresh(Info));
                }));
                t.IsBackground = true;
                t.Start();
            }
            //销毁Downloader
            Info.DisposeDownloader();
        }
        /// <summary>
        /// 删除任务(自动终止未停止的任务)
        /// </summary>
        /// <param name="Info"></param>
        public void DeleteTask(TaskInfo Info)
        {
            //停止任务
            StopTask(Info);

            //启动新线程等待任务完全停止

            ThreadPool.QueueUserWorkItem(new WaitCallback((o) =>
            {
                while (Info.Status == DownloadStatus.正在停止 || Info.Status == DownloadStatus.正在下載)
                {
                    Thread.Sleep(50);
                }

                //从任务列表中删除任务
                if (Info.Status != DownloadStatus.正在刪除)
                {
                    TaskInfos.Remove(Info);
                }
               

                //刷新信息
                preDelegates.Refresh(new ParaRefresh(Info));
            }));
        }



        /// <summary>
        /// 根据GUID值寻找对应的任务
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [DebuggerNonUserCode()]
        public TaskInfo GetTask(Guid guid)
        {
            foreach (var i in TaskInfos)
            {
                if (i.TaskId == guid)
                    return i;
            }
            return null;
        }


        private bool _bgWorkerContinue;
        private Timer _bgWorker;
        /// <summary>
        /// 启动后台自动保存任务的进程
        /// </summary>
        public void StartSaveBackgroundWorker()
        {
            _bgWorkerContinue = true;
            if (_bgWorker == null)
            {
                //每60秒自动保存一次任务状态信息
                _bgWorker = new Timer(new TimerCallback(SaveBackgroundWorker), null, 60000, 60000);
            }
        }

        /// <summary>
        /// 结束侯台自动保存任务的进程
        /// </summary>
        public void EndSaveBackgroundWorker()
        {
            _bgWorkerContinue = false;
        }

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


        private object saveTaskLock = new object();


        private string _TaskFolderPath { get { return CoreManager.StartupPath; } }
        private string _TaskFileName { get { return "Task.xml"; } }
        private string _TaskFullFileName { get { return Path.Combine(_TaskFolderPath, _TaskFileName); } }

        /// <summary>
        /// 保存所有任务到文件中
        /// </summary>
        public void SaveAllTasks()
        {
            lock (saveTaskLock)
            {

                try
                {
                    using (FileStream oFileStream = new FileStream(_TaskFullFileName, FileMode.Create))
                    {

                        XmlSerializer oXmlSerializer = new XmlSerializer(typeof(List<TaskInfo>));

                        oXmlSerializer.Serialize(oFileStream, TaskInfos);

                        oFileStream.Close();


                    }
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }

                ////保证TaskInfos对象不会被意外回收
                //GC.KeepAlive(TaskInfos);
            }
        }
        /// <summary>
        /// 从文件中读取任务列表
        /// </summary>
        public void LoadAllTasks()
        {

            //如果文件存在
            if (File.Exists(_TaskFullFileName))
            {
                try
                {
                    using (FileStream oFileStream = new FileStream(_TaskFullFileName, FileMode.Open))
                    {
                        XmlSerializer oXmlSerializer = new XmlSerializer(typeof(List<TaskInfo>));
                        TaskInfos = (List<TaskInfo>)oXmlSerializer.Deserialize(oFileStream);
                        oFileStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
            }

            foreach (TaskInfo Info in TaskInfos)
            {
                //寻找所需插件
                if (Info.BasePlugin == null)
                {
                    Info.SetPlugin(CoreManager.PluginManager.GetPlugin(Info.Url));
                    //this.preDelegates.Refresh(new ParaRefresh(Info));
                }
            }

        }


       

    }
}
