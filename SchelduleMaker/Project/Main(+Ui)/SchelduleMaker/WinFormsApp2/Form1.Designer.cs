using System.Runtime.CompilerServices;

namespace WinFormsApp2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        private void initializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "Scheduler Maker";
            this.Text = "Scheduler Maker";
            this.ResumeLayout(false);
        }
        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnCreateSchedules = new Button();
            btnAddSession = new Button();
            txtEndSday = new TextBox();
            label7 = new Label();
            txtStartSday = new TextBox();
            label8 = new Label();
            txtEndFday = new TextBox();
            label6 = new Label();
            txtStartFday = new TextBox();
            label5 = new Label();
            cbSday = new ComboBox();
            label4 = new Label();
            cbFday = new ComboBox();
            label3 = new Label();
            txtTeacherName = new TextBox();
            label2 = new Label();
            txtcourseName = new TextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // btnCreateSessions
            // 
            btnCreateSchedules.Anchor = AnchorStyles.Top;
            btnCreateSchedules.Font = new Font("IRANSansWeb", 10F, FontStyle.Bold);
            btnCreateSchedules.Location = new Point(875, 106);
            btnCreateSchedules.Name = "btnCreateSessions";
            btnCreateSchedules.Size = new Size(197, 43);
            btnCreateSchedules.TabIndex = 55;
            btnCreateSchedules.Text = "ساخت جدول هفتگی";
            btnCreateSchedules.UseVisualStyleBackColor = true;
            btnCreateSchedules.Click += btnCreateSchedules_Click;
            // 
            // btnAddSession
            // 
            btnAddSession.Anchor = AnchorStyles.Top;
            btnAddSession.Font = new Font("IRANSansWeb", 10F, FontStyle.Bold);
            btnAddSession.Location = new Point(1078, 106);
            btnAddSession.Name = "btnAddSession";
            btnAddSession.Size = new Size(197, 43);
            btnAddSession.TabIndex = 54;
            btnAddSession.Text = "اضافه کردن جلسه(ها)";
            btnAddSession.UseVisualStyleBackColor = true;
            btnAddSession.Click += btnAddSession_Click;
            // 
            // txtEndSday
            // 
            txtEndSday.Anchor = AnchorStyles.Top;
            txtEndSday.Font = new Font("IRANSansWeb", 7.79999971F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtEndSday.Location = new Point(129, 85);
            txtEndSday.Name = "txtEndSday";
            txtEndSday.Size = new Size(108, 27);
            txtEndSday.TabIndex = 53;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top;
            label7.AutoSize = true;
            label7.Font = new Font("IRANSansWeb", 9F, FontStyle.Bold);
            label7.Location = new Point(243, 90);
            label7.Name = "label7";
            label7.Size = new Size(150, 22);
            label7.TabIndex = 52;
            label7.Text = "(HH:MM)ساعت پایان";
            // 
            // txtStartSday
            // 
            txtStartSday.Anchor = AnchorStyles.Top;
            txtStartSday.Font = new Font("IRANSansWeb", 7.79999971F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtStartSday.Location = new Point(129, 49);
            txtStartSday.Name = "txtStartSday";
            txtStartSday.Size = new Size(108, 27);
            txtStartSday.TabIndex = 51;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top;
            label8.AutoSize = true;
            label8.Font = new Font("IRANSansWeb", 9F, FontStyle.Bold);
            label8.Location = new Point(243, 54);
            label8.Name = "label8";
            label8.Size = new Size(153, 22);
            label8.TabIndex = 50;
            label8.Text = "(HH:MM)ساعت شروع";
            // 
            // txtEndFday
            // 
            txtEndFday.Anchor = AnchorStyles.Top;
            txtEndFday.Font = new Font("IRANSansWeb", 7.79999971F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtEndFday.Location = new Point(483, 88);
            txtEndFday.Name = "txtEndFday";
            txtEndFday.Size = new Size(108, 27);
            txtEndFday.TabIndex = 49;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top;
            label6.AutoSize = true;
            label6.Font = new Font("IRANSansWeb", 9F, FontStyle.Bold);
            label6.Location = new Point(597, 93);
            label6.Name = "label6";
            label6.Size = new Size(150, 22);
            label6.TabIndex = 48;
            label6.Text = "(HH:MM)ساعت پایان";
            // 
            // txtStartFday
            // 
            txtStartFday.Anchor = AnchorStyles.Top;
            txtStartFday.Font = new Font("IRANSansWeb", 7.79999971F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtStartFday.Location = new Point(483, 52);
            txtStartFday.Name = "txtStartFday";
            txtStartFday.Size = new Size(108, 27);
            txtStartFday.TabIndex = 47;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top;
            label5.AutoSize = true;
            label5.Font = new Font("IRANSansWeb", 9F, FontStyle.Bold);
            label5.Location = new Point(597, 57);
            label5.Name = "label5";
            label5.Size = new Size(153, 22);
            label5.TabIndex = 46;
            label5.Text = "(HH:MM)ساعت شروع";
            // 
            // cbSday
            // 
            cbSday.Anchor = AnchorStyles.Top;
            cbSday.Font = new Font("IRANSansWeb", 14F, FontStyle.Bold);
            cbSday.FormattingEnabled = true;
            cbSday.Items.AddRange(new object[] { "-", "شنبه", "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنجشنبه" });
            cbSday.Location = new Point(129, 4);
            cbSday.Name = "cbSday";
            cbSday.Size = new Size(143, 42);
            cbSday.TabIndex = 45;
            cbSday.UseWaitCursor = true;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top;
            label4.AutoSize = true;
            label4.Font = new Font("IRANSansWeb", 12F, FontStyle.Bold);
            label4.Location = new Point(278, 7);
            label4.Name = "label4";
            label4.Size = new Size(92, 29);
            label4.TabIndex = 44;
            label4.Text = "جلسه دوم";
            // 
            // cbFday
            // 
            cbFday.Anchor = AnchorStyles.Top;
            cbFday.Font = new Font("IRANSansWeb", 14F, FontStyle.Bold);
            cbFday.FormattingEnabled = true;
            cbFday.Items.AddRange(new object[] { "شنبه", "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنجشنبه" });
            cbFday.Location = new Point(483, 4);
            cbFday.Name = "cbFday";
            cbFday.Size = new Size(143, 42);
            cbFday.TabIndex = 43;
            cbFday.UseWaitCursor = true;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top;
            label3.AutoSize = true;
            label3.Font = new Font("IRANSansWeb", 12F, FontStyle.Bold);
            label3.Location = new Point(632, 7);
            label3.Name = "label3";
            label3.Size = new Size(89, 29);
            label3.TabIndex = 42;
            label3.Text = "جلسه اول";
            // 
            // txtTeacherName
            // 
            txtTeacherName.Anchor = AnchorStyles.Top;
            txtTeacherName.Font = new Font("IRANSansWeb", 10.7999992F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtTeacherName.Location = new Point(796, 61);
            txtTeacherName.Name = "txtTeacherName";
            txtTeacherName.Size = new Size(217, 34);
            txtTeacherName.TabIndex = 41;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.Font = new Font("IRANSansWeb", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(1019, 64);
            label2.Name = "label2";
            label2.Size = new Size(82, 29);
            label2.TabIndex = 40;
            label2.Text = "نام استاد";
            // 
            // txtcourseName
            // 
            txtcourseName.Anchor = AnchorStyles.Top;
            txtcourseName.Font = new Font("IRANSansWeb", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtcourseName.Location = new Point(796, 4);
            txtcourseName.Name = "txtcourseName";
            txtcourseName.Size = new Size(217, 37);
            txtcourseName.TabIndex = 39;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Font = new Font("IRANSansWeb", 12F, FontStyle.Bold);
            label1.Location = new Point(1019, 7);
            label1.Name = "label1";
            label1.Size = new Size(80, 29);
            label1.TabIndex = 38;
            label1.Text = "نام درس";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(1290, 980);
            Controls.Add(btnCreateSchedules);
            Controls.Add(btnAddSession);
            Controls.Add(txtEndSday);
            Controls.Add(label7);
            Controls.Add(txtStartSday);
            Controls.Add(label8);
            Controls.Add(txtEndFday);
            Controls.Add(label6);
            Controls.Add(txtStartFday);
            Controls.Add(label5);
            Controls.Add(cbSday);
            Controls.Add(label4);
            Controls.Add(cbFday);
            Controls.Add(label3);
            Controls.Add(txtTeacherName);
            Controls.Add(label2);
            Controls.Add(txtcourseName);
            Controls.Add(label1);
            Name = "Form1";
            Text = "برنامه ریزی انتخاب واحد";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnCreateSchedules;
        private Button btnAddSession;
        private TextBox txtEndSday;
        private Label label7;
        private TextBox txtStartSday;
        private Label label8;
        private TextBox txtEndFday;
        private Label label6;
        private TextBox txtStartFday;
        private Label label5;
        private ComboBox cbSday;
        private Label label4;
        private ComboBox cbFday;
        private Label label3;
        private TextBox txtTeacherName;
        private Label label2;
        private TextBox txtcourseName;
        private Label label1;
    }
}
