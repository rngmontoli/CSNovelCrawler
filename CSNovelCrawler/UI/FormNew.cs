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
    public partial class FormNew : Form
    {
        public FormNew()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var Url in richTextBox1.Lines)
            {
                if (!string.IsNullOrEmpty(Url.Trim()))
                {
                    IPlugin Plugin = CoreManager.PluginManager.GetPlugin(Url);
                    TaskInfo Info = CoreManager.TaskManager.AddTask(Plugin, Url);
                    CoreManager.TaskManager.AnalysisTask(Info);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
