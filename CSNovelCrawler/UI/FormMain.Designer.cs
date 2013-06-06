namespace CSNovelCrawler.UI
{
    partial class FormMain
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.退出程式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SubscribeTimer = new System.Windows.Forms.Timer(this.components);
            this.toolStripSubscription = new System.Windows.Forms.ToolStripButton();
            this.toolStripStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripAnalysis = new System.Windows.Forms.ToolStripButton();
            this.toolStripDel = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.新增網址ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.其他ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.設定ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.插件管理ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.開啟檔案所在位置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new CSNovelCrawler.UI.SplitContainerEx();
            this.lsv = new System.Windows.Forms.ListView();
            this.Subscribe = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Author = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Progress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CurrentSection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EndSection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TotalSection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBeginSection = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtEndSection = new System.Windows.Forms.TextBox();
            this.BtnBrowseDir = new System.Windows.Forms.Button();
            this.cbSaveDir = new System.Windows.Forms.ComboBox();
            this.開啟檔案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.退出程式ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // 退出程式ToolStripMenuItem
            // 
            this.退出程式ToolStripMenuItem.Name = "退出程式ToolStripMenuItem";
            this.退出程式ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.退出程式ToolStripMenuItem.Text = "退出程式";
            this.退出程式ToolStripMenuItem.Click += new System.EventHandler(this.退出程式ToolStripMenuItem_Click);
            // 
            // SubscribeTimer
            // 
            this.SubscribeTimer.Enabled = true;
            this.SubscribeTimer.Tick += new System.EventHandler(this.SubscribeTimer_Tick);
            // 
            // toolStripSubscription
            // 
            this.toolStripSubscription.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSubscription.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSubscription.Image")));
            this.toolStripSubscription.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSubscription.Name = "toolStripSubscription";
            this.toolStripSubscription.Size = new System.Drawing.Size(36, 22);
            this.toolStripSubscription.Text = "訂閱";
            this.toolStripSubscription.Click += new System.EventHandler(this.toolStripSubscription_Click);
            // 
            // toolStripStart
            // 
            this.toolStripStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStart.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStart.Image")));
            this.toolStripStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStart.Name = "toolStripStart";
            this.toolStripStart.Size = new System.Drawing.Size(36, 22);
            this.toolStripStart.Text = "開始";
            this.toolStripStart.Click += new System.EventHandler(this.toolStripStart_Click);
            // 
            // toolStripStop
            // 
            this.toolStripStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStop.Image")));
            this.toolStripStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripStop.Name = "toolStripStop";
            this.toolStripStop.Size = new System.Drawing.Size(36, 22);
            this.toolStripStop.Text = "停止";
            this.toolStripStop.Click += new System.EventHandler(this.toolStripStop_Click);
            // 
            // toolStripAnalysis
            // 
            this.toolStripAnalysis.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripAnalysis.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAnalysis.Image")));
            this.toolStripAnalysis.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAnalysis.Name = "toolStripAnalysis";
            this.toolStripAnalysis.Size = new System.Drawing.Size(60, 22);
            this.toolStripAnalysis.Text = "重新分析";
            this.toolStripAnalysis.Click += new System.EventHandler(this.toolStripAnalysis_Click);
            // 
            // toolStripDel
            // 
            this.toolStripDel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDel.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDel.Image")));
            this.toolStripDel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDel.Name = "toolStripDel";
            this.toolStripDel.Size = new System.Drawing.Size(36, 22);
            this.toolStripDel.Text = "刪除";
            this.toolStripDel.Click += new System.EventHandler(this.toolStripDel_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSubscription,
            this.toolStripStart,
            this.toolStripStop,
            this.toolStripAnalysis,
            this.toolStripDel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(719, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新增網址ToolStripMenuItem,
            this.其他ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(719, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 新增網址ToolStripMenuItem
            // 
            this.新增網址ToolStripMenuItem.Name = "新增網址ToolStripMenuItem";
            this.新增網址ToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.新增網址ToolStripMenuItem.Text = "新增網址";
            this.新增網址ToolStripMenuItem.Click += new System.EventHandler(this.新增網址ToolStripMenuItem_Click);
            // 
            // 其他ToolStripMenuItem
            // 
            this.其他ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.設定ToolStripMenuItem1,
            this.插件管理ToolStripMenuItem1});
            this.其他ToolStripMenuItem.Name = "其他ToolStripMenuItem";
            this.其他ToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.其他ToolStripMenuItem.Text = "其他";
            // 
            // 設定ToolStripMenuItem1
            // 
            this.設定ToolStripMenuItem1.Name = "設定ToolStripMenuItem1";
            this.設定ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.設定ToolStripMenuItem1.Text = "設定";
            this.設定ToolStripMenuItem1.Click += new System.EventHandler(this.設定ToolStripMenuItem1_Click);
            // 
            // 插件管理ToolStripMenuItem1
            // 
            this.插件管理ToolStripMenuItem1.Name = "插件管理ToolStripMenuItem1";
            this.插件管理ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.插件管理ToolStripMenuItem1.Text = "插件管理";
            this.插件管理ToolStripMenuItem1.Click += new System.EventHandler(this.插件管理ToolStripMenuItem1_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.開啟檔案ToolStripMenuItem,
            this.開啟檔案所在位置ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(185, 70);
            // 
            // 開啟檔案所在位置ToolStripMenuItem
            // 
            this.開啟檔案所在位置ToolStripMenuItem.Name = "開啟檔案所在位置ToolStripMenuItem";
            this.開啟檔案所在位置ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.開啟檔案所在位置ToolStripMenuItem.Text = "開啟檔案所在資料夾";
            this.開啟檔案所在位置ToolStripMenuItem.Click += new System.EventHandler(this.開啟檔案所在資料夾ToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lsv);
            this.splitContainer1.Panel1MinSize = 0;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(719, 381);
            this.splitContainer1.SplitterDistance = 234;
            this.splitContainer1.SplitterWidth = 9;
            this.splitContainer1.TabIndex = 3;
            // 
            // lsv
            // 
            this.lsv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Subscribe,
            this.Status,
            this.Title,
            this.Author,
            this.Progress,
            this.CurrentSection,
            this.EndSection,
            this.TotalSection});
            this.lsv.ContextMenuStrip = this.contextMenuStrip2;
            this.lsv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsv.FullRowSelect = true;
            this.lsv.Location = new System.Drawing.Point(0, 0);
            this.lsv.Name = "lsv";
            this.lsv.Size = new System.Drawing.Size(719, 234);
            this.lsv.TabIndex = 0;
            this.lsv.UseCompatibleStateImageBehavior = false;
            this.lsv.View = System.Windows.Forms.View.Details;
            this.lsv.SelectedIndexChanged += new System.EventHandler(this.lsv_SelectedIndexChanged);
            // 
            // Subscribe
            // 
            this.Subscribe.Tag = "Subscribe";
            this.Subscribe.Text = "訂閱";
            this.Subscribe.Width = 40;
            // 
            // Status
            // 
            this.Status.Tag = "Status";
            this.Status.Text = "狀態";
            this.Status.Width = 78;
            // 
            // Title
            // 
            this.Title.Tag = "Title";
            this.Title.Text = "名稱";
            this.Title.Width = 215;
            // 
            // Author
            // 
            this.Author.Tag = "Author";
            this.Author.Text = "作者";
            this.Author.Width = 125;
            // 
            // Progress
            // 
            this.Progress.Tag = "Progress";
            this.Progress.Text = "進度";
            this.Progress.Width = 70;
            // 
            // CurrentSection
            // 
            this.CurrentSection.Tag = "CurrentSection";
            this.CurrentSection.Text = "目前位置";
            this.CurrentSection.Width = 70;
            // 
            // EndSection
            // 
            this.EndSection.Tag = "EndSection";
            this.EndSection.Text = "結束位置";
            this.EndSection.Width = 70;
            // 
            // TotalSection
            // 
            this.TotalSection.Tag = "TotalSection";
            this.TotalSection.Text = "總區塊";
            this.TotalSection.Width = 70;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtBeginSection, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtTitle, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtEndSection, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.BtnBrowseDir, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.cbSaveDir, 1, 3);
            this.tableLayoutPanel1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(582, 128);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("新細明體", 10F);
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "名稱";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("新細明體", 10F);
            this.label2.Location = new System.Drawing.Point(3, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 30);
            this.label2.TabIndex = 0;
            this.label2.Text = "起始位置";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtBeginSection
            // 
            this.txtBeginSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBeginSection.Location = new System.Drawing.Point(105, 35);
            this.txtBeginSection.Margin = new System.Windows.Forms.Padding(5);
            this.txtBeginSection.Name = "txtBeginSection";
            this.txtBeginSection.Size = new System.Drawing.Size(372, 22);
            this.txtBeginSection.TabIndex = 1;
            this.txtBeginSection.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateTaskinfo_KeyUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("新細明體", 10F);
            this.label3.Location = new System.Drawing.Point(3, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 30);
            this.label3.TabIndex = 0;
            this.label3.Text = "結束位置";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtTitle
            // 
            this.txtTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTitle.Location = new System.Drawing.Point(105, 5);
            this.txtTitle.Margin = new System.Windows.Forms.Padding(5);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(372, 22);
            this.txtTitle.TabIndex = 1;
            this.txtTitle.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateTaskinfo_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Font = new System.Drawing.Font("新細明體", 10F);
            this.label4.Location = new System.Drawing.Point(3, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 38);
            this.label4.TabIndex = 0;
            this.label4.Text = "儲存位置";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtEndSection
            // 
            this.txtEndSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEndSection.Location = new System.Drawing.Point(105, 65);
            this.txtEndSection.Margin = new System.Windows.Forms.Padding(5);
            this.txtEndSection.Name = "txtEndSection";
            this.txtEndSection.Size = new System.Drawing.Size(372, 22);
            this.txtEndSection.TabIndex = 1;
            this.txtEndSection.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UpdateTaskinfo_KeyUp);
            // 
            // BtnBrowseDir
            // 
            this.BtnBrowseDir.Location = new System.Drawing.Point(485, 93);
            this.BtnBrowseDir.Name = "BtnBrowseDir";
            this.BtnBrowseDir.Size = new System.Drawing.Size(75, 23);
            this.BtnBrowseDir.TabIndex = 2;
            this.BtnBrowseDir.Text = "選擇";
            this.BtnBrowseDir.UseVisualStyleBackColor = true;
            this.BtnBrowseDir.Click += new System.EventHandler(this.BtnBrowseDir_Click);
            // 
            // cbSaveDir
            // 
            this.cbSaveDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbSaveDir.FormattingEnabled = true;
            this.cbSaveDir.Location = new System.Drawing.Point(105, 97);
            this.cbSaveDir.Margin = new System.Windows.Forms.Padding(5, 7, 5, 5);
            this.cbSaveDir.Name = "cbSaveDir";
            this.cbSaveDir.Size = new System.Drawing.Size(372, 20);
            this.cbSaveDir.TabIndex = 3;
            // 
            // 開啟檔案ToolStripMenuItem
            // 
            this.開啟檔案ToolStripMenuItem.Name = "開啟檔案ToolStripMenuItem";
            this.開啟檔案ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.開啟檔案ToolStripMenuItem.Text = "開啟檔案";
            this.開啟檔案ToolStripMenuItem.Click += new System.EventHandler(this.開啟檔案ToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 430);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lsv;
        private System.Windows.Forms.ColumnHeader Title;
        private System.Windows.Forms.ColumnHeader Progress;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ColumnHeader TotalSection;
        private SplitContainerEx splitContainer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtBeginSection;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtEndSection;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button BtnBrowseDir;
        private System.Windows.Forms.ComboBox cbSaveDir;
        private System.Windows.Forms.ColumnHeader Author;
        private System.Windows.Forms.ColumnHeader EndSection;
        private System.Windows.Forms.ColumnHeader CurrentSection;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 退出程式ToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader Subscribe;
        private System.Windows.Forms.Timer SubscribeTimer;
        private System.Windows.Forms.ToolStripButton toolStripSubscription;
        private System.Windows.Forms.ToolStripButton toolStripStart;
        private System.Windows.Forms.ToolStripButton toolStripStop;
        private System.Windows.Forms.ToolStripButton toolStripAnalysis;
        private System.Windows.Forms.ToolStripButton toolStripDel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 新增網址ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 其他ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 設定ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 插件管理ToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 開啟檔案所在位置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 開啟檔案ToolStripMenuItem;

    }
}

