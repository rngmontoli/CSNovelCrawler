using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CSNovelCrawler.Class;


namespace CSNovelCrawler.Plugin
{
    public partial class EynySettingsForm : Form
    {
        private Dictionary<string, string> _configuration;
        public EynySettingsForm(DictionaryExtension<string, string> configuration)
        {
            _configuration = configuration;
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            EncryptAES.EncryptAES aes = new EncryptAES.EncryptAES();

            string postData = "jNLWAPIFsJ0iWz7D00C09Fy1nAmQepY1y5cHlwqy0+75fQ1bfPELaZdYi/OKhAghQA0TiEVPd0wsFNCzNcVQNpqObZuZyl3DE18XX+Gwn0VJD4OQvxXfjIiLdhZYzqCuQdxFn2EI72/TmzTtaSLVChEVFd/A6wmBYvM1InsnchbSSPcrHulXtQLt/dpLyQ5i";
            if (!string.IsNullOrEmpty(txtPassword.Text.Trim()) 
                && 
                !string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                postData = string.Format(aes.DecryptAES256(postData), txtUserName.Text.Trim(), txtPassword.Text.Trim().Replace("+","%2B"));
                postData = aes.EncryptAES256(postData);
            }

            _configuration["PostData"] = postData;

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
