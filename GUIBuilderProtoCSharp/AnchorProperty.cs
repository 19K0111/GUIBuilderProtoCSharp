using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GUIBuilderProtoCSharp {
    public partial class AnchorProperty : System.Windows.Forms.UserControl {
        public AnchorStyles returnValue = AnchorStyles.None;
        public AnchorProperty() {
            InitializeComponent();
        }

        public AnchorProperty(AnchorStyles current) : this() {
            checkBox1.Checked = (current & AnchorStyles.Top) == AnchorStyles.Top;
            checkBox2.Checked = (current & AnchorStyles.Left) == AnchorStyles.Left;
            checkBox3.Checked = (current & AnchorStyles.Right) == AnchorStyles.Right;
            checkBox4.Checked = (current & AnchorStyles.Bottom) == AnchorStyles.Bottom;
        }

        private void AnchorProperty_Load(object sender, EventArgs e) {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            returnValue ^= AnchorStyles.Top;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            returnValue ^= AnchorStyles.Left;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e) {
            returnValue ^= AnchorStyles.Right;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e) {
            returnValue ^= AnchorStyles.Bottom;
        }
    }
}
