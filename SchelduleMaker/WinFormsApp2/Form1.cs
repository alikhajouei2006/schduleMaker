namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml("#ecf8f8");
            label1.ForeColor = ColorTranslator.FromHtml("#b2967d");
            label2.ForeColor = ColorTranslator.FromHtml("#b2967d");
            label3.ForeColor = ColorTranslator.FromHtml("#b2967d");
            label4.ForeColor = ColorTranslator.FromHtml("#b2967d");
            label5.ForeColor = ColorTranslator.FromHtml("#b2967d");
            label6.ForeColor = ColorTranslator.FromHtml("#b2967d");
            label7.ForeColor = ColorTranslator.FromHtml("#b2967d");
            label8.ForeColor = ColorTranslator.FromHtml("#b2967d");
            txtcourseName.BackColor = ColorTranslator.FromHtml("#eee4e1");
            txtEndFday.BackColor = ColorTranslator.FromHtml("#eee4e1");
            txtEndSday.BackColor = ColorTranslator.FromHtml("#eee4e1");
            txtStartFday.BackColor = ColorTranslator.FromHtml("#eee4e1");
            txtStartSday.BackColor = ColorTranslator.FromHtml("#eee4e1");
            txtTeacherName.BackColor = ColorTranslator.FromHtml("#eee4e1");
            btnAddSession.BackColor = ColorTranslator.FromHtml("#e6beae");
            btnCreateSessions.BackColor = ColorTranslator.FromHtml("#e6beae");
            cbFday.BackColor = ColorTranslator.FromHtml("#eee4e1");
            cbSday.BackColor = ColorTranslator.FromHtml("#eee4e1");
            txtcourseName.RightToLeft = RightToLeft.Yes;
            txtEndFday.RightToLeft = RightToLeft.Yes;
            txtEndSday.RightToLeft = RightToLeft.Yes;
            txtStartFday.RightToLeft = RightToLeft.Yes;
            txtStartSday.RightToLeft = RightToLeft.Yes;
            txtTeacherName.RightToLeft = RightToLeft.Yes;
           

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
