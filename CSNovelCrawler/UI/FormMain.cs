using System;
using System.Globalization;
using System.Windows.Forms;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;
using System.Collections.ObjectModel;
using System.Threading;
using CSNovelCrawler.Core;
using CSNovelCrawler.Properties;


namespace CSNovelCrawler.UI
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            CoreManager.Initialize();
            //CoreManager.TaskManager.preDelegates.Start= new AcTaskDelegate(Updatelsv);
            CoreManager.TaskManager.PreDelegates.Refresh = RefreshTask;
            //啟動自動儲存任務
            CoreManager.TaskManager.StartSaveBackgroundWorker();

        }
/*
        private void Test()
        {
            string Url = string.Format("http://www02.eyny.com/member.php?mod=logging&action=login&loginsubmit=yes&handlekey=login&loginhash=LiKaw&inajax=1");

            ServicePointManager.Expect100Continue = false;
            string postdata = "formhash=3b765c67&referer=http%3A%2F%2Fwww02.eyny.com%2F&loginfield=username&username=''&password=''&questionid=0&answer=&cookietime=2592000";
            byte[] data = Encoding.UTF8.GetBytes(postdata);
            //生成请求
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Url);
            req.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
            req.Method = "POST";
            req.CookieContainer = new CookieContainer();

            using (var outstream = req.GetRequestStream())
            {
                outstream.Write(data, 0, data.Length);
                outstream.Flush();
            }
            //关闭请求
            req.GetResponse().Close();

            ////发送POST数据
            //Stream dataStream = req.GetRequestStream();
            //// 請求數據放入請求流  
            //dataStream.Write(data, 0, data.Length);
            //dataStream.Close();
            ////返回html  
            //HttpWebResponse httpWebResponse = (HttpWebResponse)req.GetResponse();
            //if (httpWebResponse.StatusCode == HttpStatusCode.OK)
            //{
            //    StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("GBK"));
            //    //讀取響應流  
            //    string responseFromServer = reader.ReadToEnd();
            //    reader.Close();
            //    dataStream.Close();
            //    httpWebResponse.Close();

            //    if (responseFromServer.IndexOf("賬號或密碼錯誤") > 0)
            //    {
            //        //LoginState = false;
            //        //Loginlabel.Text = "登錄狀態：失敗！";
            //    }
            //    else
            //    {
            //        //LoginState = true;
            //        //保存cookie  
            //        //gCookieCollention = httpWebResponse.Cookies;
            //        // cookie = httpWebRequest.CookieContainer;
            //        // Loginlabel.Text = "登錄狀態：成功！";
            //    }
            //}

            
        }
*/
     
       
        private void Form1_Load(object sender, EventArgs e)
        {
            //加载任务UI
            foreach (TaskInfo task in CoreManager.TaskManager.TaskInfos)
            {
                RefreshTask(new ParaRefresh(task));
            }
            

        }

        //刷新任务
        private void RefreshTask(object e)
        {

            //如果需要在安全的线程上下文中执行
            if (InvokeRequired)
            {
                Invoke(new AcTaskDelegate(RefreshTask), e);
                return;
            }

            var r = (ParaRefresh)e;
           
            TaskInfo taskInfo = r.SourceTask;

            //如果任务被删除
            if (!CoreManager.TaskManager.TaskInfos.Contains(taskInfo))
            {
                ////移除UI项
                if (lsv.Items.Contains((ListViewItem)taskInfo.UiItem))
                {
                    lsv.Items.Remove((ListViewItem)taskInfo.UiItem);
                }
                return;
            }
            //如果ListView已有此任務
            if (taskInfo.UiItem != null)
            {
                var lvi = (ListViewItem)taskInfo.UiItem;
                UpdateListViewItem(lvi, taskInfo);
            }
            else  //ListView不存在此任務
            {
                //新建ListViewItem
                var lvi = new ListViewItem();
                for (int i = 0; i < 6; i++)
                {
                    lvi.SubItems.Add("");
                }
               
                UpdateListViewItem(lvi, taskInfo);
                lvi.Tag = taskInfo.TaskId.ToString(); //设置TAG
                taskInfo.UiItem = lvi;
                lsv.Items.Add(lvi);
            }

        }

        private void UpdateListViewItem(ListViewItem lvi,TaskInfo taskInfo)
        {

            lvi.SubItems[GetColumn("Status")].Text = taskInfo.Status.ToString();
            lvi.SubItems[GetColumn("Title")].Text = taskInfo.Title.ToString(CultureInfo.InvariantCulture);
            lvi.SubItems[GetColumn("Progress")].Text = string.Format(@"{0:P}", taskInfo.GetProgress());
            lvi.SubItems[GetColumn("TotalSection")].Text = taskInfo.TotalSection.ToString(CultureInfo.InvariantCulture);
            lvi.SubItems[GetColumn("EndSection")].Text = taskInfo.EndSection.ToString(CultureInfo.InvariantCulture);
            lvi.SubItems[GetColumn("CurrentSection")].Text = taskInfo.CurrentSection.ToString(CultureInfo.InvariantCulture);
            lvi.SubItems[GetColumn("Author")].Text = taskInfo.Author.ToString(CultureInfo.InvariantCulture); 
        }

        /// <summary>
        /// 取得列所在位置
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private int GetColumn(string columnName)
        {
            foreach (ColumnHeader item in lsv.Columns)
            {
                if (item.Tag.ToString() == columnName)
                    return item.Index;
            }
            return -1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            Monitor.Enter(CoreManager.TaskManager.TaskInfosLock);
            foreach (TaskInfo taskInfo in CoreManager.TaskManager.TaskInfos)
            {
                if (taskInfo.Status == DownloadStatus.正在下載)
                {
                    Invoke(new AcTaskDelegate(RefreshTask), new ParaRefresh(taskInfo));
                }
            }
            Monitor.Exit(CoreManager.TaskManager.TaskInfosLock);
        }

        private void toolStripNew_Click(object sender, EventArgs e)
        {
            var form = new FormNew();
            form.Show();
        }
        TaskInfo _selectedTaskInfo;
        private void lsv_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lv =(ListView) sender;
            DisableExtraOptions();
            if (lv.SelectedItems.Count > 0)
            {
                _selectedTaskInfo = null;
                //SelectedTaskInfos.Clear();
                //取得最后一项
                ListViewItem sItem = lsv.SelectedItems[lv.SelectedIndices.Count - 1];
                //显示"更多"菜单
                if (lv.SelectedItems.Count == 1)
                {

                    TaskInfo taskInfo = GetTask(new Guid((string)sItem.Tag));
                   
                    txtBeginSection.Text = taskInfo.BeginSection.ToString(CultureInfo.InvariantCulture);
                    txtBeginSection.Enabled = true;
                    txtEndSection.Text = taskInfo.EndSection.ToString(CultureInfo.InvariantCulture);
                    txtEndSection.Enabled = true;
                    txtTitle.Text = taskInfo.Title.ToString(CultureInfo.InvariantCulture);
                    txtTitle.Enabled = true;
                    _selectedTaskInfo = taskInfo;
                    cbSaveDir.Text = taskInfo.SaveDirectory.ToString(CultureInfo.InvariantCulture);
                    cbSaveDir.Enabled = true;
                    //task.SaveDirectory = new DirectoryInfo(CoreManager.ConfigManager.Settings.SavePath);
                }
            }
            
        }

        private void DisableExtraOptions()
        {
                txtBeginSection.Text = string.Empty;
                txtBeginSection.Enabled = false;
                txtEndSection.Text = string.Empty;
                txtEndSection.Enabled = false;
                txtTitle.Text = string.Empty;
                txtTitle.Enabled = false;
                cbSaveDir.Text = string.Empty;
                cbSaveDir.Enabled = false;
        }

        /// <summary>
        /// 根据GUID值寻找对应的任务
        /// </summary>
        public TaskInfo GetTask(Guid guid)
        {
            return CoreManager.TaskManager.GetTask(guid);
        }

        private void BtnBrowseDir_Click(object sender, EventArgs e)
        {
            //选择文件夹
            var fbd = new FolderBrowserDialog
                {
                    ShowNewFolderButton = true,
                    Description = Resources.FormMain_BtnBrowseDir_Click_選擇你的下載資料夾,
                    SelectedPath = cbSaveDir.Text
                };

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                cbSaveDir.Text = fbd.SelectedPath;
                UpdateTaskinfo();
            }
        }
        private void UpdateTaskinfo()
        {
            if (_selectedTaskInfo != null)
            {
                TaskInfo taskInfo = _selectedTaskInfo;

                taskInfo.BeginSection = CommonTools.TryParse(txtBeginSection.Text, 1);
                taskInfo.EndSection = CommonTools.TryParse(txtEndSection.Text, 1);
                taskInfo.Title = txtTitle.Text;
                taskInfo.SaveDirectory =cbSaveDir.Text;
                //RefreshTask(new ParaRefresh(taskInfo));
                Invoke(new AcTaskDelegate(RefreshTask), new ParaRefresh(taskInfo));
            }
        }



        private void UpdateTaskinfo_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateTaskinfo();
        }

        private void toolStripStart_Click(object sender, EventArgs e)
        {
            //開始下載所有選取的任務
            foreach (ListViewItem item in lsv.SelectedItems)
            {
                TaskInfo taskInfo = GetTask(new Guid((string)item.Tag));
                CoreManager.TaskManager.StartTask(taskInfo);
            }
        }

        private void toolStripAnalysis_Click(object sender, EventArgs e)
        {
            //重新分析所有選取的任務
            foreach (ListViewItem item in lsv.SelectedItems)
            {
                TaskInfo taskInfo = GetTask(new Guid((string)item.Tag));
                CoreManager.TaskManager.AnalysisTask(taskInfo);
            }
        }


        private void toolStripStop_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show(Resources.FormMain_toolStripStop_Click_是否停止選取的下載_, Resources.FormMain_toolStripStop_Click_停止下載,
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                 MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            //停止選取的下載
            foreach (ListViewItem item in lsv.SelectedItems)
            {
                TaskInfo taskInfo = GetTask(new Guid((string)item.Tag));
                if (taskInfo.Status == DownloadStatus.正在下載)
                {
                    CoreManager.TaskManager.StopTask(taskInfo);
                }
            }

        }

        private void toolStripDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.FormMain_toolStripDel_Click_, Resources.FormMain_toolStripDel_Click_刪除任務,
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                 MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

			var willbedeleted = new Collection<TaskInfo>();
			foreach (ListViewItem item in lsv.SelectedItems)
			{
				TaskInfo task = GetTask(new Guid((string)item.Tag));
				willbedeleted.Add(task);
			}

			//取消选中所有任务
			lsv.SelectedItems.Clear();

			foreach (TaskInfo taskInfo in willbedeleted)
			{
				CoreManager.TaskManager.DeleteTask(taskInfo);
			}
        }

        private void 設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var config = new FormConfig();
            config.ShowDialog();
            config.Dispose();
            ////重新加载某些项目
            ////检查更新
            //if (CoreManager.ConfigManager.Settings.CheckUpdate)
            //    CheckUpdate();
            ////刷新“同时进行的任务数”设置
            //CoreManager.TaskManager.ContinueNext();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;

        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (e.CloseReason == CloseReason.UserClosing && CoreManager.ConfigManager.Settings.HideSysTray)
            {
                e.Cancel = true;
                Hide();
            }
            
        }

        private void ExitProgram()
        {
            Cursor = Cursors.WaitCursor;
            //停止自動儲存
            CoreManager.TaskManager.EndSaveBackgroundWorker();
            //儲存所有任務
            var t = new Thread(CoreManager.TaskManager.SaveAllTasks);
            t.Start();
            Cursor = Cursors.Default;
            //釋放系統列資源
            notifyIcon1.Dispose();

            //退出程序
            Application.Exit();
           

        }

        private void 退出程式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitProgram();
        }

        private void 插件管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var plugins = new FormPlugins();
            plugins.ShowDialog();
            plugins.Dispose();
        }



     
        
    
    
        
    }

}
