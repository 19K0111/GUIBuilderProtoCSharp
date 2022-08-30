﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GUIBuilderProtoCSharp {
    public partial class Form4 : Form {
        static string fileName = "無題";
        static string fileDetail = "";
        char pressedKey;
        public Form4() {
            InitializeComponent();
            // RichTextBoxでフォントが勝手に変わらないための処置
            richTextBox1.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e) {
            // ToolStripMenuItemのショートカットが優先される
            //if (e.KeyData == (Keys.C | Keys.Control)) {
            //    richTextBox1.Copy();
            //}
            //if (e.KeyData == (Keys.X | Keys.Control)) {
            //    richTextBox1.Cut();
            //}
            //if (e.KeyData == (Keys.V | Keys.Control)) {
            //    richTextBox1.Paste();
            //}
            //if (e.KeyData == (Keys.A | Keys.Control)) {
            //    richTextBox1.SelectAll();
            //}
            if (e.KeyData == Keys.Tab) {

            }
        }
        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e) {
            // Tabキーを半角スペースに置き換えるとRedo、Undoがうまく動作しない
            //if (e.KeyChar == '\t') {
            //    e.Handled = true;
            //    string replaceTab = "    ";
            //    int startPos = richTextBox1.SelectionStart;
            //    richTextBox1.Text = richTextBox1.Text.Insert(startPos, replaceTab);
            //    richTextBox1.SelectionStart = startPos + replaceTab.Length;
            //    // richTextBox1_KeyDown(sender, new KeyEventArgs(Keys.Tab));
            //    // richTextBox1.Text = richTextBox1.Text.Replace("\t", "");
            //}
            pressedKey = e.KeyChar;

        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e) {
            richTextBox1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
            richTextBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e) {
            IDataObject cbObj = Clipboard.GetDataObject();
            if (cbObj.GetDataPresent(DataFormats.Text)) {
                richTextBox1.Paste();
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            richTextBox1.SelectAll();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e) {
            richTextBox1.Undo();
            undoToolStripMenuItem.Enabled = richTextBox1.CanUndo;
            redoToolStripMenuItem.Enabled = true;
            IsChanged();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e) {
            richTextBox1.Redo();
            undoToolStripMenuItem.Enabled = true;
            redoToolStripMenuItem.Enabled = richTextBox1.CanRedo;
            IsChanged();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e) {
            undoToolStripMenuItem.Enabled = true;
            redoToolStripMenuItem.Enabled = false;
            Text = fileName + "* - " + Form1.CODE_EDITOR;
            (Form1.consoleForm.Controls.Find("debug", true)[0]).Text = "";
            IsChanged();
            try {
                Interpreter.Lang.Compile(richTextBox1.Text);
            } catch (Exception ex) {
                (Form1.consoleForm.Controls.Find("debug", true)[0]).Text = ex.Message;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            if (Text.Contains('*')) {
                switch (MessageBox.Show(fileName + "への変更を保存しますか？", Form1.CODE_EDITOR, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)) {
                    case DialogResult.Yes:
                        saveToolStripMenuItem.PerformClick();
                        break;
                    case DialogResult.No:
                        RichTextBoxReset();
                        break;
                    default:
                        return;
                }
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                using (StreamReader sr = new StreamReader(openFileDialog1.FileName, Encoding.UTF8)) {
                    fileDetail = sr.ReadToEnd();
                    richTextBox1.Text = fileDetail;
                }
                fileName = openFileDialog1.FileName;
                saveFileDialog1.FileName = openFileDialog1.FileName;
                Text = openFileDialog1.FileName + " - " + Form1.CODE_EDITOR;
                richTextBox1.ClearUndo();
                undoToolStripMenuItem.Enabled = false;
                redoToolStripMenuItem.Enabled = false;
                //richTextBox1.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText); // UTF-8以外のエンコーディングだと文字化けするので使わない
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            if (saveFileDialog1.FileName == "") {
                if (saveFileDialog1.ShowDialog() != DialogResult.OK) {
                    return;
                }
            }
            using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false, Encoding.UTF8)) {
                sw.Write(richTextBox1.Text);
                fileDetail = richTextBox1.Text;
            };
            fileName = saveFileDialog1.FileName;
            Text = fileName + " - " + Form1.CODE_EDITOR;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            saveFileDialog1.FileName = "";
            saveToolStripMenuItem.PerformClick();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            if (Text.Contains('*')) {
                switch (MessageBox.Show(fileName + "への変更を保存しますか？", Form1.CODE_EDITOR, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)) {
                    case DialogResult.Yes:
                        saveToolStripMenuItem.PerformClick();
                        RichTextBoxReset();
                        break;
                    case DialogResult.No:
                        RichTextBoxReset();
                        break;
                    default:
                        break;
                }
            } else {
                RichTextBoxReset();
            }
        }

        public void RichTextBoxReset() {
            fileName = "無題";
            fileDetail = "";
            redoToolStripMenuItem.Enabled = false;
            undoToolStripMenuItem.Enabled = false;
            openFileDialog1.FileName = "";
            saveFileDialog1.FileName = "";
            richTextBox1.Text = "";
            Text = fileName + " - " + Form1.CODE_EDITOR;
        }

        public void IsChanged() {
            if (richTextBox1.Text.Length == 0 && fileName == "無題") {
                Text = fileName + " - " + Form1.CODE_EDITOR;
                //return false;
            } else if (richTextBox1.Text.Length >= 0 && fileDetail == richTextBox1.Text) {
                Text = fileName + " - " + Form1.CODE_EDITOR;
                //return false;
            }
            //return true;
        }
    }
}
