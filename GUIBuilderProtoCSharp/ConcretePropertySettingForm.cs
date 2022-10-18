using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GUIBuilderProtoCSharp {
    public partial class ConcretePropertySettingForm : Form {
        public ConcretePropertySettingForm() {
            InitializeComponent();
        }

        private void ConcretePropertySettingForm_Leave(object sender, EventArgs e) {
            Close();
        }
    }
}
