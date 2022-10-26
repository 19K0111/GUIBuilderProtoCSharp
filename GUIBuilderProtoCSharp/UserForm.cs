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
    public class UserForm : Form {
        #region 非表示プロパティ
        [Browsable(false)]
        public IButtonControl AcceptButton {
            get {
                return base.AcceptButton;
            }
            set {
                base.AcceptButton = value;
            }
        }

        [Browsable(false)]
        public IButtonControl CancelButton {
            get {
                return base.CancelButton;
            }
            set {
                base.CancelButton = value;
            }
        }

        [Browsable(false)]
        public bool KeyPreview {
            get {
                return base.KeyPreview;
            }
            set {
                base.KeyPreview = value;
            }
        }

        [Browsable(false)]
        public bool HelpButton {
            get {
                return base.HelpButton;
            }
            set {
                base.HelpButton = value;
            }
        }

        [Browsable(false)]
        public bool IsMdiContainer {
            get {
                return base.IsMdiContainer;
            }
            set {
                base.IsMdiContainer = value;
            }
        }

        [Browsable(false)]
        public MenuStrip MainMenuStrip {
            get {
                return base.MainMenuStrip;
            }
            set {
                base.MainMenuStrip = value;
            }
        }

        [Browsable(false)]
        public bool MdiChildrenMinimizedAnchorBottom {
            get {
                return base.MdiChildrenMinimizedAnchorBottom;
            }
            set {
                base.MdiChildrenMinimizedAnchorBottom = value;
            }
        }

        [Browsable(false)]
        public SizeGripStyle SizeGripStyle {
            get {
                return base.SizeGripStyle;
            }
            set {
                base.SizeGripStyle = value;
            }
        }

        [Browsable(false)]
        public Color TransparencyKey {
            get {
                return base.TransparencyKey;
            }
            set {
                base.TransparencyKey = value;
            }
        }

        [Browsable(false)]
        public bool CausesValidation {
            get {
                return base.CausesValidation;
            }
            set {
                base.CausesValidation = value;
            }
        }

        [Browsable(false)]
        public string AccessibleDescription {
            get {
                return base.AccessibleDescription;
            }
            set {
                base.AccessibleDescription = value;
            }
        }

        [Browsable(false)]
        public string AccessibleName {
            get {
                return base.AccessibleName;
            }
            set {
                base.AccessibleName = value;
            }
        }

        [Browsable(false)]
        public AccessibleRole AccessibleRole {
            get {
                return base.AccessibleRole;
            }
            set {
                base.AccessibleRole = value;
            }
        }

        [Browsable(false)]
        public bool AllowDrop {
            get {
                return base.AllowDrop;
            }
            set {
                base.AllowDrop = value;
            }
        }

        [Browsable(false)]
        public AutoValidate AutoValidate {
            get {
                return base.AutoValidate;
            }
            set {
                base.AutoValidate = value;
            }
        }

        [Browsable(false)]
        public ContextMenuStrip ContextMenuStrip {
            get {
                return base.ContextMenuStrip;
            }
            set {
                base.ContextMenuStrip = value;
            }
        }

        [Browsable(false)]
        public ImeMode ImeMode {
            get {
                return base.ImeMode;
            }
            set {
                base.ImeMode = value;
            }
        }

        [Browsable(false)]
        public DockStyle Dock {
            get {
                return base.Dock;
            }
            set {
                base.Dock = value;
            }
        }

        [Browsable(false)]
        public Point Location {
            get {
                return base.Location;
            }
            set {
                base.Location = value;
            }
        }

        [Browsable(false)]
        public FormStartPosition StartPosition {
            get {
                return base.StartPosition;
            }
            set {
                base.StartPosition = value;
            }
        }

        [Browsable(false)]
        public FormWindowState WindowState {
            get {
                return base.WindowState;
            }
            set {
                base.WindowState = value;
            }
        }

        [Browsable(false)]
        public FormBorderStyle FormBorderStyle {
            get {
                return base.FormBorderStyle;
            }
            set {
                base.FormBorderStyle = value;
            }
        }

        [Browsable(false)]
        public bool UseWaitCursor {
            get {
                return base.UseWaitCursor;
            }
            set {
                base.UseWaitCursor = value;
            }
        }
        #endregion
    }
}
