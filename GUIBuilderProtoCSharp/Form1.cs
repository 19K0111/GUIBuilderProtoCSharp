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
        public static Form4 f4 = new Form4();
        public static NewProjectDialog newProjectDialog = new NewProjectDialog();
        public static Form consoleForm = new Form();

        public const string DESIGNER = "デザイナー";
        public const string CONSOLE = "コンソール";
        public const string CODE_EDITOR = "コードエディタ";

        internal static ProjectJson pj;
        public static string workingDirectory = "";

        internal static Stack<Modify> redo = new Stack<Modify>(); // [Ctrl] + [Y]
        internal static Stack<Modify> undo = new Stack<Modify>(); // [Ctrl] + [Z]
        TreeNode? selectedItem = null;
        public Form1() {
            InitializeComponent();
        }

        public void Init() {
            NumButton = 0;
            NumCheckBox = 0;
            NumCheckedListBox = 0;
            NumComboBox = 0;
            NumDateTimePicker = 0;
            NumLabel = 0;
            NumListBox = 0;
            NumPictureBox = 0;
            NumProgressBar = 0;
            NumRadioButton = 0;
            NumRichTextBox = 0;
            NumTextBox = 0;

            UserControl.init();

            UserButton.UserButtons.Clear();

            //f2 = new Form2();
            //f3 = new Form3();
            f2.Text = "Form";
            f2.Size = new Size(300, 300);
            f3.Text = "Form - プレビュー";
            int j = f2.Controls.Count;
            f2.Controls.Clear();
            f3.Controls.Clear();

            f2.MdiParent = this;
            f2.Show();
            f2.Location = new Point(200, 10);
            f3.Show();
            f3.Location = new Point(this.Size.Width + this.Location.X, this.Location.Y);
            f4.Show();
            f4.Location = new Point(this.Size.Width + this.Location.X, this.Location.Y + f3.Size.Height);
            treeView1.Enabled = true;
            saveAsToolStripMenuItem.Enabled = true;
            reloadToolStripButton.Enabled = true;

            undo.Clear();
            redo.Clear();
            Modify.Check(undo, redo);
        }

        private void Form1_Load(object sender, EventArgs e) {
            // CharReader.Main_cr();
            // Interpreter.Lang.Main_Lang();
            // HSMAssembler.Main_asm();

            f1 = this;
            //Init();

            //UserControl.UserControls.Add(UserButton.UserButtons);

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
                case nameof(Button):
                    NumButton++;
                    System.Diagnostics.Debug.WriteLine($"create Button{UserButton.Count + 1}");
                    UserButton ub = new UserButton();
                    f2.Controls.Add(ub);
                    UserButton ub2 = new UserButton(ub);
                    f3.Controls.Add(ub2);

                    // ub2 = PropertyCopier.CopyTo(ub, ub2);
                    UserButtons.Add(ub);
                    ub.BringToFront();
                    ub2.BringToFront();
                    List<object> before = new List<object>() { ub.Index };
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

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            newProjectDialog.ShowDialog();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                Form1.workingDirectory = Path.GetDirectoryName(openFileDialog1.FileName);
                Form1.f1.Init();
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                string jsonString = sr.ReadToEnd();
                pj = System.Text.Json.JsonSerializer.Deserialize<ProjectJson>(jsonString, ProjectJson.options);
                pj.Name = new string[pj.Designer.Length];
                pj.Name[0] = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                System.Diagnostics.Debug.WriteLine(pj);
                sr.Close();

                // デザインファイル読み込み
                LoadDesign();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            // TODO: デザインの保存をできるようにする
            DesignJson dj = new DesignJson(pj);
            dj.Name = f2.Name;
            dj.Text = f2.Text;
            dj.Size = new int[2] { f2.Size.Width, f2.Size.Height };
            dj.Controls = new List<ControlsJson>();
            for (int i = f2.Controls.Count - 1; i >= 0; i--) {
                System.Diagnostics.Debug.WriteLine(Form1.f2.Controls[i].ToString());
                ControlsJson cj = new ControlsJson() {
                    Type = f2.Controls[i].GetType().Name,
                    Name = f2.Controls[i].Name,
                    Text = f2.Controls[i].Text,
                    Size = new int[2] { f2.Controls[i].Size.Width, f2.Controls[i].Size.Height },
                    Location = new int[2] { f2.Controls[i].Location.X, f2.Controls[i].Location.Y }

                    //Anchor = f2.Controls[i].Anchor.ToString(),
                    //AutoEllipsis = ((ButtonBase)f2.Controls[i]).AutoEllipsis,
                    //BackColor = new int[3] { f2.Controls[i].BackColor.R, f2.Controls[i].BackColor.G, f2.Controls[i].BackColor.B },
                    //BackgroundImage = f2.Controls[i].BackgroundImage.,
                    //BackgroundImageLayout = f2.Controls[i].BackgroundImageLayout.ToString(),
                    //Dock = f2.Controls[i].Dock.ToString(),
                    //Enabled = f2.Controls[i].Enabled,
                    //Font = f2.Controls[i].Font.ToString(),
                    //FontHeight = f2.Controls[i].Font.Height,
                    //ForeColor = new int[3] { f2.Controls[i].ForeColor.R, f2.Controls[i].ForeColor.G, f2.Controls[i].ForeColor.B },
                    //Height = f2.Controls[i].Height,
                    //Image = ((ButtonBase)f2.Controls[i]).Image.ToString(),
                    //ImageAlign = ((ButtonBase)f2.Controls[i]).ImageAlign.ToString(),
                    //Location = new int[2] { f2.Controls[i].Location.X, f2.Controls[i].Location.Y },
                    //Margin = new int[4] { f2.Controls[i].Margin.Left, f2.Controls[i].Margin.Top, f2.Controls[i].Margin.Right, f2.Controls[i].Margin.Bottom },
                    //Name = f2.Controls[i].Name,
                    //Padding = new int[4] { f2.Controls[i].Padding.Left, f2.Controls[i].Padding.Top, f2.Controls[i].Padding.Right, f2.Controls[i].Padding.Bottom },
                    //Size = new int[2] { f2.Controls[i].Size.Width, f2.Controls[i].Size.Height },
                    //Text = f2.Controls[i].Text,
                    //TextAlign = ((ButtonBase)f2.Controls[i]).TextAlign.ToString(),
                    //Visible = f2.Controls[i].Visible,
                    //Width = f2.Controls[i].Width,

                };
                dj.Controls.Add(cj);
            }
            StreamWriter sw = new StreamWriter(workingDirectory + "\\" + pj.Name[0] + DesignJson.Extension);
            sw.Write(System.Text.Json.JsonSerializer.Serialize(dj, ProjectJson.options));
            sw.Close();
        }

        private void reloadToolStripButton_Click(object sender, EventArgs e) {
            switch (MessageBox.Show($"デザイン情報を再読み込みします。保存していない変更は失われますが、よろしいですか？", DESIGNER, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)) {
                case DialogResult.Yes:
                    break;
                default:
                    return;
            }
            Form1.f1.Init();
            LoadDesign();
        }

        public void LoadDesign() {
            StreamReader sr = new StreamReader(Form1.workingDirectory + "\\" + pj.Designer[0]);
            string jsonString = sr.ReadToEnd();
            DesignJson dj = System.Text.Json.JsonSerializer.Deserialize<DesignJson>(jsonString, ProjectJson.options);
            f2.Name = dj.Name;
            f2.Text = dj.Text;
            f2.Size = new Size(new Point(dj.Size[0], dj.Size[1]));
            foreach (var item in dj.Controls) {
                switch (item.Type) {
                    case nameof(UserButton):
                        UserButton ub = new UserButton();
                        // 一度Control型でプロパティを設定し、あとでUserButton等にキャストする方法を試す
                        //ub.Anchor = (AnchorStyles)Enum.Parse(typeof(AnchorStyles), item.Anchor); // string
                        //ub.AutoEllipsis = item.AutoEllipsis;
                        //ub.BackColor = Color.FromArgb(item.BackColor[0], item.BackColor[1], item.BackColor[2]);
                        //ub.BackgroundImage = Image.FromFile(item.BackgroundImage);
                        //ub.BackgroundImageLayout = (ImageLayout)Enum.Parse(typeof(ImageLayout), item.BackgroundImageLayout);
                        //ub.Dock = (DockStyle)Enum.Parse(typeof(DockStyle), item.Dock);
                        //ub.Enabled = item.Enabled;
                        //ub.Font = new Font(item.Font[0], Single.Parse(item.Font[1])); // string[]
                        //// ub.FontHeight protectedプロパティのため設定不可
                        //ub.ForeColor = Color.FromArgb(item.ForeColor[0], item.ForeColor[1], item.ForeColor[2]);
                        //ub.Height = item.Height;
                        //ub.Image = Image.FromFile(item.Image);
                        //ub.ImageAlign = (ContentAlignment)Enum.Parse(typeof(ContentAlignment), item.ImageAlign);
                        ub.Location = new Point(item.Location[0], item.Location[1]);
                        //ub.Margin = new Padding(item.Margin[0], item.Margin[1], item.Margin[2], item.Margin[3]);
                        ub.Name = item.Name;
                        //ub.Padding = new Padding(item.Padding[0], item.Padding[1], item.Padding[2], item.Padding[3]);
                        ub.Size = new Size(new Point(item.Size[0], item.Size[1]));
                        ub.Text = item.Text;
                        //ub.TextAlign = (ContentAlignment)Enum.Parse(typeof(ContentAlignment), item.TextAlign);
                        //ub.Visible = item.Visible;
                        //ub.Width = item.Width;
                        f2.Controls.Add(ub);
                        UserButton ub2 = new UserButton(ub);
                        f3.Controls.Add(ub2);
                        UserButtons.Add(ub);
                        ub.BringToFront();
                        ub2.BringToFront();
                        break;
                    default:
                        break;
                }
            }
            sr.Close();
        }
    }
}