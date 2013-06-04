using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSNovelCrawler.Core;

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
                //var attributes = members[0].GetCustomAttributes(typeof(PluginInformationAttribute), false);
                //var description = ((PluginInformationAttribute)attributes[0]);
                object[] types = plugin.GetType().GetCustomAttributes(typeof(PluginInformationAttribute), true);
                if (types.Length > 0)
                {
                    var attrib = (PluginInformationAttribute)types[0];
                    var lvi = new ListViewItem(new string[]
                        {
                            attrib.FriendlyName,
                            attrib.Version.ToString(),
                            attrib.Author,
                            attrib.Describe,
                            attrib.SupportUrl,
                            attrib.Name
                        });
                    lvi.Tag = plugin;
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
                if (plugin.Feature != null && plugin.Feature.ContainsKey("ConfigForm"))
                {
                    btnConfig.Enabled = true;
                }
              

            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {

            var lvi = listView1.SelectedItems[0];
            var plugin = (IPlugin)lvi.Tag;
            var method = (Delegate)plugin.Feature["ConfigForm"];
            this.Invoke(method);
            //保存设置
            CoreManager.PluginManager.SaveConfiguration(plugin);
        }
    }
}
