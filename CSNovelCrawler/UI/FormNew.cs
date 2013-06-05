using CSNovelCrawler.Class;
using CSNovelCrawler.Core;
using System;
using System.Windows.Forms;
using CSNovelCrawler.Interface;

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
            foreach (var url in richTextBox1.Lines)
            {
                if (!string.IsNullOrEmpty(url.Trim()))
                {
                    IPlugin plugin = CoreManager.PluginManager.GetPlugin(url);
                    TaskInfo taskInfo = CoreManager.TaskManager.AddTask(plugin, url);
                    CoreManager.TaskManager.AnalysisTask(taskInfo);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
