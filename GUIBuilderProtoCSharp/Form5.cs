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

        public async Task InitializeAsync() {
            await webView21.EnsureCoreWebView2Async(null);
            // ../bin/debug/内の指定したフォルダに仮想ドメインを割り当てる
            webView21.CoreWebView2.SetVirtualHostNameToFolderMapping(hostName, "block-editor", Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);
            webView21.CoreWebView2.Navigate($"https://{hostName}/index.html");
            webView21.CoreWebView2.WebMessageReceived += webView21_WebMessageReceived;
            await Task.Delay(500);
            await LoadBlockCode();
        }

        public async Task LoadBlockCode() {
            string code = "";
            string fileName = Form1.pj.Name[0] + GUIBuilderExtensions.BlockCode;
            using (StreamReader sr = new StreamReader(Form1.workingDirectory + "\\" + fileName)) {
                code = sr.ReadToEnd();
                code = code.Replace("\n", "");
                code = code.Replace("\"", "\\\"");
                code = code.Replace("\'", "\\\'");
            }
            await Task.Delay(500);
            //await webView21.ExecuteScriptAsync($"setTimeout(console.log, 2000, \"Loaded...\");document.getElementById(\"fileName\").innerHTML = \"{fileName}\";document.querySelector(\".text-code\").innerHTML = \"{code}\";");
            //await webView21.ExecuteScriptAsync($"setTimeout(window_load, 5000, \"{Form1.pj.Name[0] + GUIBuilderExtensions.BlockCode}\", \"{code})\")");
            await webView21.ExecuteScriptAsync($"window_load(\"{Form1.pj.Name[0] + GUIBuilderExtensions.BlockCode}\", \'{code}\')");
        }

        public async Task LoadBlockCode(string code) {
            await Task.Delay(500);
            await webView21.ExecuteScriptAsync($"window_load(\"{Form1.pj.Name[0] + GUIBuilderExtensions.BlockCode}\", \'{code}\')");

        }

        private async void webView21_WebMessageReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e) {
            string code = e.TryGetWebMessageAsString();
            Form1.f4.richTextBox1.Enabled = false;
            Form1.f4.編集を有効化EToolStripMenuItem.Checked = false;
            Form1.f4.richTextBox1.Text = code;
            code = code.Replace("\n", "\\n");
            code = code.Replace("\"", "\\\"");
            code = code.Replace("\'", "\\\'");
            await Form1.f4.webView21.ExecuteScriptAsync($"setValue(\'{code}\');");
            // Form1.f4.richTextBox1_TextChanged(sender, null);
        }

        private void Form5_FormClosing(object sender, FormClosingEventArgs e) {
            switch (e.CloseReason) {
                case CloseReason.UserClosing:
                    switch (MessageBox.Show("保存していない変更は失われますが、よろしいですか？", Form1.BLOCK_EDITOR, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)) {
                        case DialogResult.Yes:
                            break;
                        case DialogResult.No:
                            e.Cancel = true;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
