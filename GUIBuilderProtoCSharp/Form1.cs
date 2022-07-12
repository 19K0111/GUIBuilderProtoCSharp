namespace GUIBuilderProtoCSharp {
    public partial class Form1 : Form {
        /*
            Button
            CheckBox
            CheckedListBox
            ComboBox
            DateTimePicker
            Label
            ListBox
            PictureBox
            ProgressBar
            RadioButton
            RichTextBox
            TextBox
         */

        int NumButton = 0;
        int NumCheckBox = 0;
        int NumCheckedListBox = 0;
        int NumComboBox = 0;
        int NumDateTimePicker = 0;
        int NumLabel = 0;
        int NumListBox = 0;
        int NumPictureBox = 0;
        int NumProgressBar = 0;
        int NumRadioButton = 0;
        int NumRichTextBox = 0;
        int NumTextBox = 0;

        List<Button> UserButtons = new List<Button>();
        List<CheckBox> UserCheckBoxes = new List<CheckBox>();
        List<CheckedListBox> UserCheckedListBoxes = new List<CheckedListBox>();
        List<ComboBox> UserComboBoxes = new List<ComboBox>();
        List<DateTimePicker> UserDateTimePickers = new List<DateTimePicker>();
        List<Label> UserLabels = new List<Label>();
        List<ListBox> UserListBoxes = new List<ListBox>();
        List<PictureBox> UserPictureBoxes = new List<PictureBox>();
        List<ProgressBar> UserProgressBars = new List<ProgressBar>();
        List<RadioButton> UserRadioButtons = new List<RadioButton>();
        List<RichTextBox> UserRichTextBoxes = new List<RichTextBox>();
        List<TextBox> UserTextBoxes = new List<TextBox>();

        public static Form1 f1;
        public static Form2 f2 = new Form2();
        public static Form3 f3 = new Form3();
        public static Form consoleForm = new Form();

        internal static Stack<Modify> redo = new Stack<Modify>(); // [Ctrl] + [Y]
        internal static Stack<Modify> undo = new Stack<Modify>(); // [Ctrl] + [Z]
        TreeNode? selectedItem = null;
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            f1 = this;
            f2.MdiParent = this;
            f2.Show();
            f2.Location = new Point(200, 10);
            f3.Show();
            f3.Location = new Point(this.Size.Width + this.Location.X, this.Location.Y);

            consoleForm.Show();
            consoleForm.Location = new Point(f3.Location.X, f3.Location.Y + f3.Size.Height);
            consoleForm.Size = new Size(600, 300);
            consoleForm.Text = "コンソール";
            TextBox tb = new TextBox();
            tb.Name = "debug";
            tb.Dock = DockStyle.Fill;
            tb.Enabled = false;
            tb.Multiline = true;
            consoleForm.Controls.Add(tb);

            treeView1.ExpandAll();

            toolStripStatusLabel1.Text = $"サイズ：{f2.Size.Width} x {f2.Size.Height}";
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {
            System.Diagnostics.Debug.WriteLine(selectedItem.Text);
            Create(selectedItem.Text);
        }

        public void Create(string type) {
            redo.Clear();
            switch (type) {
                case "コントロール":
                    break;
                case "Button":
                    NumButton++;
                    System.Diagnostics.Debug.WriteLine($"create Button{NumButton}");
                    UserButton ub = new UserButton();
                    f2.Controls.Add(ub);
                    UserButton ub2 = new UserButton(ub);
                    f3.Controls.Add(ub2);
                    // ub2 = PropertyCopier.CopyTo(ub, ub2);
                    UserButtons.Add(ub);
                    ub.BringToFront();
                    ub2.BringToFront();
                    List<object> before = new List<object>() { ub.Index};
                    undo.Push(new Modify(Modify.OperationCode.Create, ub, ub.FindForm(), before, before));
                    break;
                case "CheckBox":
                    NumCheckBox++;
                    System.Diagnostics.Debug.WriteLine($"create CheckBox{NumCheckBox}");
                    UserCheckBox ucb = new UserCheckBox();
                    f2.Controls.Add(ucb);
                    UserCheckBox ucb2 = new UserCheckBox(ucb);
                    f3.Controls.Add(ucb2);
                    // ucb2 = PropertyCopier.CopyTo(ucb, ucb2);
                    UserCheckBoxes.Add(ucb);
                    ucb.BringToFront();
                    ucb2.BringToFront();
                    break;
                case "CheckedListBox":
                    NumCheckedListBox++;
                    break;
                case "ComboBox":
                    NumComboBox++;
                    break;
                case "DateTimePicker":
                    NumDateTimePicker++;
                    break;
                case "Label":
                    NumLabel++;
                    break;
                case "ListBox":
                    NumListBox++;
                    break;
                case "PictureBox":
                    NumPictureBox++;
                    break;
                case "ProgressBar":
                    NumProgressBar++;
                    break;
                case "RadioButton":
                    NumRadioButton++;
                    break;
                case "RichTextBox":
                    NumRichTextBox++;
                    break;
                case "TextBox":
                    NumTextBox++;
                    break;

                default:
                    throw new Exception("存在しないコントロール");
            }
            Modify.Check(undo, redo);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {
            selectedItem = treeView1.SelectedNode;
        }


        private void beginDrag(object sender, EventArgs e) {

        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e) {
            Modify.Undo(undo, redo);
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e) {
            Modify.Redo(redo, undo);
        }
    }
}