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

            var groups = courses
                .GroupBy(c => c.CourseName)
                .ToDictionary(g => g.Key, g => g.ToList());

 
            var optionsByCourseName = new List<List<CourseOption>>();
            var courseNames = new List<string>();

            foreach (var kv in groups)
            {
                string courseName = kv.Key;
                var courseRecords = kv.Value; 

                var opts = new List<CourseOption>();
                foreach (var c in courseRecords)
                {

                    var sessions = new List<Sessions.Session>();
                    if (c.FirstSession != null) sessions.Add(c.FirstSession);
                    if (c.SecondSession != null) sessions.Add(c.SecondSession);
                    opts.Add(new CourseOption(c, sessions));
                }

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