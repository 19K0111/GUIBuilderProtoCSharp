using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIBuilderProtoCSharp {
    internal partial class UserControl : Control {
        public const int BUFFER = 1024;

        private static Point p_begin; // ドラッグ開始時のマウス座標(デザイナーウィンドウ左上基準)
        private static bool l_click; // 左クリックしているときtrue
        private static bool moving; // コントロールを動かしているときtrue
        private static bool resizing;
        private static bool pressing;
        private static bool shift; // Shiftキーを押しているときtrue
        private static bool ctrl;
        private static Control? selecting;
        private static Size originSize;
        private static Point originLocation;
        private static string flag = "Default";

        public static readonly string[] NOT_CHANGED_PROPERTY = { "Enabled", "Visible" };

        internal static List<object> userControls = new();
        internal static List<object> UserControls {
            get {
                return userControls;
            }
        }

        public static void init() {
            l_click = false; // 左クリックしているときtrue
            moving = false; // コントロールを動かしているときtrue
            resizing = false;
            pressing = false;
            shift = false; // Shiftキーを押しているときtrue
            ctrl = false;
            selecting = null;
            UserButton.Init();
            UserCheckBox.Init();
        }
        public static Control? FindPreview(Control? design) {
            if (design == null) { return null; }
            return Form1.f3.Controls.Find(design.Name, true)[0];
        }
        public static Control? FindDesign(Control preview) {
            if (preview == null) { return null; }
            return Form1.f2.Controls.Find(preview.Name, true)[0];
        }

        public static void Sync(Modify m, List<object> operations) {
            Control? previewControl = null;
            try {
                previewControl = Form1.f3.Controls.Find(((Control)m.TargetControl).Name, true)[0]; // Formの数を複数扱う場合は、変える必要がある
            } catch (IndexOutOfRangeException) {
            } catch (NullReferenceException) {
                previewControl = m.TargetPreviewForm;
            }
            foreach (var item in operations) {
                switch (item) {
                    case Point p:
                        previewControl.Location = p;
                        break;
                    case Size s:
                        previewControl.Size = s;
                        break;
                    case Control c:
                        if (Form1.f3.Controls.Find(c.Name, true) != null) {
                            Form1.f3.Controls.Add(c);
                        } else {
                            Form1.f3.Controls.Remove(c);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public static void Sync(Modify m, Modify.OperationCode op_code) {
            switch (op_code) {
                case Modify.OperationCode.Create:
                    foreach (var item in m.After) {
                        Form1.f3.Controls.Add((Control)m.PreviewControl);
                        ((Control)m.PreviewControl).BringToFront();
                    }
                    break;
                case Modify.OperationCode.Modify:
                    m.TargetForm.Controls.Find(((Control)m.TargetControl).Name, true);
                    break;
                case Modify.OperationCode.Delete:
                    Form1.f3.Controls.Remove(Form1.f3.Controls.Find(((Control)m.TargetControl).Name, true)[0]);
                    //foreach (var item in m.After) {
                    //Form1.f3.Controls.Remove((Control)item);
                    //}
                    break;
                default:
                    break;
            }
        }

        public static Control GetSelectedControl() {
            return selecting;
        }
        public static Type GetSelectedControlType() {
            return selecting.GetType();
        }

        public static Control GetSelectedControlPreview() {
            return Form1.f3.Controls.Find(selecting.Name, true)[0];
        }
        public static Type GetSelectedControlPreviewType() {
            return GetSelectedControlPreview().GetType();
        }

        public static string AvailableName(string name) {
            // すでに同じ名前のコントロールのNameと衝突しないよう、コントロール作成時のNameを設定
            string intName = "";
            string typeName = "";
            int intNameToInt = 0;
            try {
                intName = System.Text.RegularExpressions.Regex.Replace(name, @"[^0-9]", "");
                typeName = name.Split(intName)[0];
                intNameToInt = int.Parse(intName);
            } catch (Exception) { }
            if ((typeName + intName).GetHashCode() == name.GetHashCode()) {
                try {
                    if (Form1.f3.Controls.Find(name, true)[0].Name.GetHashCode() == name.GetHashCode()) {
                        intNameToInt++;
                        return AvailableName(typeName + intNameToInt.ToString());
                    }
                } catch (IndexOutOfRangeException) { }
            }
            return name;
        }

        internal static void UserControl_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                p_begin = new Point(e.X, e.Y); // コントロールの左上基準
                //selecting = (Control)sender;
                originSize = selecting.Size;
                originLocation = selecting.Location;
                System.Diagnostics.Debug.WriteLine($"({e.X}, {e.Y})");
                pressing = true;
            }
        }
        internal static void UserControl_MouseMove(object sender, MouseEventArgs e) {
            Point mouse = Form1.f2.PointToClient(Cursor.Position);// デザイナーウィンドウ左上基準
            //System.Diagnostics.Debug.WriteLine($"(e.X    , e.Y    ) = ({e.X}, {e.Y})\n(mouse.X, mouse.Y) = ({mouse.X}, {mouse.Y})\n(p_begin.X, p_begin.Y) = ({p_begin.X}, {p_begin.Y})");

            Control t = (Control)sender;
            Control previewControl = Form1.f3.Controls.Find(t.Name, true)[0];
            //System.Diagnostics.Debug.WriteLine(t.Size);
            //System.Diagnostics.Debug.WriteLine(t.Location);

            if (selecting != null) {
                if (!moving && !pressing) {
                    if ((mouse.X - 5 <= t.Location.X && t.Location.X <= mouse.X + 5) && (mouse.Y - 5 <= t.Location.Y && t.Location.Y <= mouse.Y + 5) ||
                        (mouse.X - 5 <= t.Location.X + t.Size.Width && t.Location.X + t.Size.Width <= mouse.X + 5) && (mouse.Y - 5 <= t.Location.Y + t.Size.Height && t.Location.Y + t.Size.Height <= mouse.Y + 5)) {
                        Form1.f1.Cursor = Cursors.SizeNWSE; // 左上右下カーソル
                        flag = "NWSE";
                    } else if ((mouse.X - 5 <= t.Location.X + t.Size.Width && t.Location.X + t.Size.Width <= mouse.X + 5) && (mouse.Y - 5 <= t.Location.Y && t.Location.Y <= mouse.Y + 5) ||
                           (mouse.X - 5 <= t.Location.X && t.Location.X <= mouse.X + 5) && (mouse.Y - 5 <= t.Location.Y + t.Size.Height && t.Location.Y + t.Size.Height <= mouse.Y + 5)) {
                        Form1.f1.Cursor = Cursors.SizeNESW; // 左下右上カーソル
                        flag = "NESW";
                    } else if (((mouse.X - 5 <= t.Location.X && t.Location.X <= mouse.X + 5) || (mouse.X - 5 <= t.Location.X + t.Size.Width && t.Location.X + t.Size.Width <= mouse.X + 5))
                          && (t.Location.Y <= mouse.Y && mouse.Y <= t.Location.Y + t.Size.Height)) {
                        Form1.f1.Cursor = Cursors.SizeWE; // 横に広げるカーソル
                        flag = "WE";
                    } else if (((mouse.Y - 5 <= t.Location.Y && t.Location.Y <= mouse.Y + 5) || (mouse.Y - 5 <= t.Location.Y + t.Size.Height && t.Location.Y + t.Size.Height <= mouse.Y + 5))
                          && (t.Location.X <= mouse.X && mouse.X <= t.Location.X + t.Size.Width)) {
                        Form1.f1.Cursor = Cursors.SizeNS; // 縦に広げるカーソル
                        flag = "NS";
                    } else if (!resizing) {
                        Form1.f1.Cursor = Cursors.Default;
                        flag = "Default";
                    }
                }


                if (e.Button == MouseButtons.Left) {
                    Point p = new Point(t.Location.X, t.Location.Y);
                    if (flag == "NWSE" && !moving) {
                        if (-5 <= p_begin.X && p_begin.X <= 5 && -5 <= p_begin.Y && p_begin.Y <= 5) {
                            // 左上のサイズ変更
                            t.Location = new Point(mouse.X, mouse.Y);
                            t.Size = new Size(selecting.Width + p.X - mouse.X, selecting.Height + p.Y - mouse.Y);
                            if (selecting.Width <= 0) {
                                // 右端がずれないようにする
                                selecting.Width += 1;
                                t.Location = new Point(originLocation.X + originSize.Width, t.Location.Y);
                            }
                            if (selecting.Height <= 0) {
                                // 下端がずれないようにする
                                selecting.Height += 1;
                                t.Location = new Point(t.Location.X, originLocation.Y + originSize.Height);
                            }
                        } else {
                            // 右下のサイズ変更
                            t.Size = new Size(mouse.X - p.X, mouse.Y - p.Y);
                            if (selecting.Width <= 0) { // 幅が0以下にならないようにする
                                selecting.Width += 1;
                            }
                            if (selecting.Height <= 0) { // 高さが0以下にならないようにする
                                selecting.Height += 1;
                            }
                        }
                        resizing = true;
                    } else if (flag == "NESW" && !moving) {
                        if (-5 + originSize.Width <= p_begin.X && p_begin.X <= 5 + originSize.Width && -5 <= p_begin.Y && p_begin.Y <= 5) {
                            // 右上のサイズ変更
                            t.Location = new Point(p.X, mouse.Y);
                            t.Size = new Size(mouse.X - p.X, selecting.Height + (p.Y - mouse.Y));
                            if (selecting.Width <= 0) { // 幅が0以下にならないようにする
                                selecting.Width += 1;
                            }
                            if (selecting.Height <= 0) {
                                // 下端がずれないようにする
                                selecting.Height += 1;
                                t.Location = new Point(t.Location.X, originLocation.Y + originSize.Height);
                            }
                        } else {
                            // 左下のサイズ変更
                            t.Location = new Point(mouse.X, p.Y);
                            t.Size = new Size(selecting.Width + p.X - mouse.X, mouse.Y - p.Y);
                            if (selecting.Width <= 0) {
                                // 右端がずれないようにする
                                selecting.Width += 1;
                                t.Location = new Point(originLocation.X + originSize.Width, t.Location.Y);
                            }
                            if (selecting.Height <= 0) { // 高さが0以下にならないようにする
                                selecting.Height += 1;
                            }
                        }
                        resizing = true;
                    } else if (flag == "WE" && !moving) {
                        if (-5 <= p_begin.X && p_begin.X <= 5) {
                            // 左のサイズ変更
                            t.Location = new Point(mouse.X, t.Location.Y);
                            t.Size = new Size(selecting.Width + p.X - mouse.X, selecting.Height);
                            if (selecting.Width <= 0) {
                                // 右端がずれないようにする
                                selecting.Width += 1;
                                t.Location = new Point(originLocation.X + originSize.Width, t.Location.Y);
                            }
                        } else {
                            // 右のサイズ変更
                            t.Size = new Size(mouse.X - p.X, selecting.Height);
                            if (selecting.Width <= 0) { // 幅が0以下にならないようにする
                                selecting.Width += 1;
                            }
                        }
                        resizing = true;
                    } else if (flag == "NS" && !moving) {
                        if (-5 <= p_begin.Y && p_begin.Y <= 5) {
                            // 上のサイズ変更
                            t.Location = new Point(t.Location.X, mouse.Y);
                            t.Size = new Size(selecting.Width, selecting.Height + (p.Y - mouse.Y));
                            if (selecting.Height <= 0) {
                                // 下端がずれないようにする
                                selecting.Height += 1;
                                t.Location = new Point(t.Location.X, originLocation.Y + originSize.Height);
                            }
                        } else {
                            // 下のサイズ変更
                            t.Size = new Size(selecting.Width, mouse.Y - p.Y);
                            if (selecting.Height <= 0) { // 高さが0以下にならないようにする
                                selecting.Height += 1;
                            }
                        }
                        resizing = true;
                    } else if (flag == "Default" && !resizing && p_begin.X >= 0 && p_begin.Y >= 0) {
                        // 選択したコントロールを動かす
                        if (shift) {
                            // Shiftキーが押されている間は、水平or垂直方向のみ移動できるようにする
                            if (Math.Abs(mouse.X - originLocation.X) > Math.Abs(mouse.Y - originLocation.Y)) {
                                t.Location = new Point(e.X - p_begin.X + t.Location.X, originLocation.Y);
                            } else {
                                t.Location = new Point(originLocation.X, e.Y - p_begin.Y + t.Location.Y);
                            }
                        } else {
                            t.Location = new Point(e.X - p_begin.X + t.Location.X, e.Y - p_begin.Y + t.Location.Y);
                        }
                            (Form1.consoleForm.Controls.Find("debug", true)[0]).Text = $"e = ({e.X}, {e.Y})\r\np_begin = ({p_begin.X}, {p_begin.Y})\r\n" +
                                $"Screen.originLocation = ({Form1.f2.PointToScreen(originLocation)}), Screen.tLocation = {Form1.f2.PointToScreen(t.Location)}\r\nmouse = ({mouse.X}, {mouse.Y})";
                        moving = true;
                    } else {
                        System.Diagnostics.Debug.WriteLine("Else");
                    }
                    previewControl.Size = new Size((int)t.Size.Width, (int)t.Size.Height); // もう1つのウィンドウにもあるコントロールのサイズを変える
                    previewControl.Location = new Point(t.Location.X, t.Location.Y); // もう1つのウィンドウにもあるコントロールを動かす
                    Form1.f1.toolStripStatusLabel1.Text = $"{t.Name}: {t.Size.Width} x {t.Size.Height}, 座標：({t.Location.X}, {t.Location.Y})";
                    //(Form1.consoleForm.Controls.Find("debug", true)[0]).Text = $"p = ({p.X}, {p.Y})\r\np_begin = ({p_begin.X}, {p_begin.Y})\r\n" +
                    //    $"selecting = ({selecting.Location.X}. {selecting.Location.Y})\r\nmouse = ({mouse.X}, {mouse.Y})\r\n e = ({e.X}, {e.Y})";
                }
            }
        }
        internal static void UserControl_MouseEnter(object sender, EventArgs e) {
        }
        internal static void UserControl_MouseLeave(object sender, EventArgs e) {
            if (!resizing) {
                Form1.f1.Cursor = Cursors.Default;
                flag = "Default";
                moving = false;
            }
        }
        internal static void UserControl_MouseUp(object sender, MouseEventArgs e) {
            if (moving || resizing) {
                System.Reflection.PropertyInfo? propertyInfo = null;
                if (resizing) {
                    propertyInfo = ((Control)sender).GetType().GetProperty("Size");
                } else {
                    propertyInfo = ((Control)sender).GetType().GetProperty("Location");
                }
                if (originLocation != ((Control)sender).Location || originSize != ((Control)sender).Size) {
                    List<object> before = new List<object> { originLocation, originSize };
                    List<object> after = new List<object> { ((Control)sender).Location, ((Control)sender).Size };
                    Form1.undo.Push(new Modify(Modify.OperationCode.Modify, ((Control)sender), ((Control)sender).FindForm(), before, after, propertyInfo));
                    Form1.redo.Clear();
                    Form1.f1.SetPropView(FindDesign((Control)sender));
                }
            }
            Modify.Check(Form1.undo, Form1.redo);
            Form1.f1.Cursor = Cursors.Default;
            flag = "Default";
            moving = false;
            resizing = false;
            pressing = false;

        }

        internal static void UserControl_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Delete: // コントロールを削除する
                    // System.Diagnostics.Debug.WriteLine($"{selecting.GetType().Name}");
                    switch (selecting) { // 各GUI部品によって個数を減らす処理
                        case UserButton userButton:
                            System.Diagnostics.Debug.WriteLine($"1: {userButton.GetType().Name}");
                            List<object> before = new List<object>() { userButton.Index };
                            Form1.undo.Push(new Modify(Modify.OperationCode.Delete, userButton, userButton.FindForm(), before, before));
                            UserButton.Delete(userButton);
                            break;
                        case UserCheckBox userCheckBox:
                            System.Diagnostics.Debug.WriteLine($"2: {userCheckBox.GetType().Name}");
                            List<object> before2 = new List<object>() { userCheckBox.Index };
                            Form1.undo.Push(new Modify(Modify.OperationCode.Delete, userCheckBox, userCheckBox.FindForm(), before2, before2));
                            UserCheckBox.Delete(userCheckBox);
                            break;
                        case UserCheckedListBox userCheckedListBox:
                            System.Diagnostics.Debug.WriteLine($"2: {userCheckedListBox.GetType().Name}");
                            List<object> before3 = new List<object>() { userCheckedListBox.Index };
                            Form1.undo.Push(new Modify(Modify.OperationCode.Delete, userCheckedListBox, userCheckedListBox.FindForm(), before3, before3));
                            UserCheckedListBox.Delete(userCheckedListBox);
                            break;
                        case UserComboBox userComboBox:
                            System.Diagnostics.Debug.WriteLine($"2: {userComboBox.GetType().Name}");
                            List<object> before4 = new List<object>() { userComboBox.Index };
                            Form1.undo.Push(new Modify(Modify.OperationCode.Delete, userComboBox, userComboBox.FindForm(), before4, before4));
                            UserComboBox.Delete(userComboBox);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    Control previewControl = Form1.f3.Controls.Find(selecting.Name, true)[0];
                    Form1.f3.Controls.Remove(previewControl); // プレビューウィンドウのコントロールを削除
                    // previewControl.Dispose();
                    Form1.undo.Peek().After = new List<object> { previewControl };
                    Control designControl = Form1.f2.Controls.Find(selecting.Name, true)[0];
                    Form1.f2.Controls.Remove(designControl); // デザインウィンドウのコントロールを削除
                    // designControl.Dispose();
                    Form1.redo.Clear();
                    Modify.Check(Form1.undo, Form1.redo);
                    selecting = Form1.f2.ActiveControl; // 選択中のコントロールを変える

                    Form1.f1.Cursor = Cursors.Default;
                    flag = "Default";
                    moving = false;
                    resizing = false;
                    p_begin = new Point(-1, -1); // 左クリックしたままコントロールにカーソルが当たるとコントロールが移動するので、負の座標を与えて移動出来なくする
                    break;
                case Keys.ShiftKey:
                    // System.Diagnostics.Debug.WriteLine("Shift down");
                    shift = true;
                    break;
                default:
                    break;
            }
            // System.Diagnostics.Debug.WriteLine($"{e.KeyValue}");
        }
        internal static void UserControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            System.Diagnostics.Debug.WriteLine(e.KeyCode);
            switch (e.KeyCode) {
                case Keys.Up:
                    selecting.Location = new Point(selecting.Location.X, selecting.Location.Y - 1);
                    break;
                case Keys.Right:
                    selecting.Location = new Point(selecting.Location.X + 1, selecting.Location.Y);
                    break;
                case Keys.Down:
                    selecting.Location = new Point(selecting.Location.X, selecting.Location.Y + 1);
                    break;
                case Keys.Left:
                    selecting.Location = new Point(selecting.Location.X - 1, selecting.Location.Y);
                    break;
                default:
                    break;
            }
            try {
                Control previewControl = Form1.f3.Controls.Find(selecting.Name, true)[0];
                previewControl.Location = selecting.Location;
            } catch (IndexOutOfRangeException ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Form1.consoleForm.Controls.Find("debug", true)[0].Text = ex.Message;
            }
        }

        internal static void UserControl_KeyUp(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.ShiftKey:
                    // System.Diagnostics.Debug.WriteLine("Shift Up");
                    shift = false;
                    break;
                default:
                    break;
            }
        }

        internal static void UserControl_Enter(object sender, EventArgs e) {
            selecting = (Control)sender;
            Form1.f1.SetPropView(FindPreview((Control)sender));
        }

        internal static void UserControl_Leave(object sender, EventArgs e) {

        }
    }


    internal partial class UserButton : Button {
        //static int num = 0;
        internal static List<UserButton>? UserButtons = new List<UserButton>();
        private static bool[] nameManageList = new bool[UserControl.BUFFER];

        public UserButton() {
            // コンストラクタ
            this.Name = $"{GetType().BaseType.Name}0"; // 仮にButton0とおく
            this.Location = new Point(0, 0);
            this.Size = new Size(75, 23);
            this.TabIndex = 0;
            this.Text = this.Name;
            this.UseVisualStyleBackColor = true;

            this.MouseDown += UserControl.UserControl_MouseDown;
            this.MouseMove += UserControl.UserControl_MouseMove;
            //this.MouseEnter += UserControl.UserControl_MouseEnter;
            this.MouseLeave += UserControl.UserControl_MouseLeave;
            this.MouseUp += UserControl.UserControl_MouseUp;
            this.KeyDown += UserControl.UserControl_KeyDown;
            this.PreviewKeyDown += UserControl.UserControl_PreviewKeyDown;
            this.KeyUp += UserControl.UserControl_KeyUp;
            this.Enter += UserControl.UserControl_Enter;
            this.Leave += UserControl.UserControl_Leave;

            for (int i = 0; i <= UserButtons.Count; i++) {
                // Nameが重複しないようにする処理
                if (!nameManageList[i]) {
                    this.Name = UserControl.AvailableName($"{GetType().BaseType.Name}{i + 1}");
                    this.Text = this.Name;
                    break;
                }
                //if (UserButtons[i] != null) {
                //    if (UserButtons[i].Name == $"{GetType().BaseType.Name}{Count}") {

                //    }
                //} else {
                //    this.Name = $"{GetType().BaseType.Name}{i + 1}";
                //    UserButtons[i] = this;
                //    this.Text = this.Name;
                //    return;
                //}
            }
            Add(this);

            //for (int i = 0; i < UserButtons.Count; i++) {
            //    // Nameが重複しないようにする処理
            //    if (UserButtons[i] != null) {
            //        try {
            //            int hit = int.Parse(UserButtons[i].Name.Replace(GetType().BaseType.Name, ""));
            //            nameManageList[hit - 1] = true;
            //        } catch (FormatException) {
            //            continue;
            //        }
            //    } else {
            //        // this.Name = $"{GetType().BaseType.Name}{i + 1}";
            //        // UserButtons[i] = this;
            //        // this.Text = this.Name;
            //        // return;
            //    }
            //}
            //for (int i = 0; i < UserButtons.Count; i++) {
            //    if (!nameManageList[i]) {
            //        this.Name = $"{GetType().BaseType.Name}{i + 1}";
            //        UserButtons[i] = this;
            //        this.Text = this.Name;
            //        nameManageList[i] = true;
            //        UserButtons.Add(this);
            //        return;
            //    }
            //}
            //UserButtons.Add(this);
        }

        public UserButton(UserButton t) {
            // コピーコンストラクタ
            //this.Name = t.Name;
            //this.Location = t.Location;
            //this.Size = t.Size;
            //this.TabIndex = t.TabIndex;
            //this.Text = t.Text;
            //this.UseVisualStyleBackColor = t.UseVisualStyleBackColor;

            PropertyCopier.ControlCopy(t, this);

            this.Click += UserButton_Click;
        }

        public static void Init() {
            UserButton.UserButtons?.Clear();
            nameManageList = new bool[UserControl.BUFFER];
            UserControl.UserControls.Add(UserButton.UserButtons);
        }

        private void UserButton_Click(object? sender, EventArgs e) {
            Console.WriteLine("Clicked: " + this.Name);
            try {
                // Interpreter.EventList.Do(this.Name + ".Click"); // Button1.Clickを検索して呼び出す方法
                Interpreter.EventList.Do(this.Name + "_Click"); // Button1_Clickを検索して呼び出す方法　関数呼び出しに対応できる方法
            } catch (Exception ex) {
                (Form1.consoleForm.Controls.Find("debug", true)[0]).Text = ex.Message;
            }
        }

        /// <summary>
        /// デザインウィンドウに表示している個数
        /// </summary>
        public static int Count {
            get {
                return UserButtons.Count;
            }
            //private set {
            //    num = value;
            //}
        }

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
        public int Index {
            get {
                return UserButtons.IndexOf(this);
                //for (int i = 0; i < UserButtons.Count; i++) {
                //    if (UserButtons[i].Name == Name) {
                //        return i;
                //    }
                //}
                //return -1;
            }
            private set {
            }
        }

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
        public string ControlTypeName {
            get {
                return this.GetType().Name;
            }
        }

        public static void Delete(UserButton d) {
            //Count--;
            foreach (var item in UserButton.UserButtons) {
                if (item != null && item.Name == d.Name) {
                    // UserButtons[d.Index] = null;
                    UserButtons.Remove(item);
                    try {
                        nameManageList[int.Parse(d.Name.Replace(d.GetType().BaseType.Name, "")) - 1] = false;
                    } catch (Exception) { }
                    break;
                }
            }
        }

        public static void Add(UserButton d) {
            UserButtons.Add(d);
            try {
                nameManageList[int.Parse(d.Name.Replace(d.GetType().BaseType.Name, "")) - 1] = true;
            } catch (Exception) { }

            //Count++;
        }

        public static void UpdateNameManageList() {
            foreach (var item in UserButtons) {
                try {
                    nameManageList[int.Parse(item.Name.Replace(item.GetType().BaseType.Name, "")) - 1] = true;
                } catch (Exception) { }
            }
        }

        //private Point p_begin;

        //private void UserButton_MouseDown(object? sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        p_begin = new Point(e.X, e.Y);
        //    }
        //}
        //private void UserButton_MouseMove(object? sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left) {
        //        Control t = (Control)sender;
        //        Point p_end = new Point(e.X - p_begin.X + t.Location.X, e.Y - p_begin.Y + t.Location.Y);
        //        t.Location = p_end;
        //        Control previewButton = Form1.f3.Controls.Find(t.Name, true)[0];
        //        previewButton.Location = p_end;
        //    }
        //}
    }
    internal partial class UserCheckBox : CheckBox {
        // static int num = 0;
        internal static List<UserCheckBox>? UserCheckBoxes = new();
        private static bool[] nameManageList = new bool[UserControl.BUFFER];

        public UserCheckBox() {
            // コンストラクタ
            this.Name = $"{GetType().Name}0";
            this.Location = new Point(0, 0);
            this.Size = new Size(88, 19);
            this.TabIndex = 0;
            this.Text = this.Name;
            this.UseVisualStyleBackColor = true;

            this.MouseDown += UserControl.UserControl_MouseDown;
            this.MouseMove += UserControl.UserControl_MouseMove;
            //this.MouseEnter += UserControl.UserControl_MouseEnter;
            this.MouseLeave += UserControl.UserControl_MouseLeave;
            this.MouseUp += UserControl.UserControl_MouseUp;
            this.KeyDown += UserControl.UserControl_KeyDown;
            this.PreviewKeyDown += UserControl.UserControl_PreviewKeyDown;
            this.KeyUp += UserControl.UserControl_KeyUp;
            this.Enter += UserControl.UserControl_Enter;
            this.Leave += UserControl.UserControl_Leave;

            for (int i = 0; i <= UserCheckBoxes.Count; i++) {
                // Nameが重複しないようにする処理
                if (!nameManageList[i]) {
                    this.Name = UserControl.AvailableName($"{GetType().BaseType.Name}{i + 1}");
                    this.Text = this.Name;
                    break;
                }
            }
            Add(this);
        }

        public UserCheckBox(UserCheckBox t) {
            // コピーコンストラクタ
            //this.Name = t.Name;
            //this.Location = t.Location;
            //this.Size = t.Size;
            //this.TabIndex = t.TabIndex;
            //this.Text = t.Text;
            //this.UseVisualStyleBackColor = t.UseVisualStyleBackColor;
            PropertyCopier.ControlCopy(t, this);

            this.Click += UserCheckBox_Click;
        }

        public static void Init() {
            UserCheckBox.UserCheckBoxes?.Clear();
            nameManageList = new bool[UserControl.BUFFER];
            UserControl.UserControls.Add(UserCheckBox.UserCheckBoxes);
        }

        private void UserCheckBox_Click(object sender, EventArgs e) {
            Console.WriteLine("Clicked: " + this.Name);
            try {
                // Interpreter.EventList.Do(this.Name + ".Click"); // CheckBox1.Clickを検索して呼び出す方法
                Interpreter.EventList.Do(this.Name + "_Click"); // CheckBox1_Clickを検索して呼び出す方法　関数呼び出しに対応できる方法
            } catch (Exception ex) {
                (Form1.consoleForm.Controls.Find("debug", true)[0]).Text = ex.Message;
            }
        }

        /// <summary>
        /// デザインウィンドウに表示している個数
        /// </summary>
        public static int Count {
            get {
                return UserCheckBoxes.Count;
            }
        }
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
        public int Index {
            get {
                return UserCheckBoxes.IndexOf(this);
                //for (int i = 0; i < UserCheckBoxes.Count; i++) {
                //    if (UserCheckBoxes[i].Name == Name) {
                //        return i;
                //    }
                //}
                //return -1;
            }
            private set {
            }
        }

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
        public string ControlTypeName {
            get {
                return this.GetType().Name;
            }
        }

        public static void Delete(UserCheckBox d) {
            //Count--;
            foreach (var item in UserCheckBox.UserCheckBoxes) {
                if (item != null && item.Name == d.Name) {
                    // UserCheckBoxes[d.Index] = null;
                    UserCheckBoxes.Remove(item);
                    try {
                        nameManageList[int.Parse(d.Name.Replace(d.GetType().BaseType.Name, "")) - 1] = false;
                    } catch (Exception) { }
                    break;
                }
            }
        }

        public static void Add(UserCheckBox d) {
            UserCheckBoxes.Add(d);
            try {
                nameManageList[int.Parse(d.Name.Replace(d.GetType().BaseType.Name, "")) - 1] = true;
            } catch (Exception) { }

            //Count++;
        }

        public static void UpdateNameManageList() {
            foreach (var item in UserCheckBoxes) {
                try {
                    nameManageList[int.Parse(item.Name.Replace(item.GetType().BaseType.Name, "")) - 1] = true;
                } catch (Exception) { }
            }
        }
    }

    internal partial class UserCheckedListBox : CheckedListBox/*, IUserControl<UserCheckedListBox>*/ {
        // static int num = 0;
        internal static List<UserCheckedListBox>? UserCheckedListBoxes = new();
        private static bool[] nameManageList = new bool[UserControl.BUFFER];

        public UserCheckedListBox() {
            // コンストラクタ
            this.Name = $"{GetType().Name}0";
            this.Location = new Point(0, 0);
            this.Size = new Size(120, 88);
            this.TabIndex = 0;
            this.Text = this.Name;

            this.MouseDown += UserControl.UserControl_MouseDown;
            this.MouseMove += UserControl.UserControl_MouseMove;
            //this.MouseEnter += UserControl.UserControl_MouseEnter;
            this.MouseLeave += UserControl.UserControl_MouseLeave;
            this.MouseUp += UserControl.UserControl_MouseUp;
            this.KeyDown += UserControl.UserControl_KeyDown;
            this.PreviewKeyDown += UserControl.UserControl_PreviewKeyDown;
            this.KeyUp += UserControl.UserControl_KeyUp;
            this.Enter += UserControl.UserControl_Enter;
            this.Leave += UserControl.UserControl_Leave;

            for (int i = 0; i <= UserCheckedListBoxes.Count; i++) {
                // Nameが重複しないようにする処理
                if (!nameManageList[i]) {
                    this.Name = UserControl.AvailableName($"{GetType().BaseType.Name}{i + 1}");
                    this.Text = this.Name;
                    break;
                }
            }
            Add(this);
        }

        public UserCheckedListBox(UserCheckedListBox t) {
            // コピーコンストラクタ
            //this.Name = t.Name;
            //this.Location = t.Location;
            //this.Size = t.Size;
            //this.TabIndex = t.TabIndex;
            //this.Text = t.Text;
            //this.UseVisualStyleBackColor = t.UseVisualStyleBackColor;
            PropertyCopier.ControlCopy(t, this);

            this.Click += UserCheckedListBox_Click;
        }

        public void Init(ref List<UserCheckedListBox> l, ref bool[] nameManageList) {
            l.Clear();
            nameManageList = new bool[UserControl.BUFFER];
            UserControl.UserControls.Add(UserCheckedListBox.UserCheckedListBoxes);
            
            //UserCheckedListBox.UserCheckedListBoxes?.Clear();
            //nameManageList = new bool[UserControl.BUFFER];
            //UserControl.UserControls.Add(UserCheckedListBox.UserCheckedListBoxes);
        }

        private void UserCheckedListBox_Click(object sender, EventArgs e) {
            Console.WriteLine("Clicked: " + this.Name);
            try {
                // Interpreter.EventList.Do(this.Name + ".Click"); // CheckedListBox1.Clickを検索して呼び出す方法
                Interpreter.EventList.Do(this.Name + "_Click"); // CheckedListBox1_Clickを検索して呼び出す方法　関数呼び出しに対応できる方法
            } catch (Exception ex) {
                (Form1.consoleForm.Controls.Find("debug", true)[0]).Text = ex.Message;
            }
        }

        /// <summary>
        /// デザインウィンドウに表示している個数
        /// </summary>
        public static int Count {
            get {
                return UserCheckedListBoxes.Count;
            }
        }
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
        public int Index {
            get {
                return UserCheckedListBoxes.IndexOf(this);
                //for (int i = 0; i < UserCheckedListBoxes.Count; i++) {
                //    if (UserCheckedListBoxes[i].Name == Name) {
                //        return i;
                //    }
                //}
                //return -1;
            }
            private set {
            }
        }

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
        public string ControlTypeName {
            get {
                return this.GetType().Name;
            }
        }

        /*
        public new List<string> Items {
            get; set;
        }
        */

        public static void Delete(UserCheckedListBox d) {
            //Count--;
            foreach (var item in UserCheckedListBox.UserCheckedListBoxes) {
                if (item != null && item.Name == d.Name) {
                    // UserCheckedListBoxes[d.Index] = null;
                    UserCheckedListBoxes.Remove(item);
                    try {
                        nameManageList[int.Parse(d.Name.Replace(d.GetType().BaseType.Name, "")) - 1] = false;
                    } catch (Exception) { }
                    break;
                }
            }
        }

        public static void Add(UserCheckedListBox d) {
            UserCheckedListBoxes.Add(d);
            try {
                nameManageList[int.Parse(d.Name.Replace(d.GetType().BaseType.Name, "")) - 1] = true;
            } catch (Exception) { }

            //Count++;
        }

        public static void UpdateNameManageList() {
            foreach (var item in UserCheckedListBoxes) {
                try {
                    nameManageList[int.Parse(item.Name.Replace(item.GetType().BaseType.Name, "")) - 1] = true;
                } catch (Exception) { }
            }
        }
        
    }

    internal partial class UserComboBox : ComboBox/*, IUserControl<UserComboBox>*/ {
        // static int num = 0;
        internal static List<UserComboBox>? UserComboBoxes = new();
        private static bool[] nameManageList = new bool[UserControl.BUFFER];

        public UserComboBox() {
            // コンストラクタ
            this.Name = $"{GetType().Name}0";
            this.Location = new Point(0, 0);
            this.Size = new Size(120, 88);
            this.TabIndex = 0;
            this.Text = this.Name;

            this.MouseDown += UserControl.UserControl_MouseDown;
            this.MouseMove += UserControl.UserControl_MouseMove;
            //this.MouseEnter += UserControl.UserControl_MouseEnter;
            this.MouseLeave += UserControl.UserControl_MouseLeave;
            this.MouseUp += UserControl.UserControl_MouseUp;
            this.KeyDown += UserControl.UserControl_KeyDown;
            this.PreviewKeyDown += UserControl.UserControl_PreviewKeyDown;
            this.KeyUp += UserControl.UserControl_KeyUp;
            this.Enter += UserControl.UserControl_Enter;
            this.Leave += UserControl.UserControl_Leave;

            for (int i = 0; i <= UserComboBoxes.Count; i++) {
                // Nameが重複しないようにする処理
                if (!nameManageList[i]) {
                    this.Name = UserControl.AvailableName($"{GetType().BaseType.Name}{i + 1}");
                    this.Text = this.Name;
                    break;
                }
            }
            Add(this);
        }

        public UserComboBox(UserComboBox t) {
            // コピーコンストラクタ
            //this.Name = t.Name;
            //this.Location = t.Location;
            //this.Size = t.Size;
            //this.TabIndex = t.TabIndex;
            //this.Text = t.Text;
            //this.UseVisualStyleBackColor = t.UseVisualStyleBackColor;
            PropertyCopier.ControlCopy(t, this);

            this.Click += UserComboBox_Click;
        }

        public void Init(ref List<UserComboBox> l, ref bool[] nameManageList) {
            l.Clear();
            nameManageList = new bool[UserControl.BUFFER];
            UserControl.UserControls.Add(UserComboBox.UserComboBoxes);
            
            //UserComboBox.UserComboBoxes?.Clear();
            //nameManageList = new bool[UserControl.BUFFER];
            //UserControl.UserControls.Add(UserComboBox.UserComboBoxes);
        }

        private void UserComboBox_Click(object sender, EventArgs e) {
            Console.WriteLine("Clicked: " + this.Name);
            try {
                // Interpreter.EventList.Do(this.Name + ".Click"); // ComboBox1.Clickを検索して呼び出す方法
                Interpreter.EventList.Do(this.Name + "_Click"); // ComboBox1_Clickを検索して呼び出す方法　関数呼び出しに対応できる方法
            } catch (Exception ex) {
                (Form1.consoleForm.Controls.Find("debug", true)[0]).Text = ex.Message;
            }
        }

        /// <summary>
        /// デザインウィンドウに表示している個数
        /// </summary>
        public static int Count {
            get {
                return UserComboBoxes.Count;
            }
        }
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
        public int Index {
            get {
                return UserComboBoxes.IndexOf(this);
                //for (int i = 0; i < UserComboBoxes.Count; i++) {
                //    if (UserComboBoxes[i].Name == Name) {
                //        return i;
                //    }
                //}
                //return -1;
            }
            private set {
            }
        }

        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
        public string ControlTypeName {
            get {
                return this.GetType().Name;
            }
        }

        /*
        public new List<string> Items {
            get; set;
        }
        */

        public static void Delete(UserComboBox d) {
            //Count--;
            foreach (var item in UserComboBox.UserComboBoxes) {
                if (item != null && item.Name == d.Name) {
                    // UserComboBoxes[d.Index] = null;
                    UserComboBoxes.Remove(item);
                    try {
                        nameManageList[int.Parse(d.Name.Replace(d.GetType().BaseType.Name, "")) - 1] = false;
                    } catch (Exception) { }
                    break;
                }
            }
        }

        public static void Add(UserComboBox d) {
            UserComboBoxes.Add(d);
            try {
                nameManageList[int.Parse(d.Name.Replace(d.GetType().BaseType.Name, "")) - 1] = true;
            } catch (Exception) { }

            //Count++;
        }

        public static void UpdateNameManageList() {
            foreach (var item in UserComboBoxes) {
                try {
                    nameManageList[int.Parse(item.Name.Replace(item.GetType().BaseType.Name, "")) - 1] = true;
                } catch (Exception) { }
            }
        }
        
    }

    /* 
     * 新たにGUI部品を定義するとき
     * 01. UserControl.csにクラスを定義
     * 02. UserControl.csのUserControlクラスのInitメソッドにUser[Component] Init()を追加
     * 03. Form1.csのCreateメソッドの対応する部分を編集
     * 04. 一度実行して、配置してみる　PropertyGridに表示されているプロパティに注目
     * 05. 必要に応じてPropertyGridに表示しないプロパティはUserControl.HideProperty.csに記述
     * 06. シリアライズ、デシリアライズできていないプロパティについては、ControlsJson.csにそのプロパティを記述
     * 07. 必要に応じてCustomJsonConverter.csにJSON読み込み、書き出しの際の処理を記述してProjectJson.csのフィールドConvertersに追加
     * 08. Modify.csのOperateメソッドを編集
     * 09. Form1.csのpropertyGrid1_PropertyValueChangedメソッドを編集
     * 10. Form1.csのLoadDesignメソッドを編集
     */
}
