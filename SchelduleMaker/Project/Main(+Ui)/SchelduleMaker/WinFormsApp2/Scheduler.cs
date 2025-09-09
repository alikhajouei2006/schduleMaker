using System.Collections.Generic;
using System.Linq;
using CourseOptions;
using course;
using Sessions;

namespace scheduler
{
    public class Scheduler
    {
        public List<Course> courses = new List<Course>();

        public void AddCourse(Course course)
        {
            if (!courses.Any(c => c.CourseName == course.CourseName && c.TeacherName == course.TeacherName
                && c.FirstSession?.Start == course.FirstSession?.Start
                && c.FirstSession?.End == course.FirstSession?.End
                && c.SecondSession?.Start == course.SecondSession?.Start
                && c.SecondSession?.End == course.SecondSession?.End))
            {
                courses.Add(course);
            }
        }

        public List<List<CourseOption>> BuildSchedules(List<Course> courses)
        {
            // گروه‌بندی کورس‌ها بر اساس اسم (هر گروه = یک درس با چند گزینه مختلف)
            var groups = courses
                .GroupBy(c => c.CourseName)
                .ToDictionary(g => g.Key, g => g.ToList());

            // برای هر گروه، لیست CourseOptionها را تولید می‌کنیم:
            // هر رکورد Course در گروه تبدیل به یک CourseOption می‌شود (هر رکورد خودش یک گزینه است).
            var optionsByCourseName = new List<List<CourseOption>>();
            var courseNames = new List<string>(); // ترتیب ثابت برای backtracking

            foreach (var kv in groups)
            {
                string courseName = kv.Key;
                var courseRecords = kv.Value; // List<Course>

                var opts = new List<CourseOption>();
                foreach (var c in courseRecords)
                {
                    // sessions: اگر هر کدام موجود باشد اضافه کن (یکی یا دو تا)
                    var sessions = new List<Sessions.Session>();
                    if (c.FirstSession != null) sessions.Add(c.FirstSession);
                    if (c.SecondSession != null) sessions.Add(c.SecondSession);
                    opts.Add(new CourseOption(c, sessions));
                }

                // اگر هیچ گزینه‌ای نیست (نادر؛ اما ایمن باشیم) به جای خالی یک لیست اضافه نکن
                if (opts.Count > 0)
                {
                    optionsByCourseName.Add(opts);
                    courseNames.Add(courseName);
                }
            }

            var result = new List<List<CourseOption>>();
            var current = new List<CourseOption>();

            void Backtrack(int idx)
            {
                if (idx == optionsByCourseName.Count)
                {
                    // رسیدیم به یک ترکیب معتبر (current) -> اضافه کن
                    result.Add(new List<CourseOption>(current));
                    return;
                }

                foreach (var opt in optionsByCourseName[idx])
                {
                    if (!ConflictsWithCurrent(current, opt))
                    {
                        current.Add(opt);
                        Backtrack(idx + 1);
                        current.RemoveAt(current.Count - 1);
                    }
                }
            }

            Backtrack(0);
            return result;
        }

        private bool ConflictsWithCurrent(List<CourseOption> current, CourseOption candidate)
        {
            foreach (var existing in current)
            {
                foreach (var s1 in existing.Sessions_)
                    foreach (var s2 in candidate.Sessions_)
                    {
                        if (s1.Day != s2.Day) continue;
                        bool overlap = !(s1.End <= s2.Start || s2.End <= s1.Start);
                        if (overlap) return true;
                    }
            }
            return false;
        }
    }
}