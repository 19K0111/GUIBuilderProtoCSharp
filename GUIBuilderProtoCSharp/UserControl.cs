using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIBuilderProtoCSharp {
    internal class UserControl : Control {
        private static Point p_begin; // ドラッグ開始時のマウス座標(デザイナーウィンドウ左上基準)
        private static bool moving; // コントロールを動かしているときtrue
        private static bool resizing;
        private static bool pressing;
        private static Control? selecting;
        private static String flag = "Default";

        internal static void UserControl_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                p_begin = new Point(e.X, e.Y); // コントロールの左上基準
                selecting = (Control)sender;
                // System.Diagnostics.Debug.WriteLine($"({e.X}, {e.Y})");
            }
            pressing = true;
        }
        internal static void UserControl_MouseMove(object sender, MouseEventArgs e) {
            Point mouse = Form1.f2.PointToClient(Cursor.Position);// デザイナーウィンドウ左上基準
                                                                  //System.Diagnostics.Debug.WriteLine($"(e.X    , e.Y    ) = ({e.X}, {e.Y})\n(mouse.X, mouse.Y) = ({mouse.X}, {mouse.Y})\n(p_begin.X, p_begin.Y) = ({p_begin.X}, {p_begin.Y})");

            Control t = (Control)sender;
            Control previewControl = Form1.f3.Controls.Find(t.Name, true)[0];
            //System.Diagnostics.Debug.WriteLine(t.Size);
            //System.Diagnostics.Debug.WriteLine(t.Location);


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
                Size originSize = t.Size;
                Point p = new Point(t.Location.X, t.Location.Y);
                if (flag == "NWSE" && !moving) {
                    if (-5 <= p_begin.X && p_begin.X <= 5 && -5 <= p_begin.Y && p_begin.Y <= 5) {
                        // 左上のサイズ変更
                        t.Location = new Point(mouse.X, mouse.Y);
                        t.Size = new Size(selecting.Width + p.X - mouse.X, selecting.Height + p.Y - mouse.Y);
                    } else {
                        // 右下のサイズ変更
                        t.Size = new Size(mouse.X - p.X, mouse.Y - p.Y);
                    }
                    resizing = true;
                } else if (flag == "NESW" && !moving) {
                    if (-5 + originSize.Width <= p_begin.X && p_begin.X <= 5 + originSize.Width && -5 <= p_begin.Y && p_begin.Y <= 5) {
                        // 右上のサイズ変更
                        t.Location = new Point(p.X, mouse.Y);
                        t.Size = new Size(mouse.X - p.X, selecting.Height + (p.Y - mouse.Y));
                        // t.Size = new Size(selecting.Width + p.X - mouse.X + selecting.Width, selecting.Height + p.Y - mouse.Y);// 修正する
                    } else {
                        // 左下のサイズ変更
                        t.Location = new Point(mouse.X, p.Y);
                        t.Size = new Size(mouse.X - p.X, mouse.Y - p.Y);//修正する
                    }
                    Form1.f1.toolStripStatusLabel1.Text = $"{t.Name}: {t.Size.Width} x {t.Size.Height}, 座標：({t.Location.X}, {t.Location.Y})";
                    resizing = true;
                } else if (flag == "WE" && !moving) {
                    if (-5 <= p_begin.X && p_begin.X <= 5) {
                        // 左のサイズ変更
                        t.Location = new Point(mouse.X, t.Location.Y);
                        t.Size = new Size(selecting.Width + p.X - mouse.X, selecting.Height);
                    } else {
                        // 右のサイズ変更
                        t.Size = new Size(mouse.X - p.X, selecting.Height);
                    }
                    resizing = true;
                } else if (flag == "NS" && !moving) {
                    if (-5 <= p_begin.Y && p_begin.Y <= 5) {
                        // 上のサイズ変更
                        t.Location = new Point(t.Location.X, mouse.Y);
                        t.Size = new Size(selecting.Width, selecting.Height + (p.Y - mouse.Y));
                    } else {
                        // 下のサイズ変更
                        t.Size = new Size(selecting.Width, mouse.Y - p.Y);
                    }
                    resizing = true;
                } else if (flag == "Default" && !resizing && p_begin.X >= 0 && p_begin.Y >= 0) {
                    // 選択したコントロールを動かす
                    Point p_end = new Point(e.X - p_begin.X + t.Location.X, e.Y - p_begin.Y + t.Location.Y);
                    t.Location = p_end;
                    // previewControl.Location = p_end; // もう1つのウィンドウにもあるコントロールを動かす
                    moving = true;
                } else {
                    System.Diagnostics.Debug.WriteLine("Else");
                }
                previewControl.Size = new Size((int)t.Size.Width, (int)t.Size.Height);
                previewControl.Location = new Point(t.Location.X, t.Location.Y);
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
            if (resizing) {

            }
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
                            UserButton.Delete(userButton);
                            break;
                        case UserCheckBox userCheckBox:
                            System.Diagnostics.Debug.WriteLine($"2: {userCheckBox.GetType().Name}");
                            UserCheckBox.Number--;
                            break;
                        default:
                            break;
                    }
                    Control previewControl = Form1.f3.Controls.Find(selecting.Name, true)[0];
                    Form1.f3.Controls.Remove(previewControl); // プレビューウィンドウのコントロールを削除
                    previewControl.Dispose();
                    Control designControl = Form1.f2.Controls.Find(selecting.Name, true)[0];
                    Form1.f3.Controls.Remove(designControl); // デザインウィンドウのコントロールを削除
                    designControl.Dispose();
                    selecting = Form1.f2.ActiveControl; // 選択中のコントロールを変える

                    Form1.f1.Cursor = Cursors.Default;
                    flag = "Default";
                    moving = false;
                    resizing = false;
                    p_begin = new Point(-1, -1); // 左クリックしたままコントロールにカーソルが当たるとコントロールが移動するので、負の座標を与えて移動出来なくする
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
            Control previewControl = Form1.f3.Controls.Find(selecting.Name, true)[0];
            previewControl.Location = selecting.Location;
        }
    }


    internal class UserButton : Button {
        static int num = 0;
        internal static List<UserButton>? UserButtons = new List<UserButton>();

        public UserButton() {
            // コンストラクタ
            this.Name = $"{GetType().BaseType.Name}{++Number}";
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

            for (int i = 0; i < UserButtons.Count; i++) {
                // Nameが重複しないようにする処理
                if (UserButtons[i] != null) {
                    if (UserButtons[i].Name == $"{GetType().BaseType.Name}{Number}") {

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
        }

        /// <summary>
        /// デザインウィンドウに表示している個数
        /// </summary>
        public static int Number {
            get {
                return num;
            }
            private set {
                num = value;
            }
        }

        public static void Delete(UserButton d) {
            Number--;
            foreach (var item in UserButton.UserButtons) {
                if (item != null && item.Name == d.Name) {
                    UserButtons[UserButton.UserButtons.IndexOf(item)] = null;
                    break;
                }
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
    internal class UserCheckBox : CheckBox {
        static int num = 0;

        public UserCheckBox() {
            // コンストラクタ
            this.Name = $"CheckBox{++Number}";
            this.Location = new Point(0, 0);
            this.Size = new Size(88, 19);
            this.TabIndex = 0;
            this.Text = this.Name;
            this.UseVisualStyleBackColor = true;

            this.MouseDown += UserControl.UserControl_MouseDown;
            this.MouseMove += UserControl.UserControl_MouseMove;
            ;
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
        public static int Number {
            get {
                return num;
            }
            set {
                num = value;
            }
        }
    }
}
