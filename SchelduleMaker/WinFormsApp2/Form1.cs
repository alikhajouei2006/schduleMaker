using System.Configuration.Internal;
using System.Globalization;
using System.Timers;
using System.Xml;
using WinFormsApp2;
namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private Scheduler schedulerBackend = new Scheduler();
        private List<List<CourseOption>> _validSchedule = new List<List<CourseOption>>();
        Panel panelSchedule = new Panel();
        private NumericUpDown numSchedule;
        private int _currentIndex = 0;
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
            panelSchedule.AutoScroll = true;
            panelSchedule.Location = new Point(20, 200);
            panelSchedule.Size = new Size(1308, 1027);    
            panelSchedule.BorderStyle = BorderStyle.FixedSingle;
            panelSchedule.Anchor = AnchorStyles.Top;
            this.Controls.Add(panelSchedule);
            numSchedule = new NumericUpDown()
            {
                Minimum = 1,
                Maximum = 1,
                Value = 1,
                Location = new Point(panelSchedule.Left, panelSchedule.Top - 35),
                Width = 80
            };
            numSchedule.ValueChanged += numSchedule_ValueChanged;
            numSchedule.Anchor = AnchorStyles.Top;
            Controls.Add(numSchedule);
        }

        private void numSchedule_ValueChanged(object sender, EventArgs e)
        {
            if (_validSchedule.Count == 0) return;
            _currentIndex = (int)numSchedule.Value - 1;
            DisplaySchedule(_validSchedule[_currentIndex]);
        }

        // برگرداندن لیست جلسات یک Course به‌صورت امن
        private static IEnumerable<Dictionary<string, object>> GetSessions(Course c)
        {
            if (c.FirstSession != null && c.FirstSession.Count >= 3) yield return c.FirstSession;
            if (c.SecondSession != null && c.SecondSession.Count >= 3) yield return c.SecondSession;
        }

        // چک تداخل بین یک گزینه جدید و گزینه‌های انتخاب‌شده تا اینجا
        private static bool HasConflict(List<CourseOption> current, CourseOption candidate)
        {
            foreach (var co in current)
            {
                foreach (var s1 in GetSessions(co.Course))
                    foreach (var s2 in GetSessions(candidate.Course))
                    {
                        string d1 = s1["Day"].ToString();
                        string d2 = s2["Day"].ToString();
                        if (d1 != d2) continue;

                        var a1 = (TimeSpan)s1["StartTime"];
                        var b1 = (TimeSpan)s1["EndTime"];
                        var a2 = (TimeSpan)s2["StartTime"];
                        var b2 = (TimeSpan)s2["EndTime"];

                        if (!(b1 <= a2 || b2 <= a1)) return true; // overlap
                    }
            }
            return false;
        }

        // تولید همهٔ برنامه‌های ممکن بدون تداخل
        private List<List<CourseOption>> GenerateValidSchedules()
        {
            // گروه‌بندی: نام درس -> لیست گزینه‌ها
            var groups = schedulerBackend.CourseList
                .GroupBy(o => o.Course.CourseName)
                .ToDictionary(g => g.Key, g => g.ToList());
            const int Limit = 500;

            var courseNames = groups.Keys.ToList();
            var result = new List<List<CourseOption>>();
            var current = new List<CourseOption>();
            if(result.Count >= Limit) return result;
            // اگر گزینهٔ یک درس صفر بود، برنامه‌ای ساخته نمی‌شود
            if (courseNames.Any(name => groups[name].Count == 0))
                return result;

            void DFS(int idx)
            {
                if (idx == courseNames.Count)
                {
                    result.Add(new List<CourseOption>(current));
                    return;
                }

                string cname = courseNames[idx];
                foreach (var opt in groups[cname])
                {
                    if (!HasConflict(current, opt))
                    {
                        current.Add(opt);
                        DFS(idx + 1);
                        current.RemoveAt(current.Count - 1);
                    }
                }
            }

            DFS(0);
            return result;
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

            Dictionary<string, object> session1 = new Dictionary<string, object>();
            Dictionary<string, object> session2 = new Dictionary<string, object>();

            if (cbFday.SelectedItem != null && cbFday.SelectedItem.ToString() != "-")
            {
                session1["Day"] = cbFday.SelectedItem.ToString();
                try
                {
                    string time;
                    if (txtStartFday.Text.Length == 4)
                    {
                        time = "0" + txtStartFday.Text;
                    }
                    else
                    {
                        time = txtStartFday.Text;
                    }
                    session1["StartTime"] = TimeSpan.ParseExact(time, "hh\\:mm", CultureInfo.InvariantCulture);
                }
                catch
                {
                    MessageBox.Show("ساعت شروع روز اول نامعتبر است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    string time;
                    if (txtEndFday.Text.Length == 4)
                    {
                        time = "0" + txtEndFday.Text;
                    }
                    else
                    {
                        time = txtEndFday.Text;
                    }
                    session1["EndTime"] = TimeSpan.ParseExact(time, "hh\\:mm", CultureInfo.InvariantCulture);
                }
                catch
                {
                    MessageBox.Show("ساعت پایان روز اول نامعتبر است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if ((TimeSpan)session1["StartTime"] >= (TimeSpan)session1["EndTime"] )
                {
                    MessageBox.Show(".ساعت پایان روزاول نمی‌تواند کمتر از ساعت شروع باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
            }
            
            if (cbSday.SelectedItem != null && cbSday.SelectedItem.ToString() != "-")
            {
                session2["Day"] = cbSday.SelectedItem.ToString();
                try
                {
                    string time;
                    if(txtStartSday.Text.Length ==  4 )
                    {
                        time = "0" + txtStartSday.Text;
                    }
                    else
                    {
                        time = txtStartSday.Text;
                    }
                        session2["StartTime"] = TimeSpan.ParseExact(time, "hh\\:mm", CultureInfo.InvariantCulture);
                }
                catch
                {
                    MessageBox.Show("ساعت شروع روز دوم نامعتبر است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    string time;
                    if (txtEndSday.Text.Length == 4)
                    {
                        time = "0" + txtEndSday.Text;
                    }
                    else
                    {
                        time = txtEndSday.Text;
                    }
                    session2["EndTime"] = TimeSpan.ParseExact(time, "hh\\:mm", CultureInfo.InvariantCulture);
                }
                catch
                {
                    MessageBox.Show("ساعت پایان روز دوم نامعتبر است.", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if ((TimeSpan)session2["StartTime"] >= (TimeSpan)session2["EndTime"])
                {
                    MessageBox.Show(".ساعت پایان روز دوم نمی‌تواند کمتر از ساعت شروع باشد", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
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
            txtcourseName.Text = ""; txtEndFday.Text = ""; txtEndSday.Text = ""; txtStartFday.Text = ""; txtStartSday.Text = "";
            txtTeacherName.Text = ""; cbFday.SelectedIndex = 0; cbSday.SelectedIndex = 0;
        }

        private void btnCreateSessions_Click(object sender, EventArgs e)
        {
            if (schedulerBackend.CourseList.Count == 0)
            {
                MessageBox.Show("هیچ گزینه‌ای ثبت نشده است.");
                return;
            }

            _validSchedule = GenerateValidSchedules();
            if (_validSchedule.Count == 0)
            {
                MessageBox.Show("هیچ برنامهٔ بدون تداخل یافت نشد.");
                return;
            }

            numSchedule.Maximum = _validSchedule.Count;
            numSchedule.Value = 1; // این خودش DisplaySchedule را صدا می‌زند
            DisplaySchedule(_validSchedule[0]);
        }
        private void DrawSession(
    Dictionary<string, object> session, string courseName, string teacher, string color,
    int slotHeight, int dayWidth, int startHour, string[] days, int timeColWidth)
        {
            if (!session.ContainsKey("Day") || !session.ContainsKey("StartTime") || !session.ContainsKey("EndTime"))
                return;

            string day = session["Day"].ToString();
            var start = (TimeSpan)session["StartTime"];
            var end = (TimeSpan)session["EndTime"];

            int col = Array.IndexOf(days, day);
            if (col == -1) return;

            int rowStart = (int)((start.TotalHours - startHour) * slotHeight);
            int rowEnd = (int)((end.TotalHours - startHour) * slotHeight);
            int top = 24 + rowStart;
            int height = Math.Max(26, rowEnd - rowStart); // حداقل ارتفاع

            var btn = new Button()
            {
                Text = $"{courseName}\n{teacher}",
                Location = new Point(timeColWidth + col * dayWidth + 1, top + 1),
                Size = new Size(dayWidth - 2, height - 2),
                BackColor = ColorTranslator.FromHtml(color),
                Enabled = false,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", height < 50 ? 8.5f : 10.0f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            btn.FlatAppearance.BorderSize = 1;
            panelSchedule.Controls.Add(btn);
        }
        private void DisplaySchedule(List<CourseOption> schedule)
        {
            panelSchedule.SuspendLayout();
            panelSchedule.Controls.Clear();

            int slotHeight = 60;     // بزرگ‌تر و خواناتر
            int timeColWidth = 70;
            string[] days = { "شنبه", "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه" };
            int startHour = 7;
            int endHour = 20;

            // محاسبه عرض ستون‌های روز بر اساس عرض پنل
            int dayWidth = Math.Max(120, (panelSchedule.ClientSize.Width - timeColWidth - 2) / days.Length);

            // عنوان روزها
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

            // ستون زمان‌ها
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

            // کشیدن جلسات
            foreach (var option in schedule)
            {
                var course = option.Course;
                if (course.FirstSession != null) DrawSession(course.FirstSession, course.CourseName, course.TeacherName, option.Color, slotHeight, dayWidth, startHour, days, timeColWidth);
                if (course.SecondSession != null) DrawSession(course.SecondSession, course.CourseName, course.TeacherName, option.Color, slotHeight, dayWidth, startHour, days, timeColWidth);
            }

            panelSchedule.ResumeLayout();
            int total_hight = 24 + (endHour - startHour) * slotHeight;
            int total_width = timeColWidth + days.Length * dayWidth;
            panelSchedule.AutoScrollMinSize = new Size(total_width, total_hight);
            panelSchedule.AutoScroll = true;
        }

    }
}