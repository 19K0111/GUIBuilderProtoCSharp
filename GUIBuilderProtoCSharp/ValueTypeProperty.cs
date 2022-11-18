using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GUIBuilderProtoCSharp {
    public partial class ValueTypeProperty : System.Windows.Forms.UserControl {
        public object returnValue;
        public ValueTypeProperty() {
            InitializeComponent();
        }

        private void ValueTypeProperty_MouseDoubleClick(object sender, MouseEventArgs e) {
            // ListViewの値を編集するときにテキストボックスを表示させる方法
            // https://qiita.com/Toraja/items/51dd3ec878c647583231
            ListViewItem.ListViewSubItem currentColumn = listView1.SelectedItems[0].SubItems[1];
            Rectangle rect = currentColumn.Bounds;
            rect.Intersect(listView1.ClientRectangle);
            rect.Y += 1; // 微調整 環境によっては表示が崩れる可能性がある
            rect.X += 5; // 微調整 環境によっては表示が崩れる可能性がある

            // NumericUpDownを使用
            numericUpDown1.Minimum = int.MinValue;
            numericUpDown1.Maximum = int.MaxValue;
            numericUpDown1.Value = int.Parse(currentColumn.Text);
            numericUpDown1.Visible = true;
            numericUpDown1.Bounds = rect;
            numericUpDown1.Focus();
            numericUpDown1.Select(0, numericUpDown1.Value.ToString().Length);
        }
    }
}
