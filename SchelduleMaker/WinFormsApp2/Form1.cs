using System.Timers;
using System.Xml;
using WinFormsApp2;
namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private Scheduler schedulerBackend = new Scheduler();
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

        private void btnAddSession_Click(object sender, EventArgs e)
        {
            string name = txtcourseName.Text.Trim();
            string teacher = txtTeacherName.Text.Trim();
            foreach(var i in schedulerBackend.CourseList)
            {
                if(i.Course.CourseName == name)
                {
                    MessageBox.Show("این درس قبلا وارد شده است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtcourseName.Text = "";
                    return;
                }
            }
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(teacher))
            {
                MessageBox.Show("نام درس و استاد باید وارد شود.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Dictionary<string, object> session1 = new Dictionary<string, object>();
            Dictionary<string, object> session2 = new Dictionary<string, object>();

            if (cbFday.SelectedItem != null && cbFday.SelectedItem.ToString() != "-")
            {
                if (!Scheduler.TryParseTime(txtStartFday.Text, out DateTime start1) || !Scheduler.TryParseTime(txtEndFday.Text, out DateTime end1))
                {
                    MessageBox.Show("ساعت روز اول نامعتبر است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                session1["Day"] = cbFday.SelectedItem.ToString();
                session1["StartTime"] = start1;
                session1["EndTime"] = end1;
            }

            if (cbSday.SelectedItem != null && cbSday.SelectedItem.ToString() != "-")
            {
                if (!Scheduler.TryParseTime(txtStartSday.Text, out DateTime start2) || !Scheduler.TryParseTime(txtEndSday.Text, out DateTime end2))
                {
                    MessageBox.Show("ساعت روز دوم نامعتبر است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                session2["Day"] = cbSday.SelectedItem.ToString();
                session2["StartTime"] = start2;
                session2["EndTime"] = end2;
            }


            Course Course = new Course(name, teacher, session1, session2.Count > 0 ? session2 : null);
            CourseOption newCourse = new CourseOption(Course);

            foreach (var c in schedulerBackend.CourseList)
            {
                if (newCourse.Course.Overlaps(c.Course))
                {
                    MessageBox.Show($"درس جدید با درس {c.Course.CourseName} تداخل دارد.", "تداخل", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            schedulerBackend.CourseList.Add(newCourse);
            MessageBox.Show($"درس {name} اضافه شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtcourseName.Text = "";txtEndFday.Text = "";txtEndSday.Text = "";txtStartFday.Text = "";txtStartSday.Text = "";
            txtTeacherName.Text = "";cbFday.SelectedIndex = 0;cbSday.SelectedIndex = 0;
        }
    }
}
