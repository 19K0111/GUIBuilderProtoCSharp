using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GUIBuilderProtoCSharp {
    public partial class Form5 : Form {
        string hostName = "19k0111.gui-builder.com";
        public Form5() {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e) {
            InitializeAsync();
        }

        public async void InitializeAsync() {
            await webView21.EnsureCoreWebView2Async(null);
            // ../bin/debug/内の指定したフォルダに仮想ドメインを割り当てる
            webView21.CoreWebView2.SetVirtualHostNameToFolderMapping(hostName, "block-editor", Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);
            webView21.CoreWebView2.Navigate($"https://{hostName}/index.html");
            webView21.CoreWebView2.WebMessageReceived += webView21_WebMessageReceived;
        }

        private void webView21_WebMessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e) {
            Form1.f4.richTextBox1.Enabled = false;
            Form1.f4.richTextBox1.Text = e.TryGetWebMessageAsString();
            Form1.f4.richTextBox1_TextChanged(sender, null);
        }

        private void Form5_FormClosing(object sender, FormClosingEventArgs e) {
            switch (MessageBox.Show("保存していない変更は失われますが、よろしいですか？", Form1.BLOCK_EDITOR, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)) {
                case DialogResult.Yes:
                    break;
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    break;
            }
        }
    }
}
