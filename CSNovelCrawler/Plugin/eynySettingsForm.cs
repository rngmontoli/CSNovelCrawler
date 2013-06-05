using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CSNovelCrawler.Plugin
{
    public partial class eynySettingsForm : Form
    {
        private Dictionary<string, string> Configuration;
        public eynySettingsForm(DictionaryExtension<string, string> Configuration)
        {
            this.Configuration = Configuration;
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            EncryptAES.EncryptAES AES = new EncryptAES.EncryptAES();

            string PostData = "jNLWAPIFsJ0iWz7D00C09Fy1nAmQepY1y5cHlwqy0+75fQ1bfPELaZdYi/OKhAghQA0TiEVPd0wsFNCzNcVQNpqObZuZyl3DE18XX+Gwn0VJD4OQvxXfjIiLdhZYzqCuQdxFn2EI72/TmzTtaSLVChEVFd/A6wmBYvM1InsnchbSSPcrHulXtQLt/dpLyQ5i";
            if (!string.IsNullOrEmpty(txtPassword.Text.Trim()) 
                && 
                !string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                PostData = string.Format(AES.DecryptAES256(PostData), txtUserName.Text.Trim(), txtPassword.Text.Trim().Replace("+","%2B"));
                PostData = AES.EncryptAES256(PostData);
            }

            Configuration["PostData"] = PostData;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
