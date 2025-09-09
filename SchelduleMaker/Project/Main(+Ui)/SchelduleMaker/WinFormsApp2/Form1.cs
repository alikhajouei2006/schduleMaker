using Sessions;
using course;
using CourseOptions;
using scheduler;
using Utils;
using System.Globalization;
using System.Windows.Forms;
namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private Scheduler schedulerBackend = new Scheduler();
        private List<List<CourseOption>> allSchedules = new List<List<CourseOption>>();
        private int currentScheduleIndex = 0;
        private Panel panelSchedule = new Panel();
        private NumericUpDown numSchedule;
        public Form1()
        {
            InitializeComponent();
            InitializeUI();
        }
        private void InitializeUI()
        {
            // set background color
            this.BackColor = ColorTranslator.FromHtml("#ecf8f8");

            // set labels color
            Label[] labels = { label1, label2, label3, label4, label5, label6, label7, label8 };
            foreach (var lbl in labels)
                lbl.ForeColor = ColorTranslator.FromHtml("#b2967d");
            // set textboxs color
            TextBox[] textboxes = { txtcourseName, txtEndFday, txtEndSday, txtStartFday, txtStartSday, txtTeacherName };
            foreach (var tb in textboxes)
            {
                tb.BackColor = ColorTranslator.FromHtml("#eee4e1");
                tb.RightToLeft = RightToLeft.Yes;
            }

            // set buttons color
            Button[] buttons = { btnAddSession, btnCreateSchedules };
            foreach (var btn in buttons)
                btn.BackColor = ColorTranslator.FromHtml("#e6beae");

            // set comboboxs color
            cbFday.BackColor = cbSday.BackColor = ColorTranslator.FromHtml("#eee4e1");
            cbSday.BackColor = cbSday.BackColor = ColorTranslator.FromHtml("#eee4e1");

            // set panel
            panelSchedule.AutoScroll = true;
            panelSchedule.Location = new Point(20, 200);
            panelSchedule.Size = new Size(1308, 1027);
            panelSchedule.BorderStyle = BorderStyle.FixedSingle;
            panelSchedule.Anchor = AnchorStyles.Top;
            this.Controls.Add(panelSchedule);

            // set numericUpDown
            numSchedule = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 1,
                Value = 1,
                Location = new Point(panelSchedule.Left, panelSchedule.Top - 35),
                Width = 80,
                Anchor = AnchorStyles.Top
            };
            numSchedule.ValueChanged += NumSchedule_ValueChanged;
            Controls.Add(numSchedule);
        }
        private void btnAddSession_Click(object sender, EventArgs e)
        {
            string name = txtcourseName.Text.Trim();
            string teacher = txtTeacherName.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(teacher))
            {
                MessageBox.Show("نام درس و استاد باید وارد شود.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Session session1 = null;
            Session session2 = null;

            try
            {
                if (cbFday.SelectedItem != null && cbFday.SelectedItem.ToString() != "-")
                {
                    var start = TimeSpan.ParseExact(FixTime(txtStartFday.Text), "hh\\:mm", CultureInfo.InvariantCulture);
                    var end = TimeSpan.ParseExact(FixTime(txtEndFday.Text), "hh\\:mm", CultureInfo.InvariantCulture);
                    session1 = new Session(cbFday.SelectedItem.ToString(), start, end);
                }

                if (cbSday.SelectedItem != null && cbSday.SelectedItem.ToString() != "-")
                {
                    var start = TimeSpan.ParseExact(FixTime(txtStartSday.Text), "hh\\:mm", CultureInfo.InvariantCulture);
                    var end = TimeSpan.ParseExact(FixTime(txtEndSday.Text), "hh\\:mm", CultureInfo.InvariantCulture);
                    session2 = new Session(cbSday.SelectedItem.ToString(), start, end);
                }
            }
            catch
            {
                MessageBox.Show("زمان‌ها نامعتبر هستند.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Course course = new Course(name, teacher, session1, session2);
            schedulerBackend.AddCourse(course);
            MessageBox.Show($"درس {name} اضافه شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtcourseName.Text = txtTeacherName.Text = txtStartFday.Text = txtEndFday.Text = txtStartSday.Text = txtEndSday.Text = "";
            cbFday.SelectedIndex = cbSday.SelectedIndex = 0;
        }
        private string FixTime(string time) => time.Length == 4 ? "0" + time : time;
        private void btnCreateSchedules_Click(object sender, EventArgs e)
        {
            if (schedulerBackend.courses == null || schedulerBackend.courses.Count == 0)
            {
                MessageBox.Show("هیچ درسی ثبت نشده است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // تولید تمام برنامه‌های ممکن (با منطق جدید گروه‌بندی بر اساس CourseName)
            allSchedules = schedulerBackend.BuildSchedules(schedulerBackend.courses);

            if (allSchedules == null || allSchedules.Count == 0)
            {
                MessageBox.Show("هیچ برنامه‌ای بدون تداخل ساخته نشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // نمایش اولین جدول
            currentScheduleIndex = 0;
            DrawSchedule(allSchedules[currentScheduleIndex]);

            // تنظیم کنترل شماره جدول
            numSchedule.Minimum = 1;
            numSchedule.Maximum = allSchedules.Count;
            numSchedule.Value = 1;
        }
        private void NumSchedule_ValueChanged(object sender, EventArgs e)
        {
            if (allSchedules == null || allSchedules.Count == 0) return;
            currentScheduleIndex = (int)numSchedule.Value - 1;
            DrawSchedule(allSchedules[currentScheduleIndex]);
        }
        private void DrawSchedule(List<CourseOption> schedule)
        {
            panelSchedule.SuspendLayout();
            panelSchedule.Controls.Clear();

            int slotHeight = 60;
            int timeColWidth = 70;
            int startHour = 7;
            int endHour = 20;
            string[] days = { "شنبه", "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه" };
            int dayWidth = Math.Max(120, (panelSchedule.ClientSize.Width - timeColWidth - 2) / days.Length);

            // روزها (هدر)
            for (int d = 0; d < days.Length; d++)
            {
                var lblDay = new Label()
                {
                    Text = days[d],
                    Location = new Point(timeColWidth + d * dayWidth, 0),
                    Size = new Size(dayWidth, 24),
                    BorderStyle = BorderStyle.FixedSingle,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Tahoma", 10, FontStyle.Bold),
                    BackColor = Color.FromArgb(245, 247, 250)
                };
                panelSchedule.Controls.Add(lblDay);
            }

            // ستون زمان
            for (int i = 0; i <= endHour - startHour; i++)
            {
                var lblTime = new Label()
                {
                    Text = (startHour + i).ToString("00") + ":00",
                    Location = new Point(0, 24 + i * slotHeight),
                    Size = new Size(timeColWidth, slotHeight),
                    BorderStyle = BorderStyle.FixedSingle,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(250, 250, 250)
                };
                panelSchedule.Controls.Add(lblTime);
            }

            // رسم دقیقِ جلسه‌ها: از schedule (لیست CourseOption) استفاده کن،
            // و برای هر option تمام Sessions_ اش را رسم کن.
            foreach (var option in schedule)
            {
                foreach (var session in option.Sessions_)
                {
                    DrawSession(session, option.Course_.CourseName, option.Course_.TeacherName, option.Color,
                                slotHeight, dayWidth, startHour, days, timeColWidth);
                }
            }

            panelSchedule.ResumeLayout();
            int totalHeight = 24 + (endHour - startHour) * slotHeight;
            int totalWidth = timeColWidth + days.Length * dayWidth;
            panelSchedule.AutoScrollMinSize = new Size(totalWidth, totalHeight);
            panelSchedule.AutoScroll = true;
        }
        private void DrawSession(Sessions.Session session, string courseName, string teacher, string color,
    int slotHeight, int dayWidth, int startHour, string[] days, int timeColWidth)
        {
            if (session == null) return;
            string day = session.Day;
            var start = session.Start;
            var end = session.End;

            int col = Array.IndexOf(days, day);
            if (col == -1) return;

            int rowStart = (int)((start.TotalHours - startHour) * slotHeight);
            int rowEnd = (int)((end.TotalHours - startHour) * slotHeight);
            int top = 24 + rowStart;
            int height = Math.Max(26, rowEnd - rowStart);

            var btn = new Button()
            {
                Text = $"{courseName}\n{teacher}",
                Location = new Point(timeColWidth + col * dayWidth + 1, top + 1),
                Size = new Size(dayWidth - 2, height - 2),
                BackColor = ColorTranslator.FromHtml(color),
                Enabled = false,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", height < 50 ? 8.5f : 10f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btn.FlatAppearance.BorderSize = 1;
            panelSchedule.Controls.Add(btn);
        }
    }
}
