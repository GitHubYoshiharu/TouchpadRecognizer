namespace TouchpadRecognizer
{
    partial class UserSettingsForm
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            groupBox1 = new GroupBox();
            centerDiameterRatioLabel = new Label();
            centerDiameterRatioNud = new NumericUpDown();
            groupBox2 = new GroupBox();
            inactivityTimeoutMsLabel = new Label();
            inactivityTimeoutMsNud = new NumericUpDown();
            groupBox3 = new GroupBox();
            acceptableDelayMsLabel = new Label();
            acceptableDelayMsNud = new NumericUpDown();
            groupBox4 = new GroupBox();
            tapTimeThresholdMsLabel = new Label();
            tapTimeThresholdMsNud = new NumericUpDown();
            groupBox5 = new GroupBox();
            tapDistanceThresholdPxLabel = new Label();
            tapDistanceThresholdPxNud = new NumericUpDown();
            resetBtn = new Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)centerDiameterRatioNud).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)inactivityTimeoutMsNud).BeginInit();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)acceptableDelayMsNud).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tapTimeThresholdMsNud).BeginInit();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tapDistanceThresholdPxNud).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 37);
            label1.Name = "label1";
            label1.RightToLeft = RightToLeft.No;
            label1.Size = new Size(302, 40);
            label1.TabIndex = 0;
            label1.Text = "中央センサー領域の直径を、\r\nタッチパッドの短辺の何％にするかを設定できます。";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(7, 40);
            label2.Name = "label2";
            label2.Size = new Size(297, 40);
            label2.TabIndex = 1;
            label2.Text = "タッチパッドから指が全て離れてから、\r\nタップ判定を開始するまでの時間を設定できます。";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(7, 40);
            label3.Name = "label3";
            label3.Size = new Size(301, 40);
            label3.TabIndex = 2;
            label3.Text = "指が触れてから別の指が触れた場合に、\r\nそれらを同時押しとみなす時間差を設定できます。";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(7, 37);
            label4.Name = "label4";
            label4.Size = new Size(264, 40);
            label4.TabIndex = 3;
            label4.Text = "タップとみなす、指が触れてから離れるまでの\r\n最大時間を設定できます。";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(7, 40);
            label5.Name = "label5";
            label5.Size = new Size(202, 40);
            label5.TabIndex = 4;
            label5.Text = "タップとみなす、指が触れてからの\r\n最大移動距離を設定できます。";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(centerDiameterRatioLabel);
            groupBox1.Controls.Add(centerDiameterRatioNud);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(14, 16);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(319, 163);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "中央センサー領域のサイズ";
            // 
            // centerDiameterRatioLabel
            // 
            centerDiameterRatioLabel.AutoSize = true;
            centerDiameterRatioLabel.Font = new Font("Yu Gothic UI", 10F, FontStyle.Bold);
            centerDiameterRatioLabel.Location = new Point(133, 113);
            centerDiameterRatioLabel.Margin = new Padding(0);
            centerDiameterRatioLabel.Name = "centerDiameterRatioLabel";
            centerDiameterRatioLabel.RightToLeft = RightToLeft.No;
            centerDiameterRatioLabel.Size = new Size(24, 23);
            centerDiameterRatioLabel.TabIndex = 2;
            centerDiameterRatioLabel.Text = "%";
            // 
            // centerDiameterRatioNud
            // 
            centerDiameterRatioNud.Location = new Point(48, 108);
            centerDiameterRatioNud.Margin = new Padding(3, 4, 3, 4);
            centerDiameterRatioNud.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            centerDiameterRatioNud.Name = "centerDiameterRatioNud";
            centerDiameterRatioNud.Size = new Size(81, 27);
            centerDiameterRatioNud.TabIndex = 1;
            centerDiameterRatioNud.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(inactivityTimeoutMsLabel);
            groupBox2.Controls.Add(inactivityTimeoutMsNud);
            groupBox2.Controls.Add(label2);
            groupBox2.Location = new Point(14, 201);
            groupBox2.Margin = new Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 4, 3, 4);
            groupBox2.Size = new Size(319, 171);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "タップ判定時間";
            // 
            // inactivityTimeoutMsLabel
            // 
            inactivityTimeoutMsLabel.AutoSize = true;
            inactivityTimeoutMsLabel.Font = new Font("Yu Gothic UI", 10F, FontStyle.Bold);
            inactivityTimeoutMsLabel.Location = new Point(133, 123);
            inactivityTimeoutMsLabel.Margin = new Padding(0);
            inactivityTimeoutMsLabel.Name = "inactivityTimeoutMsLabel";
            inactivityTimeoutMsLabel.Size = new Size(32, 23);
            inactivityTimeoutMsLabel.TabIndex = 7;
            inactivityTimeoutMsLabel.Text = "ms";
            // 
            // inactivityTimeoutMsNud
            // 
            inactivityTimeoutMsNud.Location = new Point(48, 117);
            inactivityTimeoutMsNud.Margin = new Padding(3, 4, 3, 4);
            inactivityTimeoutMsNud.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            inactivityTimeoutMsNud.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
            inactivityTimeoutMsNud.Name = "inactivityTimeoutMsNud";
            inactivityTimeoutMsNud.Size = new Size(81, 27);
            inactivityTimeoutMsNud.TabIndex = 7;
            inactivityTimeoutMsNud.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(acceptableDelayMsLabel);
            groupBox3.Controls.Add(acceptableDelayMsNud);
            groupBox3.Controls.Add(label3);
            groupBox3.Location = new Point(14, 395);
            groupBox3.Margin = new Padding(3, 4, 3, 4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(3, 4, 3, 4);
            groupBox3.Size = new Size(319, 176);
            groupBox3.TabIndex = 7;
            groupBox3.TabStop = false;
            groupBox3.Text = "同時押し許容時間差";
            // 
            // acceptableDelayMsLabel
            // 
            acceptableDelayMsLabel.AutoSize = true;
            acceptableDelayMsLabel.Font = new Font("Yu Gothic UI", 10F, FontStyle.Bold);
            acceptableDelayMsLabel.Location = new Point(133, 128);
            acceptableDelayMsLabel.Margin = new Padding(0);
            acceptableDelayMsLabel.Name = "acceptableDelayMsLabel";
            acceptableDelayMsLabel.Size = new Size(32, 23);
            acceptableDelayMsLabel.TabIndex = 9;
            acceptableDelayMsLabel.Text = "ms";
            // 
            // acceptableDelayMsNud
            // 
            acceptableDelayMsNud.Location = new Point(48, 123);
            acceptableDelayMsNud.Margin = new Padding(3, 4, 3, 4);
            acceptableDelayMsNud.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            acceptableDelayMsNud.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
            acceptableDelayMsNud.Name = "acceptableDelayMsNud";
            acceptableDelayMsNud.Size = new Size(81, 27);
            acceptableDelayMsNud.TabIndex = 8;
            acceptableDelayMsNud.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(tapTimeThresholdMsLabel);
            groupBox4.Controls.Add(tapTimeThresholdMsNud);
            groupBox4.Controls.Add(label4);
            groupBox4.Location = new Point(368, 16);
            groupBox4.Margin = new Padding(3, 4, 3, 4);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(3, 4, 3, 4);
            groupBox4.Size = new Size(304, 163);
            groupBox4.TabIndex = 8;
            groupBox4.TabStop = false;
            groupBox4.Text = "許容接触時間";
            // 
            // tapTimeThresholdMsLabel
            // 
            tapTimeThresholdMsLabel.AutoSize = true;
            tapTimeThresholdMsLabel.Font = new Font("Yu Gothic UI", 10F, FontStyle.Bold);
            tapTimeThresholdMsLabel.Location = new Point(133, 113);
            tapTimeThresholdMsLabel.Margin = new Padding(0);
            tapTimeThresholdMsLabel.Name = "tapTimeThresholdMsLabel";
            tapTimeThresholdMsLabel.Size = new Size(32, 23);
            tapTimeThresholdMsLabel.TabIndex = 9;
            tapTimeThresholdMsLabel.Text = "ms";
            // 
            // tapTimeThresholdMsNud
            // 
            tapTimeThresholdMsNud.Location = new Point(48, 108);
            tapTimeThresholdMsNud.Margin = new Padding(3, 4, 3, 4);
            tapTimeThresholdMsNud.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            tapTimeThresholdMsNud.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
            tapTimeThresholdMsNud.Name = "tapTimeThresholdMsNud";
            tapTimeThresholdMsNud.Size = new Size(81, 27);
            tapTimeThresholdMsNud.TabIndex = 8;
            tapTimeThresholdMsNud.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(tapDistanceThresholdPxLabel);
            groupBox5.Controls.Add(tapDistanceThresholdPxNud);
            groupBox5.Controls.Add(label5);
            groupBox5.Location = new Point(368, 201);
            groupBox5.Margin = new Padding(3, 4, 3, 4);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new Padding(3, 4, 3, 4);
            groupBox5.Size = new Size(304, 171);
            groupBox5.TabIndex = 9;
            groupBox5.TabStop = false;
            groupBox5.Text = "許容移動距離";
            // 
            // tapDistanceThresholdPxLabel
            // 
            tapDistanceThresholdPxLabel.AutoSize = true;
            tapDistanceThresholdPxLabel.Font = new Font("Yu Gothic UI", 10F, FontStyle.Bold);
            tapDistanceThresholdPxLabel.Location = new Point(133, 123);
            tapDistanceThresholdPxLabel.Margin = new Padding(0);
            tapDistanceThresholdPxLabel.Name = "tapDistanceThresholdPxLabel";
            tapDistanceThresholdPxLabel.Size = new Size(29, 23);
            tapDistanceThresholdPxLabel.TabIndex = 10;
            tapDistanceThresholdPxLabel.Text = "px";
            // 
            // tapDistanceThresholdPxNud
            // 
            tapDistanceThresholdPxNud.Location = new Point(48, 117);
            tapDistanceThresholdPxNud.Margin = new Padding(3, 4, 3, 4);
            tapDistanceThresholdPxNud.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            tapDistanceThresholdPxNud.Minimum = new decimal(new int[] { 20, 0, 0, 0 });
            tapDistanceThresholdPxNud.Name = "tapDistanceThresholdPxNud";
            tapDistanceThresholdPxNud.Size = new Size(81, 27);
            tapDistanceThresholdPxNud.TabIndex = 9;
            tapDistanceThresholdPxNud.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // resetBtn
            // 
            resetBtn.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold);
            resetBtn.Location = new Point(532, 536);
            resetBtn.Margin = new Padding(3, 4, 3, 4);
            resetBtn.Name = "resetBtn";
            resetBtn.Size = new Size(140, 35);
            resetBtn.TabIndex = 11;
            resetBtn.Text = "全て既定値に戻す";
            resetBtn.UseVisualStyleBackColor = true;
            resetBtn.Click += Reset_OnClick;
            // 
            // UserSettingsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(691, 587);
            Controls.Add(resetBtn);
            Controls.Add(groupBox5);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "UserSettingsForm";
            Text = "UserSettingForm";
            FormClosing += OnFormClosing;
            Load += OnFormLoad;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)centerDiameterRatioNud).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)inactivityTimeoutMsNud).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)acceptableDelayMsNud).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tapTimeThresholdMsNud).EndInit();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tapDistanceThresholdPxNud).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private GroupBox groupBox1;
        private NumericUpDown centerDiameterRatioNud;
        private Label centerDiameterRatioLabel;
        private GroupBox groupBox2;
        private Label inactivityTimeoutMsLabel;
        private NumericUpDown inactivityTimeoutMsNud;
        private GroupBox groupBox3;
        private NumericUpDown acceptableDelayMsNud;
        private Label acceptableDelayMsLabel;
        private GroupBox groupBox4;
        private Label tapTimeThresholdMsLabel;
        private NumericUpDown tapTimeThresholdMsNud;
        private GroupBox groupBox5;
        private Label tapDistanceThresholdPxLabel;
        private NumericUpDown tapDistanceThresholdPxNud;
        private Button resetBtn;
    }
}