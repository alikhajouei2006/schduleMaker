using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private Scheduler schedulerBackend = new Scheduler();
        private Dictionary<string, List<CourseOption>> courseByName = new Dictionary<string, List<CourseOption>>();
        private Panel panelSchedule = new Panel();
        private NumericUpDown numSchedule;
        private int currentScheduleIndex = 0;
        private List<List<CourseOption>> allSchedules = new List<List<CourseOption>>();

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.BackColor = ColorTranslator.FromHtml("#ecf8f8");

            // نمونه رنگ‌بندی Label ها
            Label[] labels = { label1, label2, label3, label4, label5, label6, label7, label8 };
            foreach (var lbl in labels)
                lbl.ForeColor = ColorTranslator.FromHtml("#b2967d");

            // رنگ TextBox ها
            TextBox[] textboxes = { txtcourseName, txtEndFday, txtEndSday, txtStartFday, txtStartSday, txtTeacherName };
            foreach (var tb in textboxes)
            {
                tb.BackColor = ColorTranslator.FromHtml("#eee4e1");
                tb.RightToLeft = RightToLeft.Yes;
            }

            // رنگ دکمه‌ها
            Button[] buttons = { btnAddSession, btnCreateSessions };
            foreach (var btn in buttons)
                btn.BackColor = ColorTranslator.FromHtml("#e6beae");

            // رنگ ComboBox ها
            cbFday.BackColor = cbSday.BackColor = ColorTranslator.FromHtml("#eee4e1");

            // Panel جدول برنامه
            panelSchedule.AutoScroll = true;
            panelSchedule.Location = new Point(20, 200);
            panelSchedule.Size = new Size(1308, 1027);
            panelSchedule.BorderStyle = BorderStyle.FixedSingle;
            panelSchedule.Anchor = AnchorStyles.Top;
            this.Controls.Add(panelSchedule);

            // NumericUpDown برای جابجایی بین برنامه‌ها
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

        private void NumSchedule_ValueChanged(object sender, EventArgs e)
        {
            if (allSchedules == null || allSchedules.Count == 0) return;
            currentScheduleIndex = (int)numSchedule.Value - 1;
            DrawSchedule(allSchedules[currentScheduleIndex]);
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

            Dictionary<string, object> session1 = GetSession(cbFday, txtStartFday, txtEndFday, "روز اول");
            if (session1 == null) return;
            Dictionary<string, object> session2 = GetSession(cbSday, txtStartSday, txtEndSday, "روز دوم");

            Course course = new Course(name, teacher, session1, session2.Count > 0 ? session2 : null);
            CourseOption newCourse = new CourseOption(course);

            if (!courseByName.ContainsKey(name))
                courseByName[name] = new List<CourseOption>();
            courseByName[name].Add(newCourse);

            MessageBox.Show($"درس {name} اضافه شد.", "موفق", MessageBoxButtons.OK, MessageBoxIcon.Information);// پاک کردن فرم
            txtcourseName.Text = ""; txtEndFday.Text = ""; txtEndSday.Text = ""; txtStartFday.Text = ""; txtStartSday.Text = "";
            txtTeacherName.Text = ""; cbFday.SelectedIndex = 0; cbSday.SelectedIndex = 0;
        }

        private Dictionary<string, object> GetSession(ComboBox cbDay, TextBox txtStart, TextBox txtEnd, string label)
        {
            Dictionary<string, object> session = new Dictionary<string, object>();
            if (cbDay.SelectedItem == null || cbDay.SelectedItem.ToString() == "-") return session;

            session["Day"] = cbDay.SelectedItem.ToString();

            try
            {
                session["StartTime"] = ParseTime(txtStart.Text);
            }
            catch
            {
                MessageBox.Show($"ساعت شروع {label} نامعتبر است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            try
            {
                session["EndTime"] = ParseTime(txtEnd.Text);
            }
            catch
            {
                MessageBox.Show($"ساعت پایان {label} نامعتبر است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            if ((TimeSpan)session["StartTime"] >= (TimeSpan)session["EndTime"])
            {
                MessageBox.Show($"ساعت پایان {label} نمی‌تواند کمتر از ساعت شروع باشد.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return session;
        }

        private TimeSpan ParseTime(string text)
        {
            string time = text.Length == 4 ? "0" + text : text;
            return TimeSpan.ParseExact(time, "hh\\:mm", CultureInfo.InvariantCulture);
        }

        private void btnCreateSessions_Click(object sender, EventArgs e)
        {
            if (courseByName.Count == 0)
            {
                MessageBox.Show("هیچ گزینه‌ای ثبت نشده است.");
                return;
            }

            allSchedules = BuildAllSchedules(courseByName);
            if (allSchedules.Count == 0)
            {
                MessageBox.Show("هیچ برنامهٔ بدون تداخل یافت نشد.");
                return;
            }

            currentScheduleIndex = 0;
            numSchedule.Minimum = 1;
            numSchedule.Maximum = allSchedules.Count;
            numSchedule.Value = 1;
            DrawSchedule(allSchedules[0]);
        }

        public List<List<CourseOption>> BuildAllSchedules(Dictionary<string, List<CourseOption>> coursesByName)
        {
            List<List<CourseOption>> allSchedules = new List<List<CourseOption>>();
            List<string> courseNames = coursesByName.Keys.ToList();
            List<CourseOption> currentSchedule = new List<CourseOption>();

            void Backtrack(int index)
            {
                if (index == courseNames.Count)
                {
                    allSchedules.Add(new List<CourseOption>(currentSchedule));
                    return;
                }

                string courseName = courseNames[index];
                foreach (var option in coursesByName[courseName])
                {
                    if (!HasConflict(currentSchedule, option))
                    {
                        currentSchedule.Add(option);
                        Backtrack(index + 1);
                        currentSchedule.RemoveAt(currentSchedule.Count - 1);
                    }
                }
            }

            Backtrack(0);
            return allSchedules;
        }

        private bool HasConflict(List<CourseOption> current, CourseOption candidate)
        {
            foreach (var co in current)
            {
                foreach (var s1 in GetSessions(co.Course))
                    foreach (var s2 in GetSessions(candidate.Course))
                    {
                        string d1 = s1["Day"].ToString();
                        string d2 = s2["Day"].ToString();
                        if (d1 != d2) continue; var a1 = (TimeSpan)s1["StartTime"];
                        var b1 = (TimeSpan)s1["EndTime"];
                        var a2 = (TimeSpan)s2["StartTime"];
                        var b2 = (TimeSpan)s2["EndTime"];

                        if (!(b1 <= a2 || b2 <= a1)) return true;
                    }
            }
            return false;
        }

        private IEnumerable<Dictionary<string, object>> GetSessions(Course c)
        {
            if (c.FirstSession != null && c.FirstSession.Count >= 3) yield return c.FirstSession;
            if (c.SecondSession != null && c.SecondSession.Count >= 3) yield return c.SecondSession;
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

            // روزها
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

            // جلسات
            foreach (var option in schedule)
            {
                var course = option.Course;
                if (course.FirstSession != null)
                    DrawSession(course.FirstSession, course.CourseName, course.TeacherName, option.Color, slotHeight, dayWidth, startHour, days, timeColWidth);
                if (course.SecondSession != null)
                    DrawSession(course.SecondSession, course.CourseName, course.TeacherName, option.Color, slotHeight, dayWidth, startHour, days, timeColWidth);
            }

            panelSchedule.ResumeLayout();
            int totalHeight = 24 + (endHour - startHour) * slotHeight;
            int totalWidth = timeColWidth + days.Length * dayWidth;
            panelSchedule.AutoScrollMinSize = new Size(totalWidth, totalHeight);
            panelSchedule.AutoScroll = true;
        }

        private void DrawSession(Dictionary<string, object> session, string courseName, string teacher, string color,
            int slotHeight, int dayWidth, int startHour, string[] days, int timeColWidth)
        {
            if (!session.ContainsKey("Day") || !session.ContainsKey("StartTime") || !session.ContainsKey("EndTime"))
                return;

            string day = session["Day"].ToString();
            var start = (TimeSpan)session["StartTime"];
            var end = (TimeSpan)session["EndTime"];
            int col = Array.IndexOf(days, day);
            if (col == -1) return; int rowStart = (int)((start.TotalHours - startHour) * slotHeight);
            int rowEnd = (int)((end.TotalHours - startHour) * slotHeight);
            int top = 24 + rowStart;
            int height = Math.Max(26, rowEnd - rowStart);

            var btn = new Button
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