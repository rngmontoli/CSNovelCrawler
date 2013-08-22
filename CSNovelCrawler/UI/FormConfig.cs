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
            rbUnicode.Checked = CoreManager.ConfigManager.Settings.TextEncoding == rbUnicode.Tag.ToString();
            rbUTF8.Checked = CoreManager.ConfigManager.Settings.TextEncoding == rbUTF8.Tag.ToString();
            cb_Format.Items.AddRange(CoreManager.ConfigManager.Settings.DefaultFormatFileName.ToArray());
            cb_Format.Items.AddRange(CoreManager.ConfigManager.Settings.CustomFormatFileName.ToArray());
            cb_Format.DisplayMember = "Name";
            cb_Format.ValueMember = "Format";
            cb_Format.SelectedIndex=cb_Format.FindString(CoreManager.ConfigManager.Settings.SelectFormatName);


                
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
            CoreManager.ConfigManager.Settings.SubscribeTime = CommonTools.TryParse(txtSubTime.Text, 10) < 1 ? 10 : CommonTools.TryParse(txtSubTime.Text, 10);
            CoreManager.ConfigManager.Settings.TextEncoding = rbUnicode.Checked
                                                                  ? rbUnicode.Tag.ToString()
                                                                  : rbUTF8.Checked ? rbUTF8.Tag.ToString():"utf-16";
            CoreManager.ConfigManager.Settings.SelectFormatName = ((CSNovelCrawler.Core.CustomSettings.FormatFileName_Class)(cb_Format.SelectedItem)).Name;
            CoreManager.ConfigManager.Settings.SelectFormat = ((CSNovelCrawler.Core.CustomSettings.FormatFileName_Class)(cb_Format.SelectedItem)).Format;


            CoreManager.ConfigManager.SaveSettings();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_EditFormat_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();
            var ConfigFormat = new FormConfigFormat();
            dr=ConfigFormat.ShowDialog();

            //if (dr == DialogResult.OK)
            //    MessageBox.Show("User clicked OK button");
            //else if (dr == DialogResult.Cancel)
            //    MessageBox.Show("User clicked Cancel button");

            ConfigFormat.Dispose();
        }


        
    }
}
