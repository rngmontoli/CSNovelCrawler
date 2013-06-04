using CSNovelCrawler.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSNovelCrawler.UI
{
    public partial class FormConfig : Form
    {
        
        public FormConfig()
        {
            InitializeComponent();
        }
        private void FormConfig_Load(object sender, EventArgs e)
        {
            CoreManager.ConfigManager.LoadSettings();
            txtSavePath.Text = CoreManager.ConfigManager.Settings.DefaultSaveFolder;

            chbSysTray.Checked = CoreManager.ConfigManager.Settings.HideSysTray;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //选择文件夹
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.Description = "請設定預設下載資料夾：";
            fbd.SelectedPath = txtSavePath.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
                txtSavePath.Text = fbd.SelectedPath;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            CoreManager.ConfigManager.Settings.DefaultSaveFolder = txtSavePath.Text;
            CoreManager.ConfigManager.Settings.HideSysTray = chbSysTray.Checked;
            CoreManager.ConfigManager.SaveSettings();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
