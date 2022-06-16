using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIBuilderProtoCSharp
{
    public partial class Form2 : Form
    {
        public static Form1 f1 = new Form1();
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_LocationChanged(object sender, EventArgs e)
        {
            // System.Diagnostics.Debug.WriteLine($"Location: ({this.Location.X}, {this.Location.Y})");
            if (this.Location.X < 140) Location = new Point(140, this.Location.Y);
            if (this.Location.Y < 0) Location = new Point(this.Location.X, 0);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) e.Cancel = true;
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            Form1.f3.Size = this.Size;

            Form1.f1.toolStripStatusLabel1.Text = $"サイズ：{this.Size.Width} x {this.Size.Height}";
            System.Diagnostics.Debug.WriteLine($"Form座標：({Location.X}, {Location.Y})");
        }
    }
}
