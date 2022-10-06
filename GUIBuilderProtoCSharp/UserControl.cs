using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIBuilderProtoCSharp {
    internal class UserControl : Control {
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

        //internal static List<object> UserControls = new List<object>();

        public static void init() {
            l_click = false; // 左クリックしているときtrue
            moving = false; // コントロールを動かしているときtrue
            resizing = false;
            pressing = false;
            shift = false; // Shiftキーを押しているときtrue
            ctrl = false;
            selecting = null;
            UserButton.UserButtons.Clear();
        }
        public static Control FindPreview(Control design) {
            return Form1.f3.Controls.Find(design.Name, true)[0];
        }

        public static void Sync(Modify m, List<object> operations) {
            Control? previewControl = null;
            try {
                previewControl = Form1.f3.Controls.Find(((Control)m.TargetControl).Name, true)[0]; // Formの数を複数扱う場合は、変える必要がある
            } catch (IndexOutOfRangeException) {
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

        internal static void UserControl_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                p_begin = new Point(e.X, e.Y); // コントロールの左上基準
                selecting = (Control)sender;
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
                if (originLocation != ((Control)sender).Location || originSize != ((Control)sender).Size) {
                    List<object> before = new List<object> { originLocation, originSize };
                    List<object> after = new List<object> { ((Control)sender).Location, ((Control)sender).Size };
                    Form1.undo.Push(new Modify(Modify.OperationCode.Modify, ((Control)sender), ((Control)sender).FindForm(), before, after));
                    Form1.redo.Clear();
                    Form1.f1.SetPropView((Control)sender);
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
                            UserCheckBox.Count--;
                            break;
                        default:
                            break;
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
            Form1.f1.SetPropView((Control)sender);
        }

        internal static void UserControl_Leave(object sender, EventArgs e) {

        }
    }


    internal class UserButton : Button {
        //static int num = 0;
        internal static List<UserButton>? UserButtons = new List<UserButton>();

        public UserButton() {
            // コンストラクタ
            this.Name = $"{GetType().BaseType.Name}{Count + 1}";
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

            for (int i = 0; i < UserButtons.Count; i++) {
                // Nameが重複しないようにする処理
                if (UserButtons[i] != null) {
                    if (UserButtons[i].Name == $"{GetType().BaseType.Name}{Count}") {

                    }
                } else {
                    this.Name = $"{GetType().BaseType.Name}{i + 1}";
                    UserButtons[i] = this;
                    this.Text = this.Name;
                    return;
                }
            }
            UserButtons.Add(this);

        }

        public UserButton(UserButton t) {
            // コピーコンストラクタ
            this.Name = t.Name;
            this.Location = t.Location;
            this.Size = t.Size;
            this.TabIndex = t.TabIndex;
            this.Text = t.Text;
            this.UseVisualStyleBackColor = t.UseVisualStyleBackColor;

            this.Click += UserButton_Click;
        }

        private void UserButton_Click(object? sender, EventArgs e) {
            Console.WriteLine("Clicked: " + this.Name);
            try {
                Interpreter.EventList.Do(this.Name + ".Click");
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
                Index = value; ;
            }
        }

        public static void Delete(UserButton d) {
            //Count--;
            foreach (var item in UserButton.UserButtons) {
                if (item != null && item.Name == d.Name) {
                    UserButtons[d.Index] = null;
                    break;
                }
            }
        }

        public static void Add(UserButton d) {
            //Count++;
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
    internal class UserCheckBox : CheckBox {
        static int num = 0;

        public UserCheckBox() {
            // コンストラクタ
            this.Name = $"CheckBox{++Count}";
            this.Location = new Point(0, 0);
            this.Size = new Size(88, 19);
            this.TabIndex = 0;
            this.Text = this.Name;
            this.UseVisualStyleBackColor = true;

            this.MouseDown += UserControl.UserControl_MouseDown;
            this.MouseMove += UserControl.UserControl_MouseMove;
        }

        public UserCheckBox(UserCheckBox t) {
            // コピーコンストラクタ
            this.Name = t.Name;
            this.Location = t.Location;
            this.Size = t.Size;
            this.TabIndex = t.TabIndex;
            this.Text = t.Text;
            this.UseVisualStyleBackColor = t.UseVisualStyleBackColor;
        }

        /// <summary>
        /// デザインウィンドウに表示している個数
        /// </summary>
        public static int Count {
            get {
                return num;
            }
            set {
                num = value;
            }
        }
    }
}
