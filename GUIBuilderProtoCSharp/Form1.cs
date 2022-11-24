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
        public static Form5 f5 = new Form5();
        public static NewProjectDialog newProjectDialog = new NewProjectDialog();
        public static Form consoleForm = new Form();
        public static ConcretePropertySettingForm concretePropertyForm = new ConcretePropertySettingForm();

        public const string DESIGNER = "デザイナー";
        public const string CONSOLE = "コンソール";
        public const string CODE_EDITOR = "コードエディタ";
        public const string BLOCK_EDITOR = "ブロックエディタ";

        internal static ProjectJson pj;
        public static string workingDirectory = "";

        internal static Stack<Modify> redo = new Stack<Modify>(); // [Ctrl] + [Y]
        internal static Stack<Modify> undo = new Stack<Modify>(); // [Ctrl] + [Z]
        TreeNode? selectedItem = null;
        ControlProperties controlProperties = new ControlProperties();
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

            //f2 = new Form2();
            //f3 = new Form3();
            f2.Text = "Form - デザイン";
            f2.Size = new Size(300, 300);
            f3.Text = "Form";
            int j = f2.Controls.Count;
            f2.Controls.Clear();
            f3.Controls.Clear();

            f5.Show();
            f2.MdiParent = this;
            f2.Show();
            f2.Location = new Point(10, 10);
            f3.Show();
            f3.Location = new Point(this.Size.Width + this.Location.X, this.Location.Y);
            f4.Init();
            f4.Show();
            f4.Location = new Point(this.Size.Width + this.Location.X, this.Location.Y + f3.Size.Height);
            treeView1.Enabled = true;
            saveAsToolStripMenuItem.Enabled = true;
            reloadToolStripButton.Enabled = true;

            consoleForm.Show();
            consoleForm.Location = new Point(f1.Location.X, f1.Location.Y + f1.Size.Height);
            consoleForm.Size = new Size(600, 300);
            consoleForm.Text = "コンソール";
            TextBox tb = new TextBox();
            tb.Name = "debug";
            tb.Dock = DockStyle.Fill;
            tb.Enabled = false;
            tb.Multiline = true;
            consoleForm.Controls.Add(tb);

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


            treeView1.ExpandAll();
            SetPropView(null);

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
            propertyGrid1.SelectedObject = UserControl.GetSelectedControl();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e) {
            Modify.Redo(redo, undo);
            propertyGrid1.SelectedObject = UserControl.GetSelectedControl();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            newProjectDialog.ShowDialog();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                Form1.workingDirectory = Path.GetDirectoryName(openFileDialog1.FileName);
                StreamReader sr = new StreamReader(openFileDialog1.FileName);
                string jsonString = sr.ReadToEnd();
                pj = System.Text.Json.JsonSerializer.Deserialize<ProjectJson>(jsonString, ProjectJson.options);
                pj.Name = new string[pj.Designer.Length];
                // pj.Name[0] = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                pj.Name[0] = pj.Code[0].Split('.')[0];
                System.Diagnostics.Debug.WriteLine(pj);
                sr.Close();
                Form1.f1.Init();

                // デザインファイル読み込み
                LoadDesign();
                f5.LoadBlockCode();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            // TODO: デザインの保存をできるようにする
            DesignJson dj = new DesignJson(pj);
            dj.Name = f2.Name;
            dj.Text = f2.Text;
            dj.Size = new(f2.Size.Width, f2.Size.Height);
            // dj.Size = new int[2] { f2.Size.Width, f2.Size.Height };
            // dj.Controls = new();
            dj.Controls = new List<ControlsJson>();
            for (int i = f2.Controls.Count - 1; i >= 0; i--) {
                System.Diagnostics.Debug.WriteLine(Form1.f2.Controls[i].ToString());
                ControlsJson cj = new ControlsJson();
                //ControlsJson cj = new ControlsJson() {
                //    ControlTypeName= f2.Controls[i].GetType().Name,
                //    Name = f2.Controls[i].Name,
                //    Text = f2.Controls[i].Text,
                //    Size = new Size( f2.Controls[i].Size.Width, f2.Controls[i].Size.Height ),
                //    //Size = new int[2] { f2.Controls[i].Size.Width, f2.Controls[i].Size.Height },
                //    Location = new Point(f2.Controls[i].Location.X, f2.Controls[i].Location.Y )
                //    //Location = new int[2] { f2.Controls[i].Location.X, f2.Controls[i].Location.Y }

                //    //Anchor = f2.Controls[i].Anchor.ToString(),
                //    //AutoEllipsis = ((ButtonBase)f2.Controls[i]).AutoEllipsis,
                //    //BackColor = new int[3] { f2.Controls[i].BackColor.R, f2.Controls[i].BackColor.G, f2.Controls[i].BackColor.B },
                //    //BackgroundImage = f2.Controls[i].BackgroundImage.,
                //    //BackgroundImageLayout = f2.Controls[i].BackgroundImageLayout.ToString(),
                //    //Dock = f2.Controls[i].Dock.ToString(),
                //    //Enabled = f2.Controls[i].Enabled,
                //    //Font = f2.Controls[i].Font.ToString(),
                //    //FontHeight = f2.Controls[i].Font.Height,
                //    //ForeColor = new int[3] { f2.Controls[i].ForeColor.R, f2.Controls[i].ForeColor.G, f2.Controls[i].ForeColor.B },
                //    //Height = f2.Controls[i].Height,
                //    //Image = ((ButtonBase)f2.Controls[i]).Image.ToString(),
                //    //ImageAlign = ((ButtonBase)f2.Controls[i]).ImageAlign.ToString(),
                //    //Location = new int[2] { f2.Controls[i].Location.X, f2.Controls[i].Location.Y },
                //    //Margin = new int[4] { f2.Controls[i].Margin.Left, f2.Controls[i].Margin.Top, f2.Controls[i].Margin.Right, f2.Controls[i].Margin.Bottom },
                //    //Name = f2.Controls[i].Name,
                //    //Padding = new int[4] { f2.Controls[i].Padding.Left, f2.Controls[i].Padding.Top, f2.Controls[i].Padding.Right, f2.Controls[i].Padding.Bottom },
                //    //Size = new int[2] { f2.Controls[i].Size.Width, f2.Controls[i].Size.Height },
                //    //Text = f2.Controls[i].Text,
                //    //TextAlign = ((ButtonBase)f2.Controls[i]).TextAlign.ToString(),
                //    //Visible = f2.Controls[i].Visible,
                //    //Width = f2.Controls[i].Width,

                //};
                //dj.Controls.Add(cj);
            }
            StreamWriter sw = new StreamWriter(workingDirectory + "\\" + pj.Name[0] + GUIBuilderExtensions.Design);
            // sw.Write(System.Text.Json.JsonSerializer.Serialize(dj, ProjectJson.options));
            sw.Write(System.Text.Json.JsonSerializer.Serialize(f3, f3.GetType(), ProjectJson.options)); // FormクラスをSystem.Text.Jsonでシリアライズ
            //sw.Write(Newtonsoft.Json.JsonConvert.SerializeObject(f3,Newtonsoft.Json.Formatting.Indented, ProjectJson.newton_options)); // FormクラスをNewtonsoft.Jsonでシリアライズ
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

            //try {
            //    using (Form3 dj = System.Text.Json.JsonSerializer.Deserialize<Form3>(jsonString, ProjectJson.options)) {  // System.Text.Json
            //        f3 = dj;
            //    }
            //} catch (Exception) { }

            DesignJson dj = System.Text.Json.JsonSerializer.Deserialize<DesignJson>(jsonString, ProjectJson.options);
            for (int j = 0; j < dj.GetType().GetProperties().Length; j++) {
                string propName = dj.GetType().GetProperties()[j].Name;
                try {
                    f2.GetType().GetProperty(propName).SetValue(f2, dj.GetType().GetProperties()[j].GetValue(dj));
                    f3.GetType().GetProperty(propName).SetValue(f3, dj.GetType().GetProperties()[j].GetValue(dj));
                } catch (NullReferenceException) {
                } catch (ArgumentException) {
                }
            }
            // Fontを変えてからSizeを変えないとSizeが正しく復元されない
            f2.GetType().GetProperty("Size").SetValue(f2, dj.GetType().GetProperty("Size").GetValue(dj));
            f3.GetType().GetProperty("Size").SetValue(f3, dj.GetType().GetProperty("Size").GetValue(dj));
            f2.Text = dj.Text + " - デザイン";

            // TODO: dj.Controlsをforで回してControlTypeNameによってポリモーフィズムなインスタンスを作成
            try {
                for (int i = 0; i < dj.Controls.Count; i++) {
                    Control c;
                    Control c2;
                    if (dj.Controls[i].ControlTypeName == "UserButton") {
                        c = new UserButton();
                    } else {
                        throw new NotImplementedException();
                    }
                    for (int j = 0; j < typeof(ControlsJson).GetProperties().Length; j++) {
                        try {
                            string propName = typeof(ControlsJson).GetProperties()[j].Name;
                            var cjValue = dj.Controls[i].GetType().GetProperty(propName).GetValue(dj.Controls[i]);
                            c.GetType().GetProperty(propName).SetValue(c, cjValue);
                        } catch (ArgumentException) { }
                    }
                    if (dj.Controls[i].ControlTypeName == "UserButton") {
                        c2 = new UserButton((UserButton)c);
                    } else {
                        throw new NotImplementedException();
                    }
                    // 移動操作のためにデザイン側のコントロールを制御
                    c.Enabled = true;
                    c.Visible = true;
                    f2.Controls.Add(c);
                    f3.Controls.Add(c2);
                }
            } catch (NullReferenceException) { } // 空のフォームの場合

            //try {
            //    dj.Dispose();
            //} catch (NullReferenceException) { }
            //f3 = dj;
            // Form3 dj = Newtonsoft.Json.JsonConvert.DeserializeObject<Form3>(jsonString, ProjectJson.newton_options); // Newtonsoft.Json
            //DesignJson dj = Newtonsoft.Json.JsonConvert.DeserializeObject<DesignJson>(jsonString, ProjectJson.newton_options);
            //f2.Name = dj.Name;
            //f2.Text = dj.Text + " - デザイン";
            //f3.Text = dj.Text;
            //f2.Size = new Size(new Point(dj.Size[0], dj.Size[1]));
            //f3.Size = new Size(new Point(dj.Size[0], dj.Size[1]));
            //f2.Location = new Point(10, 10);
            //f3.Location = new Point(this.Size.Width + this.Location.X, this.Location.Y);
            //f4.Location = new Point(this.Size.Width + this.Location.X, this.Location.Y + f3.Size.Height);
            //foreach (var item in dj.Controls) {
            //    switch (item.Type) {
            //        case nameof(UserButton):
            //            UserButton ub = new UserButton();
            //            // 一度Control型でプロパティを設定し、あとでUserButton等にキャストする方法を試す
            //            //ub.Anchor = (AnchorStyles)Enum.Parse(typeof(AnchorStyles), item.Anchor); // string
            //            //ub.AutoEllipsis = item.AutoEllipsis;
            //            //ub.BackColor = Color.FromArgb(item.BackColor[0], item.BackColor[1], item.BackColor[2]);
            //            //ub.BackgroundImage = Image.FromFile(item.BackgroundImage);
            //            //ub.BackgroundImageLayout = (ImageLayout)Enum.Parse(typeof(ImageLayout), item.BackgroundImageLayout);
            //            //ub.Dock = (DockStyle)Enum.Parse(typeof(DockStyle), item.Dock);
            //            //ub.Enabled = item.Enabled;
            //            //ub.Font = new Font(item.Font[0], Single.Parse(item.Font[1])); // string[]
            //            //// ub.FontHeight protectedプロパティのため設定不可
            //            //ub.ForeColor = Color.FromArgb(item.ForeColor[0], item.ForeColor[1], item.ForeColor[2]);
            //            //ub.Height = item.Height;
            //            //ub.Image = Image.FromFile(item.Image);
            //            //ub.ImageAlign = (ContentAlignment)Enum.Parse(typeof(ContentAlignment), item.ImageAlign);
            //            ub.Location = new Point(item.Location[0], item.Location[1]);
            //            //ub.Margin = new Padding(item.Margin[0], item.Margin[1], item.Margin[2], item.Margin[3]);
            //            ub.Name = item.Name;
            //            //ub.Padding = new Padding(item.Padding[0], item.Padding[1], item.Padding[2], item.Padding[3]);
            //            ub.Size = new Size(new Point(item.Size[0], item.Size[1]));
            //            ub.Text = item.Text;
            //            //ub.TextAlign = (ContentAlignment)Enum.Parse(typeof(ContentAlignment), item.TextAlign);
            //            //ub.Visible = item.Visible;
            //            //ub.Width = item.Width;
            //            f2.Controls.Add(ub);
            //            UserButton ub2 = new UserButton(ub);
            //            f3.Controls.Add(ub2);
            //            UserButtons.Add(ub);
            //            ub.BringToFront();
            //            ub2.BringToFront();
            //            break;
            //        default:
            //            throw (new NotImplementedException());
            //    }
            //}
            sr.Close();
        }

        public void LoadDesign2() { // リフレクションを使わずにプロパティを設定する方法なので廃止
            StreamReader sr = new StreamReader(Form1.workingDirectory + "\\" + pj.Designer[0]);
            string jsonString = sr.ReadToEnd();
            DesignJson dj = System.Text.Json.JsonSerializer.Deserialize<DesignJson>(jsonString, ProjectJson.options);
            f2.Name = dj.Name;
            f2.Text = dj.Text + " - デザイン";
            f3.Text = dj.Text;
            f2.Size = new Size(new Point(dj.Size.Width, dj.Size.Height));
            f3.Size = new Size(new Point(dj.Size.Width, dj.Size.Height));
            // f2.Size = new Size(new Point(dj.Size[0], dj.Size[1]));
            // f3.Size = new Size(new Point(dj.Size[0], dj.Size[1]));
            f2.Location = new Point(10, 10);
            f3.Location = new Point(this.Size.Width + this.Location.X, this.Location.Y);
            f4.Location = new Point(this.Size.Width + this.Location.X, this.Location.Y + f3.Size.Height);
            foreach (var item in dj.Controls) {
                switch (item.ControlTypeName) {
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
                        ub.Location = new Point(item.Location.X, item.Location.Y);
                        //ub.Margin = new Padding(item.Margin[0], item.Margin[1], item.Margin[2], item.Margin[3]);
                        ub.Name = item.Name;
                        //ub.Padding = new Padding(item.Padding[0], item.Padding[1], item.Padding[2], item.Padding[3]);
                        ub.Size = new Size(new Point(item.Size.Width, item.Size.Height));
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
                        throw (new NotImplementedException());
                }
            }
            sr.Close();
        }

        private void 常に最前面に表示FToolStripMenuItem_Click(object sender, EventArgs e) {
            常に最前面に表示FToolStripMenuItem.Checked = !常に最前面に表示FToolStripMenuItem.Checked;
            f3.TopMost = 常に最前面に表示FToolStripMenuItem.Checked;
        }

        public void SetPropView(Control control) {
            propertyGrid1.SelectedObject = control;
            if (false) {
                listView1.Clear();

                System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("共通", System.Windows.Forms.HorizontalAlignment.Left);
                System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Button", System.Windows.Forms.HorizontalAlignment.Left);
                System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("CheckBox", System.Windows.Forms.HorizontalAlignment.Left);
                System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("CheckedListBox", System.Windows.Forms.HorizontalAlignment.Left);
                System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("DateTimePicker", System.Windows.Forms.HorizontalAlignment.Left);
                System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Label", System.Windows.Forms.HorizontalAlignment.Left);
                System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup("ListBox", System.Windows.Forms.HorizontalAlignment.Left);
                System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup("PictureBox", System.Windows.Forms.HorizontalAlignment.Left);
                System.Windows.Forms.ListViewGroup listViewGroup9 = new System.Windows.Forms.ListViewGroup("ProgressBar", System.Windows.Forms.HorizontalAlignment.Left);
                System.Windows.Forms.ListViewGroup listViewGroup10 = new System.Windows.Forms.ListViewGroup("RadioButton", System.Windows.Forms.HorizontalAlignment.Left);
                System.Windows.Forms.ListViewGroup listViewGroup11 = new System.Windows.Forms.ListViewGroup("RichTextBox", System.Windows.Forms.HorizontalAlignment.Left);
                System.Windows.Forms.ListViewGroup listViewGroup12 = new System.Windows.Forms.ListViewGroup("TextBox", System.Windows.Forms.HorizontalAlignment.Left);

                this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.propName, this.Value });
                {
                    listViewGroup1.Header = "共通";
                    listViewGroup1.Name = "CommonGroup";
                    listViewGroup2.Header = "Button";
                    listViewGroup2.Name = "ButtonGroup";
                    listViewGroup3.Header = "CheckBox";
                    listViewGroup3.Name = "CheckBoxGroup";
                    listViewGroup4.Header = "CheckedListBox";
                    listViewGroup4.Name = "CheckedListBoxGroup";
                    listViewGroup5.Header = "DateTimePicker";
                    listViewGroup5.Name = "DateTimePickerGroup";
                    listViewGroup6.Header = "Label";
                    listViewGroup6.Name = "LabelGroup";
                    listViewGroup7.Header = "ListBox";
                    listViewGroup7.Name = "ListBoxGroup";
                    listViewGroup8.Header = "PictureBox";
                    listViewGroup8.Name = "PictureBoxGroup";
                    listViewGroup9.Header = "ProgressBar";
                    listViewGroup9.Name = "ProgressBarGroup";
                    listViewGroup10.Header = "RadioButton";
                    listViewGroup10.Name = "RadioButtonGroup";
                    listViewGroup11.Header = "RichTextBox";
                    listViewGroup11.Name = "RichTextBoxGroup";
                    listViewGroup12.Header = "TextBox";
                    listViewGroup12.Name = "TextBoxGroup";
                }

                this.listView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
                listViewGroup1,
                listViewGroup2,
                listViewGroup3,
                listViewGroup4,
                listViewGroup5,
                listViewGroup6,
                listViewGroup7,
                listViewGroup8,
                listViewGroup9,
                listViewGroup10,
                listViewGroup11,
                listViewGroup12});
                listView1.Enabled = (control == null ? false : true);
                PropertiesFactory common = new ControlProperties();
                for (int i = 0; i < common.Count; i++) { // 共通のプロパティ
                    System.Reflection.PropertyInfo? property;
                    string[] pairs;
                    if (control == null) {
                        pairs = new string[] { common.GetProperty(i) };
                    } else {
                        try {
                            property = UserControl.GetSelectedControlType().GetProperty(common.GetProperty(i));
                            pairs = new string[] { common.GetProperty(i), property.GetValue(f2.Controls.Find(control.Name, true)[0]).ToString() };
                        } catch (NullReferenceException) {
                            pairs = new string[] { common.GetProperty(i), "null" };
                        } catch (IndexOutOfRangeException) {
                            pairs = new string[] { common.GetProperty(i), "null" };
                        }
                    }
                    ListViewItem item = new ListViewItem(pairs);
                    listView1.Items.Add(item);
                    item.Group = listViewGroup1;
                }
                PropertiesFactory? component = null;
                ListViewGroup? group = null;
                switch (UserControl.GetSelectedControl()) {
                    case UserButton ub:
                        component = new ButtonProperties();
                        group = listViewGroup2;
                        break;
                    case null:
                        return;
                    default:
                        throw new NotImplementedException();
                }
                for (int i = 0; i < component.Count; i++) { // 各GUI部品に応じたプロパティ
                    System.Reflection.PropertyInfo? property;
                    string[] pairs;
                    if (control == null) {
                        pairs = new string[] { component.GetProperty(i) };
                    } else {
                        try {
                            property = UserControl.GetSelectedControlType().GetProperty(component.GetProperty(i));
                            pairs = new string[] { component.GetProperty(i), property.GetValue(f2.Controls.Find(control.Name, true)[0]).ToString() };
                        } catch (NullReferenceException) {
                            pairs = new string[] { component.GetProperty(i), "null" };
                        } catch (IndexOutOfRangeException) {
                            pairs = new string[] { common.GetProperty(i), "null" };
                        }
                    }
                    ListViewItem item = new ListViewItem(pairs);
                    listView1.Items.Add(item);
                    item.Group = group;
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e) {
            PropertiesFactory factory;
            switch (UserControl.GetSelectedControl()) {
                case UserButton ub:
                    factory = new ButtonProperties();
                    break;
                default:
                    throw new NotImplementedException();
            }
            try {
                label1.Text = $"{((ListView)sender).FocusedItem.Text}\n{controlProperties.GetDescription(((ListView)sender).FocusedItem.Text)}";
            } catch (KeyNotFoundException) {
                label1.Text = $"{((ListView)sender).FocusedItem.Text}\n{factory.GetDescription(((ListView)sender).FocusedItem.Text)}";
            } catch (NullReferenceException) {
                label1.Text = "";
                return;
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e) {
            // ListViewの値を編集するときにテキストボックスを表示させる方法
            // https://qiita.com/Toraja/items/51dd3ec878c647583231
            ListViewItem.ListViewSubItem currentColumn = listView1.SelectedItems[0].SubItems[1];
            Rectangle rect = currentColumn.Bounds;
            rect.Intersect(listView1.ClientRectangle);
            rect.Y += 1; // 微調整 環境によっては表示が崩れる可能性がある
            rect.X += 5; // 微調整 環境によっては表示が崩れる可能性がある

            System.Reflection.PropertyInfo? property = UserControl.GetSelectedControlType().GetProperty(((ListView)sender).FocusedItem.Text);
            // Type? returnType = (Type)property.GetValue(f2.Controls.Find(UserControl.GetSelectedControl().Name, true)[0]).GetType();
            Type? returnType = property.PropertyType;
            if (returnType == typeof(string)) {
                textBox1.Visible = true;
                textBox1.Text = currentColumn.Text;
                textBox1.Bounds = rect;
                textBox1.Focus();
                textBox1.SelectAll();
            } else if (returnType == typeof(int)) {
                // NumericUpDownを使用
                numericUpDown1.Minimum = int.MinValue;
                numericUpDown1.Maximum = int.MaxValue;
                numericUpDown1.Value = int.Parse(currentColumn.Text);
                numericUpDown1.Visible = true;
                numericUpDown1.Bounds = rect;
                numericUpDown1.Focus();
                numericUpDown1.Select(0, numericUpDown1.Value.ToString().Length);
            } else if (returnType == typeof(bool)) {
                // ComboBoxを使用
                comboBox1.Items.Clear();
                System.Reflection.FieldInfo[] fields = typeof(bool).GetFields();
                for (int i = 0; i < fields.Length; i++) {
                    string? additem = fields[i].GetValue(fields[i]).ToString();
                    comboBox1.Items.Add(additem);
                    if (additem == currentColumn.Text) {
                        comboBox1.SelectedIndex = i;
                    }
                }
                comboBox1.Visible = true;
                comboBox1.Bounds = rect;
                comboBox1.Focus();
            } else if (((Type)returnType).BaseType == typeof(Enum)) {
                if (returnType == typeof(AnchorStyles)) {
                    concretePropertyForm.Controls.Clear();
                    AnchorProperty anchorProperty = new AnchorProperty(UserControl.GetSelectedControl().Anchor);
                    concretePropertyForm.Controls.Add(anchorProperty);
                    concretePropertyForm.Text = property.Name;
                    concretePropertyForm.ClientSize = new Size(anchorProperty.Size.Width, anchorProperty.Size.Height);
                    switch (concretePropertyForm.ShowDialog()) {
                        case DialogResult.Cancel:
                            break;
                        default:
                            break;
                    }
                    currentColumn.Text = anchorProperty.returnValue.ToString();
                    SetValueFromListBox(UserControl.GetSelectedControl(), property, anchorProperty.returnValue);
                } else {
                    // ComboBoxを使用
                    comboBox1.Items.Clear();
                    Array values = Enum.GetValues(returnType);
                    for (int i = 0; i < values.Length; i++) {
                        string? additem = values.GetValue(i).ToString();
                        comboBox1.Items.Add(additem);
                        if (additem == currentColumn.Text) {
                            comboBox1.SelectedIndex = i;
                        }
                    }
                    comboBox1.Visible = true;
                    comboBox1.Bounds = rect;
                    comboBox1.Focus();
                }
            } else if (returnType == typeof(Color)) {
                colorDialog1.Color = (Color)property.GetValue(UserControl.GetSelectedControl());
                switch (colorDialog1.ShowDialog()) {
                    case (DialogResult.Cancel):
                        return;
                    default:
                        break;
                }
                currentColumn.Text = colorDialog1.Color.ToString();
                SetValueFromListBox(UserControl.GetSelectedControl(), property, colorDialog1.Color);
                return;
            } else if (returnType.BaseType == typeof(ValueType)) {
                // 別のダイアログにテキストボックスを配置
                List<System.Reflection.PropertyInfo> properties = new List<System.Reflection.PropertyInfo>();
                properties = returnType.GetProperties().ToList();
                concretePropertyForm.Controls.Clear();
                ValueTypeProperty valueTypeProperty = new ValueTypeProperty();
                concretePropertyForm.Controls.Add(valueTypeProperty);
                concretePropertyForm.Text = property.Name;
                //concretePropertyForm.ClientSize = new Size(valueTypeProperty.Size.Width, valueTypeProperty.Size.Height);
                properties.RemoveAll(p => p.SetMethod == null); // Setterのないプロパティを除く
                foreach (System.Reflection.PropertyInfo p in properties) {
                    string[] pairs = { p.Name, p.GetValue(UserControl.GetSelectedControl()).ToString() };
                    ListViewItem item = new ListViewItem(pairs);
                    valueTypeProperty.listView1.Items.Add(item);
                }
                switch (concretePropertyForm.ShowDialog()) {
                    case DialogResult.Cancel:
                        break;
                    default:
                        break;
                }
                SetValueFromListBox(UserControl.GetSelectedControl(), property, valueTypeProperty.returnValue);
                currentColumn.Text = valueTypeProperty.returnValue.ToString();
            }
        }

        private void textBox1_Leave(object sender, EventArgs e) {
            textBox1.Visible = false;
            listView1.SelectedItems[0].SubItems[1].Text = textBox1.Text;
            SetValueFromListBox(UserControl.GetSelectedControl(), listView1.SelectedItems[0].Text, textBox1.Text);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) {
            // ListViewの値を編集するときにテキストボックスを表示させる方法
            // https://qiita.com/Toraja/items/51dd3ec878c647583231
            switch (e.KeyChar) {
                case (char)Keys.Enter:
                    listView1.Focus();
                    e.Handled = true;
                    break;
                case (char)Keys.Escape:
                    textBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;
                    listView1.Focus();
                    e.Handled = true;
                    break;
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e) {
            comboBox1.Visible = false;
            listView1.SelectedItems[0].SubItems[1].Text = comboBox1.SelectedItem.ToString();
            SetValueFromListBox(UserControl.GetSelectedControl(), listView1.SelectedItems[0].Text, comboBox1.SelectedItem.ToString());
        }

        private void numericUpDown1_Leave(object sender, EventArgs e) {
            numericUpDown1.Visible = false;
            listView1.SelectedItems[0].SubItems[1].Text = numericUpDown1.Value.ToString();
            SetValueFromListBox(UserControl.GetSelectedControl(), listView1.SelectedItems[0].Text, numericUpDown1.Value.ToString());
        }

        public void SetValueFromListBox(Control? control, System.Reflection.PropertyInfo? propName, object? value) {
            propName?.SetValue(UserControl.FindPreview(control), value);
            propName?.SetValue(control, value);
        }
        public void SetValueFromListBox(Control? control, string? propName, object? value) {
            System.Reflection.PropertyInfo property = UserControl.GetSelectedControlType().GetProperty(propName);
            if (property.PropertyType.BaseType == typeof(Enum)) {
                object strToEnum;
                Enum.TryParse(property.PropertyType, value.ToString(), out value);
            }
            SetValueFromListBox(control, property, Convert.ChangeType(value, property.PropertyType));
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) {
            List<object> before = new List<object>() { e.OldValue };
            List<object> after = new List<object>() { e.ChangedItem.Value };
            System.Reflection.PropertyInfo? property = propertyGrid1.SelectedObject.GetType().GetProperty(e.ChangedItem.Label);
            if (propertyGrid1.SelectedObject.GetType().BaseType == typeof(UserForm)) {
                f2.GetType().GetProperty(property.Name).SetValue(f2, f3.GetType().GetProperty(property.Name).GetValue(f3));
                undo.Push(new Modify(Modify.OperationCode.Modify, (Form)propertyGrid1.SelectedObject, f2, before, after, property));
                if (property?.Name == "Text") {
                    f2.Text += " - デザイン";
                }
            } else {
                Control selecting = UserControl.GetSelectedControl();
                Control? previewControl = null;
                if (property?.Name == "Name") {
                    previewControl = f3.Controls.Find(e.OldValue.ToString(), true)[0];
                    int cnt = 0;
                    for (int i = 0; i < f2.Controls.Count; i++) {
                        // デザイン側のFormでName被りが無いか検証
                        if (f2.Controls[i].Name.GetHashCode() == e.ChangedItem.Value.GetHashCode()) {
                            cnt++;
                        }
                        if (cnt > 1) {
                            MessageBox.Show($"名前 {f2.Controls[i].Name} は別のコンポーネントによって既に使用されています。", DESIGNER, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Control duplicatedName = ((Control)((PropertyGrid)s).SelectedObject);
                            if (duplicatedName.GetType() == typeof(UserButton)) {
                                ((UserButton)duplicatedName).Name = e.OldValue.ToString();
                                UserButton.UpdateNameManageList();
                            } else {
                                throw new NotImplementedException();
                            }
                            //((Control)((PropertyGrid)s).SelectedObject).Name = e.OldValue.ToString();
                            ((PropertyGrid)s).SelectedObject = ((PropertyGrid)s).SelectedObject;
                            return;
                        }
                    }
                } else
                    previewControl = f3.Controls.Find(selecting.Name, true)[0];
                Type? returnType = property?.PropertyType;
                SetValueFromListBox(previewControl, property, e.ChangedItem.Value);
                undo.Push(new Modify(Modify.OperationCode.Modify, propertyGrid1.SelectedObject, selecting.FindForm(), before, after, property));
            }
            redo.Clear();
            Modify.Check(undo, redo);
        }
    }
}