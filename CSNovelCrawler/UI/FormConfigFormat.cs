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
    public partial class FormConfigFormat : Form
    {
        public FormConfigFormat()
        {
            InitializeComponent();
        }
        TaskInfo taskInfo = new TaskInfo
        {
            Author = "我是作者",
            Title = "我是書名"
        };

        private void FormConfigFormat_Load(object sender, EventArgs e)
        {
            ReloadFormat();
        }
        private void ReloadFormat()
        {
            listView1.Items.Clear();
            foreach (var f in CoreManager.ConfigManager.Settings.DefaultFormatFileName)
            {

                ListViewItem lvi = new ListViewItem();
                lvi.Text = f.Name;
                lvi.SubItems.Add(f.Format);
                lvi.Tag=f.Key;
                listView1.Items.Add(lvi);
            }
            foreach (var f in CoreManager.ConfigManager.Settings.CustomFormatFileName)
            {

                ListViewItem lvi = new ListViewItem();
                lvi.Text = f.Name;
                lvi.Tag = f.Key;
                lvi.SubItems.Add(f.Format);
                listView1.Items.Add(lvi);
            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var lv = (ListView)sender;
            if (lv.SelectedItems.Count > 0)
            {
                var lvi = lv.SelectedItems[0];
                FormatFileName FFN = new FormatFileName();
                textBox1.Text = FFN.OutputFormat(taskInfo, lvi.SubItems[1].Text);
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var AddFormat = new FormAddFormat();
            AddFormat.ShowDialog();
            AddFormat.Dispose();
            
            ReloadFormat();
        }

        private void btn_remove_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count > 0)
            {
                var lvi = listView1.SelectedItems[0];
                CoreManager.ConfigManager.Settings.CustomFormatFileName.RemoveAll(f => f.Key == ((int)lvi.Tag));
                lvi.Remove();
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        

     
    }
}
