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
    public partial class Form3 : UserForm, IDisposable {
        public Form3() {
            InitializeComponent();
        }
        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public new IntPtr Handle {
            get {
                return base.Handle;
            }
        }
        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore, Browsable(false)]
        public Cursor Cursor {
            get {
                return base.Cursor;
            }
            set {
                base.Cursor = value;
            }
        }
        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public AccessibleObject AccessibilityObject {
            get {
                return base.AccessibilityObject;
            }
        }
        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public Form Owner {
            get {
                return base.Owner;
            }
            set {
                base.Owner = value;
            }
        }
    }
}
