using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Threading;
using CSNovelCrawler.Core;


namespace CSNovelCrawler.UI
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            CoreManager.Initialize();
            //CoreManager.TaskManager.preDelegates.Start= new AcTaskDelegate(Updatelsv);
            CoreManager.TaskManager.preDelegates.Refresh = new AcTaskDelegate(RefreshTask);
            //啟動自動儲存任務
            CoreManager.TaskManager.StartSaveBackgroundWorker();

        }
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
            if (this.InvokeRequired)
            {
                this.Invoke(new AcTaskDelegate(RefreshTask), e);
                return;
            }

            ParaRefresh r = (ParaRefresh)e;
           
            TaskInfo Info = r.SourceTask;

            //如果任务被删除
            if (!CoreManager.TaskManager.TaskInfos.Contains(Info))
            {
                ////移除UI项
                if (lsv.Items.Contains((ListViewItem)Info.UIItem))
                {
                    lsv.Items.Remove((ListViewItem)Info.UIItem);
                }
                return;
            }
            //如果ListView已有此任務
            if (Info.UIItem != null)
            {
                ListViewItem lvi = (ListViewItem)Info.UIItem;
                UpdateListViewItem(lvi, Info);
            }
            else  //ListView不存在此任務
            {
                //新建ListViewItem
                ListViewItem lvi = new ListViewItem();
                for (int i = 0; i < 6; i++)
                {
                    lvi.SubItems.Add("");
                }
               
                UpdateListViewItem(lvi, Info);
                lvi.Tag = Info.TaskId.ToString(); //设置TAG
                Info.UIItem = lvi;
                lsv.Items.Add(lvi);
            }

        }

        private void UpdateListViewItem(ListViewItem lvi,TaskInfo Info)
        {

            lvi.SubItems[GetColumn("Status")].Text = Info.Status.ToString();
            lvi.SubItems[GetColumn("Title")].Text = Info.Title.ToString();
            lvi.SubItems[GetColumn("Progress")].Text = string.Format(@"{0:P}", Info.GetProgress());
            lvi.SubItems[GetColumn("TotalSection")].Text = Info.TotalSection.ToString();
            lvi.SubItems[GetColumn("EndSection")].Text = Info.EndSection.ToString();
            lvi.SubItems[GetColumn("CurrentSection")].Text = Info.CurrentSection.ToString();
            lvi.SubItems[GetColumn("Author")].Text = Info.Author.ToString(); 
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
            foreach (TaskInfo Info in CoreManager.TaskManager.TaskInfos)
            {
                if (Info.Status == DownloadStatus.正在下載)
                {
                    this.Invoke(new AcTaskDelegate(RefreshTask), new ParaRefresh(Info));
                }
            }
            Monitor.Exit(CoreManager.TaskManager.TaskInfosLock);
        }

        private void toolStripNew_Click(object sender, EventArgs e)
        {
            FormNew Form = new FormNew();
            Form.Show();
        }
        TaskInfo SelectedTaskInfo=null;
        private void lsv_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView lv =(ListView) sender;
            DisableExtraOptions();
            if (lv.SelectedItems.Count > 0)
            {
                SelectedTaskInfo = null;
                //SelectedTaskInfos.Clear();
                //取得最后一项
                ListViewItem sItem = lsv.SelectedItems[lv.SelectedIndices.Count - 1];
                //显示"更多"菜单
                if (lv.SelectedItems.Count == 1)
                {

                    TaskInfo Info = GetTask(new Guid((string)sItem.Tag));
                   
                    txtBeginSection.Text = Info.BeginSection.ToString();
                    txtBeginSection.Enabled = true;
                    txtEndSection.Text = Info.EndSection.ToString();
                    txtEndSection.Enabled = true;
                    txtTitle.Text = Info.Title.ToString();
                    txtTitle.Enabled = true;
                    SelectedTaskInfo = Info;
                    cbSaveDir.Text = Info.SaveDirectory.ToString();
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
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.Description = "为您的下载选择一个目标文件夹：";
            fbd.SelectedPath = cbSaveDir.Text;
            
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                cbSaveDir.Text = fbd.SelectedPath;
                UpdateTaskinfo();
            }
        }
        private void UpdateTaskinfo()
        {
            if (SelectedTaskInfo != null)
            {
                TaskInfo Info = SelectedTaskInfo;

                Info.BeginSection = CommonTools.TryParse(txtBeginSection.Text, 1);
                Info.EndSection = CommonTools.TryParse(txtEndSection.Text, 1);
                Info.Title = txtTitle.Text;
                Info.SaveDirectory =cbSaveDir.Text;
                //RefreshTask(new ParaRefresh(Info));
                this.Invoke(new AcTaskDelegate(RefreshTask), new ParaRefresh(Info));
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
                TaskInfo Info = GetTask(new Guid((string)item.Tag));
                CoreManager.TaskManager.StartTask(Info);
            }
        }

        private void toolStripAnalysis_Click(object sender, EventArgs e)
        {
            //重新分析所有選取的任務
            foreach (ListViewItem item in lsv.SelectedItems)
            {
                TaskInfo Info = GetTask(new Guid((string)item.Tag));
                CoreManager.TaskManager.AnalysisTask(Info);
            }
        }


        private void toolStripStop_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("是否停止選取的下載?", "停止下載",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                 MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            //停止選取的下載
            foreach (ListViewItem item in lsv.SelectedItems)
            {
                TaskInfo Info = GetTask(new Guid((string)item.Tag));
                if (Info.Status == DownloadStatus.正在下載)
                {
                    CoreManager.TaskManager.StopTask(Info);
                }
            }

        }

        private void toolStripDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否刪除選取的任務？", "刪除任務",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                 MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

			Collection<TaskInfo> willbedeleted = new Collection<TaskInfo>();
			foreach (ListViewItem item in lsv.SelectedItems)
			{
				TaskInfo task = GetTask(new Guid((string)item.Tag));
				willbedeleted.Add(task);
			}

			//取消选中所有任务
			lsv.SelectedItems.Clear();

			foreach (TaskInfo Info in willbedeleted)
			{
				CoreManager.TaskManager.DeleteTask(Info);
			}
        }

        private void 設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfig config = new FormConfig();
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
            this.Show();
            this.WindowState = FormWindowState.Normal;

        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (e.CloseReason == CloseReason.UserClosing && CoreManager.ConfigManager.Settings.HideSysTray)
            {
                e.Cancel = true;
                this.Hide();
            }
            
        }

        private void ExitProgram()
        {
            this.Cursor = Cursors.WaitCursor;
            //停止自動儲存
            CoreManager.TaskManager.EndSaveBackgroundWorker();
            //儲存所有任務
            Thread t = new Thread(new ThreadStart(new MethodInvoker(CoreManager.TaskManager.SaveAllTasks)));
            t.Start();
            this.Cursor = Cursors.Default;
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
            FormPlugins Plugins = new FormPlugins();
            Plugins.ShowDialog();
            Plugins.Dispose();
        }



     
        
    
    
        
    }

}
