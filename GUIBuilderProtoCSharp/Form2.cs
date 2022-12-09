using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIBuilderProtoCSharp {
    public partial class Form2 : Form {
        static Size originSize = new Size(300, 300);

        public static Form1 f1 = new Form1();
        public Form2() {
            InitializeComponent();
        }

        private void Form2_LocationChanged(object sender, EventArgs e) {
            // System.Diagnostics.Debug.WriteLine($"Location: ({this.Location.X}, {this.Location.Y})");
            if (this.Location.X < 0) Location = new Point(0, this.Location.Y);
            if (this.Location.Y < 0) Location = new Point(this.Location.X, 0);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e) {
            if (e.CloseReason == CloseReason.UserClosing) e.Cancel = true;
        }

        private void Form2_Resize(object sender, EventArgs e) {
            Form1.f3.Size = this.Size;

            Form1.f1.toolStripStatusLabel1.Text = $"サイズ：{this.Size.Width} x {this.Size.Height}";
            System.Diagnostics.Debug.WriteLine($"Form座標：({Location.X}, {Location.Y})");
        }

        private void Form2_Click(object sender, EventArgs e) {
            Form1.f1.propertyGrid1.SelectedObject = Form1.f3;
        }

        private void Form2_ResizeEnd(object sender, EventArgs e) {
            System.Reflection.PropertyInfo propertyInfo = ((Control)sender).GetType().GetProperty("Size");
            if (originSize != Size) {
                List<object> before = new List<object> { originSize };
                List<object> after = new List<object> { Size };
                Form1.undo.Push(new Modify(Modify.OperationCode.Modify, Form1.f3, this, before, after, propertyInfo));
                Form1.redo.Clear();
                Form1.f1.propertyGrid1.SelectedObject = Form1.f3;
                originSize = Size;
                Modify.Check(Form1.undo, Form1.redo);
            }

        }
    }
}
