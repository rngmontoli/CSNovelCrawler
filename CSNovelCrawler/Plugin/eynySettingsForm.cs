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

            string PostData = string.Empty;
            if (!string.IsNullOrEmpty(txtPassword.Text.Trim()) 
                && 
                !string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                PostData = string.Format("formhash=3b765c67&referer=http%3A%2F%2Fwww02.eyny.com%2F&loginfield=username&username={0}&password={1}&questionid=0&answer=&cookietime=2592000", txtUserName.Text.Trim(), txtPassword.Text.Trim());
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
