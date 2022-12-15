using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GUIBuilderProtoCSharp {
    public partial class Form6 : Form {
        public Form6() {
            InitializeComponent();
        }

        Dictionary<string, List<string>> macro = new();
        readonly string FILE_NAME = $"{Form1.workingDirectory}\\event.macro";

        public void AddMacro(string key) {
            List<string> copy = new();
            // Deep Copyで実装しないと不具合がおこる
            foreach (var item in listBox2.Items) {
                copy.Add(item.ToString());
            }
            macro[key] = copy;
        }

        public void RemoveMacro(string key) {
            macro.Remove(key);
        }

        public void RenameMacro(string oldKey, string newKey) {
            if (oldKey.GetHashCode() == newKey.GetHashCode()) { return; }
            List<string> copy = macro[oldKey];
            macro[newKey] = copy;
            macro.Remove(oldKey);
        }

        private void Form6_Load(object sender, EventArgs e) {
            textBox1.Text = "";
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            try {
                using (StreamReader sr = new StreamReader(FILE_NAME)) {
                    macro = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, List<string>>>(sr.ReadToEnd(), ProjectJson.options);
                    foreach (var k in macro.Keys) {
                        listBox1.Items.Add(k.ToString());
                    }
                }
            } catch (FileNotFoundException) {
                using (StreamWriter sw = new StreamWriter(FILE_NAME)) {

                }
            }
            foreach (var item in Interpreter.Lang.eventList) {
                listBox3.Items.Add(item.Name);
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e) {
            // ListViewの値を編集するときにテキストボックスを表示させる方法
            // https://qiita.com/Toraja/items/51dd3ec878c647583231
            // Rectangle rect = new Rectangle(listBox1.Location.X, listBox1.Location.Y + listBox1.ItemHeight * listBox1.SelectedIndex, listBox1.Width, listBox1.ItemHeight);
            // rect.Intersect(listBox1.ClientRectangle);
            // rect.Y += 1; // 微調整 環境によっては表示が崩れる可能性がある
            // rect.X += 5; // 微調整 環境によっては表示が崩れる可能性がある

            textBox1.Visible = true;
            try {
                textBox1.Text = listBox1.SelectedItem.ToString();
            } catch (NullReferenceException) { }
            // textBox1.Bounds = rect;
            textBox1.Focus();
            textBox1.SelectAll();
        }

        private void textBox1_Leave(object sender, EventArgs e) {
            // textBox1.Visible = false;
            try {
                string oldKey = listBox1.SelectedItem.ToString();
                string newKey = textBox1.Text;
                //if (listBox1.Items.Contains(newKey)) {
                // 同じ名前のイベントマクロを含むとき
                // RemoveMacro(oldKey);
                //} else {
                RenameMacro(oldKey, newKey);
                //}
                listBox1.SelectedItem = textBox1.Text;
                listBox1.Items[listBox1.SelectedIndex] = $"{textBox1.Text}";
            } catch (ArgumentOutOfRangeException) {
            } catch (NullReferenceException) { }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) {
            // ListViewの値を編集するときにテキストボックスを表示させる方法
            // https://qiita.com/Toraja/items/51dd3ec878c647583231
            switch (e.KeyChar) {
                case (char)Keys.Enter:
                    listBox1.Focus();
                    e.Handled = true;
                    break;
                case (char)Keys.Escape:
                    try {
                        textBox1.Text = listBox1.SelectedItem.ToString();
                    } catch (NullReferenceException) { }
                    listBox1.Focus();
                    e.Handled = true;
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            const string newName = "Macro";
            int i = 1;
            while (true) {
                if (listBox1.Items.Contains(newName + i)) {
                    i++;
                } else {
                    break;
                }
            }
            listBox2.Items.Clear();
            AddMacro($"{newName}{i}");
            listBox1.Items.Add($"{newName}{i}");
            listBox1.SelectedItem = $"{newName}{i}";
            listBox1_DoubleClick(sender, e);
        }

        private void button2_Click(object sender, EventArgs e) {
            try {
                button7.Enabled = false;
                RemoveMacro(listBox1.SelectedItem.ToString());
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                listBox2.Enabled = false;
                listBox3.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
            } catch (ArgumentOutOfRangeException) { }
        }

        private void button3_Click(object sender, EventArgs e) {
            try {
                object tmp = listBox2.Items[listBox2.SelectedIndex];
                listBox2.Items[listBox2.SelectedIndex] = listBox2.Items[listBox2.SelectedIndex - 1];
                listBox2.Items[listBox2.SelectedIndex - 1] = tmp;
                listBox2.SelectedIndex = listBox2.SelectedIndex - 1;
                AddMacro(listBox1.SelectedItem.ToString());
            } catch (ArgumentOutOfRangeException) { }
        }

        private void button4_Click(object sender, EventArgs e) {
            try {
                object tmp = listBox2.Items[listBox2.SelectedIndex];
                listBox2.Items[listBox2.SelectedIndex] = listBox2.Items[listBox2.SelectedIndex + 1];
                listBox2.Items[listBox2.SelectedIndex + 1] = tmp;
                listBox2.SelectedIndex = listBox2.SelectedIndex + 1;
                AddMacro(listBox1.SelectedItem.ToString());
            } catch (ArgumentOutOfRangeException) { }
        }

        private void button5_Click(object sender, EventArgs e) {
            try {
                listBox2.Items.Add(listBox3.SelectedItem);
                AddMacro(listBox1.SelectedItem.ToString());
            } catch (ArgumentNullException) { }
            button7.Enabled = listBox2.Items.Count > 0;
        }

        private void button6_Click(object sender, EventArgs e) {
            int index = listBox2.SelectedIndex;
            try {
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                AddMacro(listBox1.SelectedItem.ToString());
                listBox2.SelectedIndex = index;
            } catch (ArgumentOutOfRangeException) {
                // 最後の要素を削除したとき
                try {
                    listBox2.SelectedIndex = index - 1;
                } catch (IndexOutOfRangeException) {
                    // 削除したらListBox2が空になったとき
                    listBox2.SelectedIndex = -1;
                } catch (ArgumentOutOfRangeException) {
                }
            } catch (IndexOutOfRangeException) {
            }
            button7.Enabled = listBox2.Items.Count > 0;
        }

        private void button8_Click(object sender, EventArgs e) {
            while (listBox2.SelectedIndex > 0) {
                button3_Click(sender, e);
            }
        }

        private void button9_Click(object sender, EventArgs e) {
            while (listBox2.SelectedIndex != listBox2.Items.Count - 1) {
                button4_Click(sender, e);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            if (listBox1.Items.Count > 0) {
                listBox2.Enabled = listBox1.SelectedItem != null;
                listBox3.Enabled = listBox1.SelectedItem != null;
                button2.Enabled = listBox1.SelectedItem != null;
                button3.Enabled = listBox1.SelectedItem != null;
                button4.Enabled = listBox1.SelectedItem != null;
                button5.Enabled = listBox1.SelectedItem != null;
                button6.Enabled = listBox1.SelectedItem != null;
                button8.Enabled = listBox1.SelectedItem != null;
                button9.Enabled = listBox1.SelectedItem != null;
                listBox2.Items.Clear();
                try {
                    foreach (var item in macro[listBox1.SelectedItem.ToString()]) {
                        listBox2.Items.Add(item);
                    }
                    button7.Enabled = listBox2.Items.Count > 0;
                    textBox1.Text = listBox1.SelectedItem.ToString();
                } catch (KeyNotFoundException) {
                } catch (NullReferenceException) { }
            } else {
                listBox2.Enabled = false;
                listBox3.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
            }
        }

        private void Form6_FormClosing(object sender, FormClosingEventArgs e) {
            using (StreamWriter sw = new StreamWriter(FILE_NAME)) {
                sw.Write(System.Text.Json.JsonSerializer.Serialize(macro, macro.GetType(), ProjectJson.options));
            }
        }

        private void button7_Click(object sender, EventArgs e) {
            Form1.f3.Focus();
            foreach (var item in listBox2.Items) {
                Interpreter.EventList.Do(item.ToString());
            }
        }

        private void listBox3_DoubleClick(object sender, EventArgs e) {
            if (listBox3.SelectedItem != null) {
                button5_Click(sender, e);
                listBox3.SelectedItem = null;
            }
        }
    }
}
