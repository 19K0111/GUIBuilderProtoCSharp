using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Text.Json;

namespace GUIBuilderProtoCSharp {
    public partial class NewProjectDialog : Form {
        public NewProjectDialog() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {

            using (CommonOpenFileDialog cofd = new CommonOpenFileDialog() {
                Title = "フォルダを選択",
                IsFolderPicker = true,
            }) {
                if (cofd.ShowDialog() == CommonFileDialogResult.Ok) {
                    Form1.newProjectDialog.textBox2.Text = cofd.FileName;
                }
                if (Directory.Exists(textBox2.Text + "\\" + textBox1.Text)) {
                    label3.Text = "同じ名前のプロジェクトが存在します";
                    label3.Visible = true;
                } else {
                    label3.Visible = false;
                }
                buttonEnabledChange();
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            Form1.f1.Init();
            Form1.workingDirectory = textBox2.Text + "\\" + textBox1.Text;
            Directory.CreateDirectory(Form1.workingDirectory);
            Directory.CreateDirectory($"{Form1.workingDirectory}\\Resources");
            StreamWriter sw = new StreamWriter(Form1.workingDirectory + "\\" + textBox1.Text + GUIBuilderExtensions.Project);
            Form1.pj = new ProjectJson("Form");
            sw.Write(JsonSerializer.Serialize(Form1.pj, ProjectJson.options));
            // sw.Write("{\"designer\": [\"Form.dsn\"],\"src\": [\"Form.blk\"]}\r\n");
            sw.Close();
            sw = new StreamWriter(Form1.workingDirectory + "\\" + Form1.pj.Name[0] + GUIBuilderExtensions.Design);
            DesignJson dj = new DesignJson(Form1.pj);
            sw.Write(JsonSerializer.Serialize(dj, ProjectJson.options));
            sw.Close();
            sw = new StreamWriter(Form1.workingDirectory + "\\" + Form1.pj.Name[0] + GUIBuilderExtensions.BlockCode);
            sw.Close();
            Close();
            textBox1.Text = "";
            textBox2.Text = "";
            button2.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            if (Directory.Exists(textBox2.Text + "\\" + textBox1.Text)) {
                label3.Text = "同じ名前のプロジェクトが存在します";
                label3.Visible = true;
            } else {
                // ファイル名に使えない正規表現 https://dobon.net/vb/dotnet/file/invalidpathchars.html
                System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(
                    "[\\x00-\\x1f<>:\"/\\\\|?*]" +
                    "|^(CON|PRN|AUX|NUL|COM[0-9]|LPT[0-9]|CLOCK\\$)(\\.|$)" +
                    "|[\\. ]$",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                label3.Text = "プロジェクト名が無効です";
                label3.Visible = r.IsMatch(textBox1.Text) || textBox1.TextLength == 0;
                buttonEnabledChange();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e) {
            // テキストボックスに入力できるようにするなら、正規表現を使ってパスが正しいかを調べる
            System.Text.RegularExpressions.Regex r1 = new System.Text.RegularExpressions.Regex(
                @"^([a-zA-Z]:\\[\w\\\.]*)$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex r2 = new System.Text.RegularExpressions.Regex(
                "[\\x00-\\x1f<>:\"/\\|?*]" +
                "|^(CON|PRN|AUX|NUL|COM[0-9]|LPT[0-9]|CLOCK\\$)(\\.|$)" +
                "|[\\. ]$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //label4.Visible = !r1.IsMatch(textBox2.Text) || r2.IsMatch(textBox2.Text);
            buttonEnabledChange();
        }

        private void buttonEnabledChange() {
            if (!label3.Visible && !label4.Visible && textBox1.TextLength != 0 && textBox2.TextLength != 0) {
                button2.Enabled = true;
            } else {
                button2.Enabled = false;
            }
        }
    }
}
