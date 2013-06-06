using System;
using System.Globalization;
using System.Windows.Forms;
using CSNovelCrawler.Class;
using CSNovelCrawler.Interface;
using System.Collections.ObjectModel;
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
            CoreManager.TaskManager.PreDelegates.Exit = ExitProgram;
            //啟動自動儲存任務
            CoreManager.TaskManager.StartSaveBackgroundWorker();
            SubscribeTimer.Interval = CoreManager.ConfigManager.Settings.SubscribeTime*60000;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //載入任務到UI
            foreach (TaskInfo task in CoreManager.TaskManager.TaskInfos)
            {
                RefreshTask(new ParaRefresh(task));
            }
        }

        //重新整理UI
        private void RefreshTask(object e)
        {
            
            //如果不是安全執行緒，叫用 (Invoke) 方法
            if (InvokeRequired)
            {
                Invoke(new TaskDelegate(RefreshTask), e);
                return;
            }

            var r = (ParaRefresh)e;
           
            TaskInfo taskInfo = r.SourceTask;

            //如果任務已不在集合內
            if (!CoreManager.TaskManager.TaskInfos.Contains(taskInfo))
            {
                //移除
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
                //建立ListViewItem
                var lvi = new ListViewItem();
                for (int i = 0; i < 8; i++)
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
            lvi.SubItems[(int)LsvColumn.Subscribe].Text = taskInfo.GetSubscribe();
            lvi.SubItems[(int)LsvColumn.Status].Text = taskInfo.GetDownloadStatus();
            lvi.SubItems[(int)LsvColumn.Title].Text = taskInfo.Title.ToString(CultureInfo.InvariantCulture);
            lvi.SubItems[(int)LsvColumn.Progress].Text = string.Format(@"{0:P}", taskInfo.GetProgress());
            lvi.SubItems[(int)LsvColumn.TotalSection].Text = taskInfo.TotalSection.ToString(CultureInfo.InvariantCulture);
            lvi.SubItems[(int)LsvColumn.EndSection].Text = taskInfo.EndSection.ToString(CultureInfo.InvariantCulture);
            lvi.SubItems[(int)LsvColumn.CurrentSection].Text = taskInfo.CurrentSection.ToString(CultureInfo.InvariantCulture);
            lvi.SubItems[(int)LsvColumn.Author].Text = taskInfo.Author.ToString(CultureInfo.InvariantCulture); 
        }
        public enum LsvColumn
        {
            Subscribe = 0,
            Status,
            Title,
            Author,
            Progress,
            CurrentSection,
            EndSection,
            TotalSection,

        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {

            //Monitor.Enter(CoreManager.TaskManager.TaskInfosLock);
            foreach (var taskInfo in 
                CoreManager.TaskManager.TaskInfos.FindAll(taskInfo => taskInfo.Status == DownloadStatus.Downloading))
            {
                Invoke(new TaskDelegate(RefreshTask), new ParaRefresh(taskInfo));
                
            }
            //Monitor.Exit(CoreManager.TaskManager.TaskInfosLock);
        }

        /// <summary>
        /// 暫存最後選取的清單
        /// </summary>
        TaskInfo _selectedTaskInfo;
        private void lsv_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lv =(ListView) sender;
            DisableExtraOptions();
            if (lv.SelectedItems.Count > 0)
            {
                _selectedTaskInfo = null;

                if (lv.SelectedItems.Count == 1)
                {
                    ListViewItem sItem = lsv.SelectedItems[0];

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
        /// 用GUID找對應的任務
        /// </summary>
        public TaskInfo GetTask(Guid guid)
        {
            return CoreManager.TaskManager.GetTask(guid);
        }


        private void BtnBrowseDir_Click(object sender, EventArgs e)
        {
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
                Invoke(new TaskDelegate(RefreshTask), new ParaRefresh(taskInfo));
            }
        }


        private void UpdateTaskinfo_KeyUp(object sender, KeyEventArgs e)
        {
            switch (((TextBox)sender).Name)
            {
                case "txtBeginSection":
                    if (CommonTools.TryParse(txtBeginSection.Text, 0) < 1 )
                    {
                        txtBeginSection.Text = _selectedTaskInfo.BeginSection.ToString(CultureInfo.InvariantCulture);
                    }
                   
                    break;
                case "txtEndSection":
                    if (CommonTools.TryParse(txtEndSection.Text, 0) == 0 ||
                        CommonTools.TryParse(txtEndSection.Text, 0) > _selectedTaskInfo.TotalSection)
                        txtEndSection.Text = _selectedTaskInfo.EndSection.ToString(CultureInfo.InvariantCulture);
                    break;
            }
            UpdateTaskinfo();
        }

        private void SubscribeTimer_Tick(object sender, EventArgs e)
        {
            CoreManager.TaskManager.SubscribeTask();
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
                if (taskInfo.Status == DownloadStatus.Downloading)
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

			//取消選取的清單
			lsv.SelectedItems.Clear();

			foreach (TaskInfo taskInfo in willbedeleted)
			{
				CoreManager.TaskManager.DeleteTask(taskInfo);
			}
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            Show();
            WindowState = FormWindowState.Normal;

        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.UserClosing:
                    e.Cancel = true;
                    if (CoreManager.ConfigManager.Settings.HideSysTray)
                    {
                        
                        
                        notifyIcon1.Visible = true;
                        Invoke(new MethodInvoker(() => notifyIcon1.ShowBalloonTip(1500, "縮小到系統列", "連點圖示開啟視窗，或是按右鍵退出", ToolTipIcon.Info)));
                        
                        Hide();
                        break;
                    }
                    Cursor = Cursors.WaitCursor;
                    CoreManager.TaskManager.BreakAndSaveAllTasks();
                    break;

                case CloseReason.ApplicationExitCall:
                    break;

            } 
            
        }

     

        private void ExitProgram()
        {

            //如果不是安全執行緒，叫用 (Invoke) 方法
            if (InvokeRequired)
            {
                Invoke(new SysDelegate(ExitProgram));
                return;
            }
            //釋放系統列資源
            notifyIcon1.Visible = false;
            notifyIcon1.Dispose();

            Cursor = Cursors.Default;
             //退出程序
            Application.Exit();
        }



        private void 退出程式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            CoreManager.TaskManager.BreakAndSaveAllTasks();
        }

        private void toolStripSubscription_Click(object sender, EventArgs e)
        {
            //訂閱所有選取的任務
            foreach (ListViewItem item in lsv.SelectedItems)
            {
                TaskInfo taskInfo = GetTask(new Guid((string)item.Tag));
                CoreManager.TaskManager.SwitchSubscribe(taskInfo);
            }
        }

        private void 新增網址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormNew();
            form.Show();
        }

        private void 設定ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var config = new FormConfig();
            config.ShowDialog();
            config.Dispose();
          
        }

        private void 插件管理ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var plugins = new FormPlugins();
            plugins.ShowDialog();
            plugins.Dispose();
        }

        private void 開啟檔案所在資料夾ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lsv.SelectedItems.Count == 1)
            {
                var sItem = lsv.SelectedItems[0];
                var taskInfo = GetTask(new Guid((string)sItem.Tag));
                System.Diagnostics.Process.Start(taskInfo.SaveDirectory);
            }

        }

        private void 開啟檔案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lsv.SelectedItems.Count==1)
            {
                var sItem = lsv.SelectedItems[0];
                var taskInfo = GetTask(new Guid((string)sItem.Tag));
                System.Diagnostics.Process.Start(taskInfo.SaveFilePath);
            }
        }


      
   
       



     
        
    
    
        
    }

}
