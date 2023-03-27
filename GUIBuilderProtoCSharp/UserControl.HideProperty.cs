using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace GUIBuilderProtoCSharp {
    internal partial class UserControl {
        #region 非表示プロパティ

        #endregion
    }
    internal partial class UserButton {
        #region 非表示プロパティ
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
        public ContextMenuStrip ContextMenuStrip {
            get {
                return base.ContextMenuStrip;
            }
            set {
                base.ContextMenuStrip = value;
            }
        }

        [Browsable(false)]
        public DialogResult DialogResult {
            get {
                return base.DialogResult;
            }
            set {
                base.DialogResult = value;
            }
        }

        [Browsable(false)]
        public bool UseCompatibleTextRendering {
            get {
                return base.UseCompatibleTextRendering;
            }
            set {
                base.UseCompatibleTextRendering = value;
            }
        }

        [Browsable(false)]
        public Cursor Cursor {
            get {
                return base.Cursor;
            }
            set {
                base.Cursor = value;
            }
        }

        [Browsable(false)]
        public int ImageIndex {
            get {
                return base.ImageIndex;
            }
            set {
                base.ImageIndex = value;
            }
        }

        [Browsable(false)]
        public string ImageKey {
            get {
                return base.ImageKey;
            }
            set {
                base.ImageKey = value;
            }
        }

        [Browsable(false)]
        public ImageList ImageList {
            get {
                return base.ImageList;
            }
            set {
                base.ImageList = value;
            }
        }

        [Browsable(false)]
        public RightToLeft RightToLeft {
            get {
                return base.RightToLeft;
            }
            set {
                base.RightToLeft = value;
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

        [Browsable(true), Description("オブジェクトを識別するコードで使われる名前です。") /* Description(((DescriptionAttribute)TypeDescriptor.GetProperties(new UserButton())["Name"].Attributes[typeof(DescriptionAttribute)]).Description) */]
        public string Name {
            get {
                return base.Name;
            }
            set {
                //AttributeCollection ac = TypeDescriptor.GetProperties(new UserButton())["Name"].Attributes;
                //string da = ((DescriptionAttribute)TypeDescriptor.GetProperties(new UserButton())["Name"].Attributes[typeof(DescriptionAttribute)]).Description;
                try {
                    int before = int.Parse(base.Name.Replace(this.GetType().BaseType.Name, ""));
                    nameManageList[before - 1] = false;
                } catch (Exception) { }
                try {
                    int after = int.Parse(value.Replace(this.GetType().BaseType.Name, ""));
                    nameManageList[after - 1] = true;
                } catch (Exception) { }
                base.Name = value;
            }
        }

        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public new Control Parent {
            get {
                return base.Parent;
            }
            set {
            }
        }

        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public IntPtr Handle {
            get {
                return base.Handle;
            }
        }

        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public AccessibleObject AccessibilityObject{
            get {
                return base.AccessibilityObject;
            }
        }
        #endregion
    }

    internal partial class UserCheckBox {
        #region 非表示プロパティ
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
        public ContextMenuStrip ContextMenuStrip {
            get {
                return base.ContextMenuStrip;
            }
            set {
                base.ContextMenuStrip = value;
            }
        }

        [Browsable(false)]
        public bool UseCompatibleTextRendering {
            get {
                return base.UseCompatibleTextRendering;
            }
            set {
                base.UseCompatibleTextRendering = value;
            }
        }

        [Browsable(false)]
        public Cursor Cursor {
            get {
                return base.Cursor;
            }
            set {
                base.Cursor = value;
            }
        }

        [Browsable(false)]
        public int ImageIndex {
            get {
                return base.ImageIndex;
            }
            set {
                base.ImageIndex = value;
            }
        }

        [Browsable(false)]
        public string ImageKey {
            get {
                return base.ImageKey;
            }
            set {
                base.ImageKey = value;
            }
        }

        [Browsable(false)]
        public ImageList ImageList {
            get {
                return base.ImageList;
            }
            set {
                base.ImageList = value;
            }
        }

        [Browsable(false)]
        public RightToLeft RightToLeft {
            get {
                return base.RightToLeft;
            }
            set {
                base.RightToLeft = value;
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

        [Browsable(true), Description("オブジェクトを識別するコードで使われる名前です。") /* Description(((DescriptionAttribute)TypeDescriptor.GetProperties(new UserButton())["Name"].Attributes[typeof(DescriptionAttribute)]).Description) */]
        public string Name {
            get {
                return base.Name;
            }
            set {
                //AttributeCollection ac = TypeDescriptor.GetProperties(new UserButton())["Name"].Attributes;
                //string da = ((DescriptionAttribute)TypeDescriptor.GetProperties(new UserButton())["Name"].Attributes[typeof(DescriptionAttribute)]).Description;
                try {
                    int before = int.Parse(base.Name.Replace(this.GetType().BaseType.Name, ""));
                    nameManageList[before - 1] = false;
                } catch (Exception) { }
                try {
                    int after = int.Parse(value.Replace(this.GetType().BaseType.Name, ""));
                    nameManageList[after - 1] = true;
                } catch (Exception) { }
                base.Name = value;
            }
        }

        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public new Control Parent {
            get {
                return base.Parent;
            }
            set {
            }
        }

        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public IntPtr Handle {
            get {
                return base.Handle;
            }
        }

        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public AccessibleObject AccessibilityObject{
            get {
                return base.AccessibilityObject;
            }
        }
        #endregion        
    }
    internal partial class UserCheckedListBox {
        #region 非表示プロパティ
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
        public ContextMenuStrip ContextMenuStrip {
            get {
                return base.ContextMenuStrip;
            }
            set {
                base.ContextMenuStrip = value;
            }
        }

        [Browsable(false)]
        public bool UseCompatibleTextRendering {
            get {
                return base.UseCompatibleTextRendering;
            }
            set {
                base.UseCompatibleTextRendering = value;
            }
        }

        [Browsable(false)]
        public Cursor Cursor {
            get {
                return base.Cursor;
            }
            set {
                base.Cursor = value;
            }
        }

        [Browsable(false)]
        public RightToLeft RightToLeft {
            get {
                return base.RightToLeft;
            }
            set {
                base.RightToLeft = value;
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

        [Browsable(true), Description("オブジェクトを識別するコードで使われる名前です。") /* Description(((DescriptionAttribute)TypeDescriptor.GetProperties(new UserButton())["Name"].Attributes[typeof(DescriptionAttribute)]).Description) */]
        public string Name {
            get {
                return base.Name;
            }
            set {
                //AttributeCollection ac = TypeDescriptor.GetProperties(new UserButton())["Name"].Attributes;
                //string da = ((DescriptionAttribute)TypeDescriptor.GetProperties(new UserButton())["Name"].Attributes[typeof(DescriptionAttribute)]).Description;
                try {
                    int before = int.Parse(base.Name.Replace(this.GetType().BaseType.Name, ""));
                    nameManageList[before - 1] = false;
                } catch (Exception) { }
                try {
                    int after = int.Parse(value.Replace(this.GetType().BaseType.Name, ""));
                    nameManageList[after - 1] = true;
                } catch (Exception) { }
                base.Name = value;
            }
        }

        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public new Control Parent {
            get {
                return base.Parent;
            }
            set {
            }
        }

        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public IntPtr Handle {
            get {
                return base.Handle;
            }
        }

        [System.Text.Json.Serialization.JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public AccessibleObject AccessibilityObject{
            get {
                return base.AccessibilityObject;
            }
        }
        #endregion
    }
}
