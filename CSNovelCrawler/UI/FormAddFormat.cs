using CSNovelCrawler.Class;
using CSNovelCrawler.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSNovelCrawler.UI
{
    public partial class FormAddFormat : Form
    {
        public FormAddFormat()
        {
            InitializeComponent();
        }
        TaskInfo taskInfo = new TaskInfo
        {
            Author = "我是作者",
            Title = "我是書名"
        };
        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBox1.Text))
            {
                MessageBox.Show("格式不能為空白");
                return;
            }
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("名稱不能為空白");
                return;
            }
            int count = CoreManager.ConfigManager.Settings.CustomFormatFileName.Count+4;
            CoreManager.ConfigManager.Settings.CustomFormatFileName.Add(new CSNovelCrawler.Core.CustomSettings.FormatFileName_Class { Key=count, Name = textBox1.Text, Format = richTextBox1.Text });
            this.Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            FormatFileName FFN = new FormatFileName();
            textBox2.Text = FFN.OutputFormat(taskInfo, richTextBox1.Text);
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
