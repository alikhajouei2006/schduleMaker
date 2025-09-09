using System;
using System.Collections.Generic;
using Sessions;
using course;

namespace CourseOptions
{
    public class CourseOption
    {
        public Course Course_ { get; set; }
        public List<Session> Sessions_ { get; set; }
        public string Color { get; set; }

        public CourseOption(Course course, List<Session> sessions)
        {
            Course_ = course;
            Sessions_ = sessions;
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
}