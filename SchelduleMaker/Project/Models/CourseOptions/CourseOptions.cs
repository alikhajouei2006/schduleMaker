namespace Course
{
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
}