using System;
using System.Collections.Generic;
using System.Text;

namespace GUIBuilderProtoCSharp {
    /// <summary>
    ///  "やり直し"、"元に戻す"を実現するクラス
    /// </summary>
    internal class Modify {
        /// <summary>
        /// コントロールが配置されているフォーム名
        /// </summary>
        public Form TargetForm {
            get; set;
        }
        /// <summary>
        /// 変更対象のコントロール
        /// </summary>
        public object TargetControl {
            get; set;
        }
        /// <summary>
        /// プレビューウィンドウのコントロール
        /// </summary>
        public object PreviewControl {
            get; set;
        }
        /// <summary>
        /// コントロールの名前
        /// </summary>
        public string ClassName {
            get; private set;
        }
        /// <summary>
        /// コントロールに対する操作
        /// </summary>
        public OperationCode Operation {
            get; private set;
        }
        /// <summary>
        /// 変更前の操作
        /// </summary>
        public List<object> Before {
            get; set;
        }
        /// <summary>
        /// 変更後の操作
        /// </summary>
        public List<object> After {
            get; set;
        }
        /// <summary>
        /// 操作コード
        /// </summary>
        public enum OperationCode {
            Create = 0,
            Modify = 1,
            Delete = 2,
        }
        public Modify(/*string type,*/ OperationCode operation, object control, Form parentForm, List<object> before, List<object> after) {
            ClassName = control.GetType().Name;
            Operation = operation;
            TargetControl = control;
            PreviewControl = UserControl.FindPreview((Control)control);
            TargetForm = parentForm;
            Before = before;
            After = after;
        }
        public static void Redo(Stack<Modify> stack, Stack<Modify> push_stack) {
            // やり直し [Ctrl] + [Y]
            // OperationNameの通りの操作をする
            try {
                Modify top = stack.Pop();
                switch (top.Operation) {
                    case OperationCode.Create:
                    case OperationCode.Delete:
                        Operate(top, top.Operation);
                        break;
                    case OperationCode.Modify:
                        Operate(top, top.After);
                        break;
                    default:
                        break;
                }
                push_stack.Push(top);
                Check(push_stack, stack);
            } catch (InvalidOperationException ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        public static void Undo(Stack<Modify> stack, Stack<Modify> push_stack) {
            // 元に戻す [Ctrl] + [Z]
            // OperationNameとは逆の操作をする
            try {
                Modify top = stack.Pop();
                switch (top.Operation) {
                    case OperationCode.Create:
                        Operate(top, OperationCode.Delete);
                        break;
                    case OperationCode.Modify:
                        Operate(top, top.Before);
                        break;
                    case OperationCode.Delete:
                        Operate(top, OperationCode.Create);
                        break;
                    default:
                        break;
                }
                push_stack.Push(top);
                Check(stack, push_stack);
            } catch (InvalidOperationException ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        public static void Operate(Modify m, OperationCode op_code) {
            switch (op_code) {
                case OperationCode.Create:
                    // Form1.f1.Create(m.ClassName);
                    m.TargetForm.Controls.Add((Control)m.TargetControl);
                    switch (m.TargetControl) {
                        case UserButton userButton:
                            UserButton.Add(userButton);
                            userButton.BringToFront();
                            UserButton.UserButtons[(int)m.Before[0]] = userButton;
                            List<object> op = new List<object> { op_code };
                            UserControl.Sync(m, op_code);
                            break;
                        default:
                            break;
                    }
                    break;
                case OperationCode.Modify:
                    // 例外を発生させても良いかもしれない
                    break;
                case OperationCode.Delete:
                    switch (m.TargetControl) {
                        case UserButton userButton:
                            UserButton.Delete(userButton);
                            m.TargetForm.Controls.Remove((Control)m.TargetControl);
                            UserControl.Sync(m, op_code);
                            break;
                        default:
                            break;
                    }
                    //switch (m.ClassName) {
                    //    case "Button":
                    //        UserButton.Delete((UserButton)m.TargetControl);
                    //        break;
                    //    default:
                    //        break;
                    //}
                    break;
                default:
                    break;
            }
        }
        public static void Operate(Modify m, List<object> op) {
            foreach (object item in op) {
                switch (item) {
                    case Point p:
                        ((Control)m.TargetControl).Location = p;
                        break;
                    case Size s:
                        ((Control)m.TargetControl).Size = s;
                        break;
                    default:
                        break;
                }
            }
            UserControl.Sync(m, op);
        }

        public static void Check(Stack<Modify> undoStack, Stack<Modify> redoStack) {
            if (undoStack.Count == 0) {
                Form1.f1.undoToolStripMenuItem.Enabled = false;
                Form1.f1.undoToolStripButton.Enabled = false;
            } else {
                Form1.f1.undoToolStripMenuItem.Enabled = true;
                Form1.f1.undoToolStripButton.Enabled = true;
            }
            if (redoStack.Count == 0) {
                Form1.f1.redoToolStripMenuItem.Enabled = false;
                Form1.f1.redoToolStripButton.Enabled = false;
            } else {
                Form1.f1.redoToolStripMenuItem.Enabled = true;
                Form1.f1.redoToolStripButton.Enabled = true;
            }
        }
    }
}
