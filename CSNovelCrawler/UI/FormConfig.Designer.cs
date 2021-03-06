﻿namespace CSNovelCrawler.UI
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
            this.label4 = new System.Windows.Forms.Label();
            this.rbUTF8 = new System.Windows.Forms.RadioButton();
            this.rbUnicode = new System.Windows.Forms.RadioButton();
            this.cb_Format = new System.Windows.Forms.ComboBox();
            this.btn_EditFormat = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
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
            this.btnConfirm.Location = new System.Drawing.Point(197, 328);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "確定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(278, 328);
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
            this.chbSysTray.Location = new System.Drawing.Point(12, 192);
            this.chbSysTray.Name = "chbSysTray";
            this.chbSysTray.Size = new System.Drawing.Size(144, 16);
            this.chbSysTray.TabIndex = 6;
            this.chbSysTray.Text = "最小化時縮小到系統列";
            this.chbSysTray.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "訂閱更新週期";
            // 
            // txtSubTime
            // 
            this.txtSubTime.Location = new System.Drawing.Point(93, 151);
            this.txtSubTime.Name = "txtSubTime";
            this.txtSubTime.Size = new System.Drawing.Size(45, 22);
            this.txtSubTime.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(146, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "分鐘";
            // 
            // chbWatchClipboard
            // 
            this.chbWatchClipboard.AutoSize = true;
            this.chbWatchClipboard.Location = new System.Drawing.Point(12, 214);
            this.chbWatchClipboard.Name = "chbWatchClipboard";
            this.chbWatchClipboard.Size = new System.Drawing.Size(84, 16);
            this.chbWatchClipboard.TabIndex = 6;
            this.chbWatchClipboard.Text = "監視剪貼簿";
            this.chbWatchClipboard.UseVisualStyleBackColor = true;
            // 
            // chbLogging
            // 
            this.chbLogging.AutoSize = true;
            this.chbLogging.Location = new System.Drawing.Point(12, 236);
            this.chbLogging.Name = "chbLogging";
            this.chbLogging.Size = new System.Drawing.Size(96, 16);
            this.chbLogging.TabIndex = 6;
            this.chbLogging.Text = "記錄錯誤日誌";
            this.chbLogging.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "預設編碼方式";
            // 
            // rbUTF8
            // 
            this.rbUTF8.AutoSize = true;
            this.rbUTF8.Location = new System.Drawing.Point(94, 110);
            this.rbUTF8.Name = "rbUTF8";
            this.rbUTF8.Size = new System.Drawing.Size(54, 16);
            this.rbUTF8.TabIndex = 11;
            this.rbUTF8.TabStop = true;
            this.rbUTF8.Tag = "utf-8";
            this.rbUTF8.Text = "UTF-8";
            this.rbUTF8.UseVisualStyleBackColor = true;
            // 
            // rbUnicode
            // 
            this.rbUnicode.AutoSize = true;
            this.rbUnicode.Location = new System.Drawing.Point(154, 110);
            this.rbUnicode.Name = "rbUnicode";
            this.rbUnicode.Size = new System.Drawing.Size(62, 16);
            this.rbUnicode.TabIndex = 11;
            this.rbUnicode.TabStop = true;
            this.rbUnicode.Tag = "utf-16";
            this.rbUnicode.Text = "Unicode";
            this.rbUnicode.UseVisualStyleBackColor = true;
            // 
            // cb_Format
            // 
            this.cb_Format.BackColor = System.Drawing.SystemColors.Menu;
            this.cb_Format.FormattingEnabled = true;
            this.cb_Format.Location = new System.Drawing.Point(12, 77);
            this.cb_Format.Name = "cb_Format";
            this.cb_Format.Size = new System.Drawing.Size(240, 20);
            this.cb_Format.TabIndex = 12;
            // 
            // btn_EditFormat
            // 
            this.btn_EditFormat.Location = new System.Drawing.Point(261, 77);
            this.btn_EditFormat.Name = "btn_EditFormat";
            this.btn_EditFormat.Size = new System.Drawing.Size(75, 23);
            this.btn_EditFormat.TabIndex = 13;
            this.btn_EditFormat.Text = "編輯";
            this.btn_EditFormat.UseVisualStyleBackColor = true;
            this.btn_EditFormat.Click += new System.EventHandler(this.btn_EditFormat_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "預設檔名格式";
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 363);
            this.Controls.Add(this.btn_EditFormat);
            this.Controls.Add(this.cb_Format);
            this.Controls.Add(this.rbUnicode);
            this.Controls.Add(this.rbUTF8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSubTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chbLogging);
            this.Controls.Add(this.chbWatchClipboard);
            this.Controls.Add(this.chbSysTray);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rbUTF8;
        private System.Windows.Forms.RadioButton rbUnicode;
        private System.Windows.Forms.ComboBox cb_Format;
        private System.Windows.Forms.Button btn_EditFormat;
        private System.Windows.Forms.Label label5;
    }
}