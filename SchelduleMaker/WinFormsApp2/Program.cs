using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp2
{
    // ===== کلاس درس =====
    public class Course
    {
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public Dictionary<string, object> FirstSession { get; set; }
        public Dictionary<string, object> SecondSession { get; set; }

        public Course(string courseName, string teacherName, Dictionary<string, object> fSession, Dictionary<string, object> sSession = null)
        {
            CourseName = courseName;
            TeacherName = teacherName;
            FirstSession = fSession;
            SecondSession = sSession;
        }

        // بررسی تداخل
        public bool Overlaps(Course other)
        {
            var sessions1 = new List<Dictionary<string, object>> { FirstSession };
            if (SecondSession != null) sessions1.Add(SecondSession);

            var sessions2 = new List<Dictionary<string, object>> { other.FirstSession };
            if (other.SecondSession != null) sessions2.Add(other.SecondSession);

            foreach (var s1 in sessions1)
            {
                foreach (var s2 in sessions2)
                {
                    string day1 = s1["Day"].ToString();
                    string day2 = s2["Day"].ToString();
                    if (day1 != day2) continue;

                    TimeSpan start1 = (TimeSpan)(s1["StartTime"]);
                    TimeSpan end1 = (TimeSpan)(s1["EndTime"]);
                    TimeSpan start2 = (TimeSpan)(s2["StartTime"]);
                    TimeSpan end2 = (TimeSpan)(s2["EndTime"]);

                    // تداخل زمانی
                    if (!(end1 <= start2 || end2 <= start1))
                        return true;
                }
            }

            return false;
        }
    }

    // ===== کلاس گزینه درس با رنگ =====
    public class CourseOption
    {
        public Course Course { get; set; }
        public string Color { get; set; }

        public CourseOption(Course course)
        {
            Course = course;
            Color = RandomColor();
        }

        private static string RandomColor()
        {
            Random rnd = new Random();
            int r = rnd.Next(60, 200);
            int g = rnd.Next(60, 200);
            int b = rnd.Next(60, 200);
            return $"#{r:X2}{g:X2}{b:X2}";
        }
    }

    // ===== کلاس Scheduler =====
    public class Scheduler
    {
        public List<CourseOption> CourseList { get; set; } = new List<CourseOption>();

        public static readonly string[] CanonicalDays =
            { "شنبه", "یکشنبه", "دوشنبه", "سه‌شنبه", "چهارشنبه", "پنج‌شنبه", "جمعه" };

        public void AddCourse(CourseOption course)
        {
            CourseList.Add(course);
        }

        public List<(string Day, string Course1, string Course2)> GetConflicts()
        {
            var conflicts = new List<(string, string, string)>();
            for (int i = 0; i < CourseList.Count; i++)
            {
                for (int j = i + 1; j < CourseList.Count; j++)
                {
                    if (CourseList[i].Course.Overlaps(CourseList[j].Course))
                    {
                        conflicts.Add((CourseList[i].Course.FirstSession["Day"].ToString(), CourseList[i].Course.CourseName, CourseList[j].Course.CourseName));
                    }
                }
            }
            return conflicts.Distinct().ToList();
        }

        // تبدیل زمان HH:MM به ساعت اعشاری
        public static bool TryParseTime(string input, out TimeSpan result)
        {
            return TimeSpan.TryParseExact(input, "hh\\:mm", null, out result);
        }
        public static string PersianToEnglishDigits(string s)
        {
            char[] persianDigits = "۰۱۲۳۴۵۶۷۸۹".ToCharArray();
            char[] englishDigits = "0123456789".ToCharArray();
            for (int i = 0; i < persianDigits.Length; i++)
                s = s.Replace(persianDigits[i], englishDigits[i]);
            return s;
        }

        // نگاشت ساعت به اسلات نیم ساعته
        public static int TimeToSlot(double hour, double startHour = 8)
        {
            return (int)Math.Round((hour - startHour) * 2);
        }
    }

    // ===== برنامه اصلی =====
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}