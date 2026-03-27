using System.Diagnostics;

namespace TouchpadRecognizer
{
    partial class KeyConfirmForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            backgroundLabel = new Label();
            touchAreaLabelUL = new TouchAreaLabel();
            touchAreaLabelLL = new TouchAreaLabel();
            touchAreaLabelUR = new TouchAreaLabel();
            touchAreaLabelLR = new TouchAreaLabel();
            touchAreaLabelCenter = new TouchAreaLabel();
            inputKeyTitleLabel = new Label();
            inputKeyLabel = new Label();
            filePathTBox = new TextBox();
            openFileDialogBtn = new Button();
            fileEditTBox = new TextBox();
            saveBtn = new Button();
            discardChangeBtn = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // backgroundLabel
            // 
            backgroundLabel.BackColor = Color.Black;
            backgroundLabel.Location = new Point(27, 81);
            backgroundLabel.Name = "backgroundLabel";
            backgroundLabel.Size = new Size(400, 250);
            backgroundLabel.TabIndex = 0;
            // 
            // touchAreaLabelUL
            // 
            touchAreaLabelUL.BackColor = Color.LightGray;
            touchAreaLabelUL.Location = new Point(27, 80);
            touchAreaLabelUL.Margin = new Padding(0);
            touchAreaLabelUL.Name = "touchAreaLabelUL";
            touchAreaLabelUL.Size = new Size(199, 124);
            touchAreaLabelUL.TabIndex = 1;
            touchAreaLabelUL.MouseDown += TouchArea_OnMouseDown;
            // 
            // touchAreaLabelLL
            // 
            touchAreaLabelLL.BackColor = Color.LightGray;
            touchAreaLabelLL.Location = new Point(27, 207);
            touchAreaLabelLL.Margin = new Padding(0);
            touchAreaLabelLL.Name = "touchAreaLabelLL";
            touchAreaLabelLL.Size = new Size(199, 124);
            touchAreaLabelLL.TabIndex = 2;
            touchAreaLabelLL.MouseDown += TouchArea_OnMouseDown;
            // 
            // touchAreaLabelUR
            // 
            touchAreaLabelUR.BackColor = Color.LightGray;
            touchAreaLabelUR.Location = new Point(228, 80);
            touchAreaLabelUR.Margin = new Padding(0);
            touchAreaLabelUR.Name = "touchAreaLabelUR";
            touchAreaLabelUR.Size = new Size(199, 124);
            touchAreaLabelUR.TabIndex = 3;
            touchAreaLabelUR.MouseDown += TouchArea_OnMouseDown;
            // 
            // touchAreaLabelLR
            // 
            touchAreaLabelLR.BackColor = Color.LightGray;
            touchAreaLabelLR.Location = new Point(228, 207);
            touchAreaLabelLR.Margin = new Padding(0);
            touchAreaLabelLR.Name = "touchAreaLabelLR";
            touchAreaLabelLR.Size = new Size(199, 124);
            touchAreaLabelLR.TabIndex = 4;
            touchAreaLabelLR.MouseDown += TouchArea_OnMouseDown;
            // 
            // touchAreaLabelCenter
            // 
            touchAreaLabelCenter.BackColor = Color.LightGray;
            touchAreaLabelCenter.IsCircle = true;
            touchAreaLabelCenter.Location = new Point(150, 132);
            touchAreaLabelCenter.Margin = new Padding(0);
            touchAreaLabelCenter.Name = "touchAreaLabelCenter";
            touchAreaLabelCenter.Size = new Size(150, 150);
            touchAreaLabelCenter.TabIndex = 5;
            touchAreaLabelCenter.MouseDown += TouchArea_OnMouseDown;
            // 
            // inputKeyTitleLabel
            // 
            inputKeyTitleLabel.AutoSize = true;
            inputKeyTitleLabel.Font = new Font("Yu Gothic UI", 14F, FontStyle.Bold);
            inputKeyTitleLabel.Location = new Point(27, 356);
            inputKeyTitleLabel.Name = "inputKeyTitleLabel";
            inputKeyTitleLabel.RightToLeft = RightToLeft.No;
            inputKeyTitleLabel.Size = new Size(178, 32);
            inputKeyTitleLabel.TabIndex = 6;
            inputKeyTitleLabel.Text = "入力されるキー：";
            inputKeyTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // inputKeyLabel
            // 
            inputKeyLabel.AutoSize = true;
            inputKeyLabel.Font = new Font("Yu Gothic UI", 14F, FontStyle.Bold);
            inputKeyLabel.Location = new Point(211, 356);
            inputKeyLabel.Name = "inputKeyLabel";
            inputKeyLabel.Size = new Size(55, 32);
            inputKeyLabel.TabIndex = 7;
            inputKeyLabel.Text = "無し";
            inputKeyLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // filePathTBox
            // 
            filePathTBox.Font = new Font("Yu Gothic UI", 10F);
            filePathTBox.Location = new Point(479, 32);
            filePathTBox.Margin = new Padding(3, 4, 3, 4);
            filePathTBox.MaxLength = 260;
            filePathTBox.Name = "filePathTBox";
            filePathTBox.ReadOnly = true;
            filePathTBox.Size = new Size(337, 30);
            filePathTBox.TabIndex = 8;
            filePathTBox.TabStop = false;
            filePathTBox.WordWrap = false;
            // 
            // openFileDialogBtn
            // 
            openFileDialogBtn.Font = new Font("Yu Gothic UI", 9.5F, FontStyle.Bold);
            openFileDialogBtn.Location = new Point(822, 32);
            openFileDialogBtn.Margin = new Padding(3, 4, 3, 4);
            openFileDialogBtn.Name = "openFileDialogBtn";
            openFileDialogBtn.Size = new Size(65, 33);
            openFileDialogBtn.TabIndex = 9;
            openFileDialogBtn.Text = "参照";
            openFileDialogBtn.UseVisualStyleBackColor = true;
            openFileDialogBtn.Click += OpenDialog_OnClick;
            // 
            // fileEditTBox
            // 
            fileEditTBox.AcceptsReturn = true;
            fileEditTBox.AcceptsTab = true;
            fileEditTBox.Location = new Point(478, 81);
            fileEditTBox.Margin = new Padding(3, 4, 3, 4);
            fileEditTBox.Multiline = true;
            fileEditTBox.Name = "fileEditTBox";
            fileEditTBox.ScrollBars = ScrollBars.Vertical;
            fileEditTBox.Size = new Size(409, 404);
            fileEditTBox.TabIndex = 10;
            fileEditTBox.TextChanged += EditTBox_OnTextChanged;
            // 
            // saveBtn
            // 
            saveBtn.Enabled = false;
            saveBtn.Font = new Font("Yu Gothic UI", 10F, FontStyle.Bold);
            saveBtn.Location = new Point(672, 504);
            saveBtn.Margin = new Padding(3, 4, 3, 4);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(86, 39);
            saveBtn.TabIndex = 11;
            saveBtn.Text = "保存";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += Save_OnClick;
            // 
            // discardChangeBtn
            // 
            discardChangeBtn.Enabled = false;
            discardChangeBtn.Font = new Font("Yu Gothic UI", 10F, FontStyle.Bold);
            discardChangeBtn.Location = new Point(785, 504);
            discardChangeBtn.Margin = new Padding(3, 4, 3, 4);
            discardChangeBtn.Name = "discardChangeBtn";
            discardChangeBtn.Size = new Size(102, 37);
            discardChangeBtn.TabIndex = 12;
            discardChangeBtn.Text = "変更を破棄";
            discardChangeBtn.UseVisualStyleBackColor = true;
            discardChangeBtn.Click += DiscardChange_OnClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI", 11F, FontStyle.Bold);
            label1.Location = new Point(27, 32);
            label1.Name = "label1";
            label1.Size = new Size(358, 25);
            label1.TabIndex = 13;
            label1.Text = "タップした時に入力されるキーを確認できます。";
            // 
            // KeyConfirmForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(915, 556);
            Controls.Add(label1);
            Controls.Add(discardChangeBtn);
            Controls.Add(saveBtn);
            Controls.Add(fileEditTBox);
            Controls.Add(openFileDialogBtn);
            Controls.Add(filePathTBox);
            Controls.Add(inputKeyLabel);
            Controls.Add(inputKeyTitleLabel);
            Controls.Add(touchAreaLabelCenter);
            Controls.Add(touchAreaLabelLR);
            Controls.Add(touchAreaLabelUR);
            Controls.Add(touchAreaLabelLL);
            Controls.Add(touchAreaLabelUL);
            Controls.Add(backgroundLabel);
            Font = new Font("Yu Gothic UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "KeyConfirmForm";
            Text = "KeyConfirmForm";
            FormClosing += OnFormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label backgroundLabel;
        private TouchAreaLabel touchAreaLabelUL;
        private TouchAreaLabel touchAreaLabelLL;
        private TouchAreaLabel touchAreaLabelUR;
        private TouchAreaLabel touchAreaLabelLR;
        private TouchAreaLabel touchAreaLabelCenter;
        private Label inputKeyTitleLabel;
        private Label inputKeyLabel;
        private TextBox filePathTBox;
        private Button openFileDialogBtn;
        private TextBox fileEditTBox;
        private Button saveBtn;
        private Button discardChangeBtn;
        private Label label1;
    }
}