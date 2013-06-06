namespace CSNovelCrawler.UI
{
    partial class FormConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtSavePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chbSysTray = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSubTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chbWatchClipboard = new System.Windows.Forms.CheckBox();
            this.chbLogging = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtSavePath
            // 
            this.txtSavePath.Location = new System.Drawing.Point(12, 34);
            this.txtSavePath.Name = "txtSavePath";
            this.txtSavePath.ReadOnly = true;
            this.txtSavePath.Size = new System.Drawing.Size(242, 22);
            this.txtSavePath.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "預設下載路徑";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(261, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "修改";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(183, 187);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "確定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(264, 187);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chbSysTray
            // 
            this.chbSysTray.AutoSize = true;
            this.chbSysTray.Location = new System.Drawing.Point(12, 116);
            this.chbSysTray.Name = "chbSysTray";
            this.chbSysTray.Size = new System.Drawing.Size(144, 16);
            this.chbSysTray.TabIndex = 6;
            this.chbSysTray.Text = "關閉時，縮小到系統列";
            this.chbSysTray.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "訂閱更新週期";
            // 
            // txtSubTime
            // 
            this.txtSubTime.Location = new System.Drawing.Point(93, 62);
            this.txtSubTime.Name = "txtSubTime";
            this.txtSubTime.Size = new System.Drawing.Size(45, 22);
            this.txtSubTime.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(146, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "分鐘";
            // 
            // chbWatchClipboard
            // 
            this.chbWatchClipboard.AutoSize = true;
            this.chbWatchClipboard.Location = new System.Drawing.Point(12, 138);
            this.chbWatchClipboard.Name = "chbWatchClipboard";
            this.chbWatchClipboard.Size = new System.Drawing.Size(84, 16);
            this.chbWatchClipboard.TabIndex = 6;
            this.chbWatchClipboard.Text = "監視剪貼簿";
            this.chbWatchClipboard.UseVisualStyleBackColor = true;
            // 
            // chbLogging
            // 
            this.chbLogging.AutoSize = true;
            this.chbLogging.Location = new System.Drawing.Point(12, 160);
            this.chbLogging.Name = "chbLogging";
            this.chbLogging.Size = new System.Drawing.Size(96, 16);
            this.chbLogging.TabIndex = 6;
            this.chbLogging.Text = "記錄錯誤日誌";
            this.chbLogging.UseVisualStyleBackColor = true;
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 222);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSubTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chbLogging);
            this.Controls.Add(this.chbWatchClipboard);
            this.Controls.Add(this.chbSysTray);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSavePath);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfig";
            this.Text = "設定";
            this.Load += new System.EventHandler(this.FormConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSavePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chbSysTray;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSubTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chbWatchClipboard;
        private System.Windows.Forms.CheckBox chbLogging;
    }
}