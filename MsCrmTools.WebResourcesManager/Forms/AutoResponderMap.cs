using MscrmTools.WebresourcesManager.AutoResponder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MscrmTools.WebresourcesManager.Forms
{
    public partial class AutoResponderMap : Form
    {
        public AutoResponderMap(List<ProxyResource> proxyResources)
        {
            InitializeComponent();
            foreach (var p in proxyResources) {
                listView1.Items.Add(new ListViewItem(new string[] {p.Server, p.UrlPath, p.FilePath }));
            }
        }
    }
}
