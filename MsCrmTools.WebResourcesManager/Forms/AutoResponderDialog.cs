using Microsoft.Win32;
using MscrmTools.WebresourcesManager.AppCode;
using MscrmTools.WebresourcesManager.AutoResponder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Titanium.Web.Proxy.Helpers;
using WeifenLuo.WinFormsUI.Docking;

namespace MscrmTools.WebresourcesManager.Forms
{
    public partial class AutoResponderDialog : DockContent
    {
        private static readonly ProxyTestController controller = new ProxyTestController();
        private ConsoleWriter consoleWriter = null;
        private List<ProxyResource> proxyResources = new List<ProxyResource>();
        private MyPluginControl myPluginControl;
        private Settings settings = null;
        public AutoResponderDialog(MyPluginControl myPluginControl)
        {
            this.myPluginControl = myPluginControl;
            InitializeComponent();
            Console.SetOut(consoleWriter = new ConsoleWriter(this.listView1));

        }
        private void AutoResponderDialog_Load(object sender, EventArgs e)
        {
            if (myPluginControl.ConnectionDetail != null)
                this.txtFilterUrlPath.Text = myPluginControl.ConnectionDetail.WebApplicationUrl;

            LoadProxySettings();
        }
        private void LoadProxySettings()
        {
            RegistryKey registry = Registry.CurrentUser.OpenSubKey
              ("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
            var obj = registry.GetValue("ProxyEnable");
            if (obj != null)
                lblProxyEnabled.Text = obj.ToString() == "1" ? "True" : "False";
            obj = registry.GetValue("ProxyServer");
            if (obj != null)
                lblIpPort.Text = obj.ToString();
        }
        private void UpdateProxyResources()
        {
            proxyResources.Clear();
            string urlPath = txtFilterUrlPath.Text;
            UpdateProxyResourcesRecusrsive(urlPath, this.treeView1.Nodes);
            controller.UpdateProxyResources(proxyResources);
        }
        private void UpdateProxyResourcesRecusrsive(string urlPath, TreeNodeCollection node)
        {
            foreach (TreeNode tn in node)
            {
                if (tn.Checked && tn.Tag != null)
                    proxyResources.Add(new ProxyResource()
                    {
                        Server = urlPath,
                        FilePath = (string)tn.Tag,
                        UrlPath = @"/WebResources" + tn.FullPath.Substring(tn.FullPath.IndexOf(@"\")).Replace(@"\", "/")
                    });
                else if (tn.Nodes.Count > 0)
                    UpdateProxyResourcesRecusrsive(urlPath, tn.Nodes);
            }

        }               
        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
            treeView.ExpandAll();
        }
        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name) { Checked = true} ;
            
            foreach (var directory in directoryInfo.GetDirectories())
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            foreach (var file in directoryInfo.GetFiles())
            {
                directoryNode.Nodes.Add(new TreeNode(file.Name) { Checked = true,Tag = file.FullName });

            }
            return directoryNode;
        }
        public void UpdateConnection(Settings settings)
        {
            if (myPluginControl.ConnectionDetail != null)
                txtFilterUrlPath.Text = myPluginControl.ConnectionDetail.WebApplicationUrl;
            this.settings = settings;
        }
        public void StopAutoResponder()
        {
            try
            {
                controller.Stop();
            }
            catch (Exception ex)
            {
            }
        }
        private void cbEnableAutoResponder_CheckedChanged(object sender, EventArgs e)
        {
            if (txtFilterUrlPath.Text.Length == 0 || txtFolderPath.Text.Length == 0)
            {
                MessageBox.Show("Please provide server url and webresource folder path", "Error");
                cbEnableAutoResponder.Checked = false;
                return;
            }
            if (!settings.SaveOnDisk)
            {
                DialogResult dr = MessageBox.Show("Save contents on local file during edit should be enable. Do you want to enable it?", "Save Content On Local File", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                    settings.SaveOnDisk = true;
            }
            if (cbEnableAutoResponder.Checked)
            {
                if (RunTime.IsWindows)
                {
                    // fix console hang due to QuickEdit mode
                    ConsoleHelper.DisableQuickEditMode();
                }


                // Start proxy controller
                controller.StartProxy();
            }
            else
            {
                StopAutoResponder();
            }
            LoadProxySettings();
        }
        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog folderBrowser = new System.Windows.Forms.OpenFileDialog();
            // Set validate names and check file exists to false otherwise windows will
            // not let you select "Folder Selection."
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            // Always default to Folder Selection.
            folderBrowser.FileName = "Folder Selection.";
            if (settings?.LastFolderUsed?.Length > 0)
                folderBrowser.InitialDirectory = settings.LastFolderUsed;
            if (folderBrowser.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowser.FileName))
            {
                string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
                txtFolderPath.Text = folderPath;
                ListDirectory(this.treeView1, folderPath);
                UpdateProxyResources();
            }

            //using (var fbd = new FolderBrowserDialog())
            //{
            //    DialogResult result = fbd.ShowDialog();

            //    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            //    {
            //        ListDirectory(this.treeView1, fbd.SelectedPath);
            //    }
            //    UpdateProxyResources();
            //}
        }
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {            
            foreach (TreeNode tn in e.Node.Nodes)
            {
                tn.Checked = e.Node.Checked;
            }
            UpdateProxyResources();
        }        
        private void btnMaps_Click(object sender, EventArgs e)
        {
            AutoResponderMap autoResponderMap = new AutoResponderMap(proxyResources);
            autoResponderMap.ShowDialog();
        }
        private void cbWriteActivity_CheckedChanged(object sender, EventArgs e) => consoleWriter.isWritingEnabled = cbWriteActivity.Checked;
        private void cbServedWebResources_CheckedChanged(object sender, EventArgs e) => consoleWriter.isWebResourceActivityEnabled = cbServedWebResources.Checked;
        private void btnClear_Click(object sender, EventArgs e) => listView1.Items.Clear();

        
        
    }
}
