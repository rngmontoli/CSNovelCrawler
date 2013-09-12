using System;
using System.Windows.Forms;
using CSNovelCrawler.Core;
using CSNovelCrawler.Interface;

namespace CSNovelCrawler.UI
{
    public partial class FormPlugins : Form
    {
        public FormPlugins()
        {
            InitializeComponent();
        }

        private void FormPlugins_Load(object sender, EventArgs e)
        {
            foreach (var plugin in CoreManager.PluginManager.Plugins)
            {
            
                object[] types = plugin.GetType().GetCustomAttributes(typeof(PluginInformationAttribute), true);
                if (types.Length > 0)
                {
                    var attrib = (PluginInformationAttribute)types[0];
                    var lvi = new ListViewItem(new[]
                        {
                            attrib.FriendlyName,
                            attrib.Version.ToString()
                           // attrib.Author,
                            //attrib.Describe,
                            //attrib.SupportUrl,
                           // attrib.Name
                        }) {Tag = plugin};
                    listView1.Items.Add(lvi);
                }
            }
            
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView lv = (ListView)sender;
            btnConfig.Enabled = false;
            if (lv.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lv.SelectedItems[0];
                var plugin = (IPlugin)lvi.Tag;
                if (plugin.Extensions != null && plugin.Extensions.ContainsKey("ConfigForm"))
                {
                    btnConfig.Enabled = true;
                }
              

            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {

            var lvi = listView1.SelectedItems[0];
            var plugin = (IPlugin)lvi.Tag;
            var method = (Delegate)plugin.Extensions["ConfigForm"];
            Invoke(method);
            //儲存設定
            CoreManager.PluginManager.SaveConfiguration(plugin);
        }
    }
}
