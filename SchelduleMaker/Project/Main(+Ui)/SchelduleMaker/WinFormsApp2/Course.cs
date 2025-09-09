using CourseOptions;
using Sessions;
using System.Collections.Generic;

namespace course
{
    public class Course
    {
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public Session FirstSession { get; set; }
        public Session SecondSession { get; set; }

        public Course(string name, string teacher, Session first = null, Session second = null)
        {
            CourseName = name;
            TeacherName = teacher;
            FirstSession = first;
            SecondSession = second;
        }

        public List<CourseOption> ToOptions()
        {
            var sessions = new List<Session>();
            if (FirstSession != null) sessions.Add(FirstSession);
            if (SecondSession != null) sessions.Add(SecondSession);

            return new List<CourseOption> { new CourseOption(this, sessions) };
        }
        public IEnumerable<Session> GetSessions()
        {
            if (FirstSession != null) yield return FirstSession;
            if (SecondSession != null) yield return SecondSession;
        }
    }
}