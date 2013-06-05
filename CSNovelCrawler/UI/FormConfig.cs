using CSNovelCrawler.Core;
using System;
using System.Windows.Forms;
using CSNovelCrawler.Properties;

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
            var fbd = new FolderBrowserDialog
                {
                    ShowNewFolderButton = true,
                    Description = Resources.FormConfig_button1_Click_,
                    SelectedPath = txtSavePath.Text
                };
            if (fbd.ShowDialog() == DialogResult.OK)
                txtSavePath.Text = fbd.SelectedPath;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            CoreManager.ConfigManager.Settings.DefaultSaveFolder = txtSavePath.Text;
            CoreManager.ConfigManager.Settings.HideSysTray = chbSysTray.Checked;
            CoreManager.ConfigManager.SaveSettings();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
