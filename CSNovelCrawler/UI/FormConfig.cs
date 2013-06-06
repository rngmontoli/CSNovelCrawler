using System.Globalization;
using CSNovelCrawler.Class;
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
            chbWatchClipboard.Checked = CoreManager.ConfigManager.Settings.WatchClipboard;
            chbLogging.Checked = CoreManager.ConfigManager.Settings.Logging;
            chbSysTray.Checked = CoreManager.ConfigManager.Settings.HideSysTray;
            txtSubTime.Text = CoreManager.ConfigManager.Settings.SubscribeTime.ToString(CultureInfo.InvariantCulture);
        }

        private void button1_Click(object sender, EventArgs e)
        {

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
            CoreManager.ConfigManager.Settings.WatchClipboard = chbWatchClipboard.Checked;
            CoreManager.ConfigManager.Settings.Logging = chbLogging.Checked;
            CoreManager.ConfigManager.Settings.SubscribeTime = CommonTools.TryParse(txtSubTime.Text,10);
            CoreManager.ConfigManager.SaveSettings();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


        
    }
}
