using Sessions;
namespace Course
{
    public class Course
    {
        public string Name { get; set; }
        public string Teacher { get; set; }
        public List<Session> Sessions { get; set; }
        public Course() { }
        public Course(string name, string teacher)
        {
            Name = name;
            Teacher = teacher;
        }

        public bool OverLaps(Course other)
        {
            foreach (var s1 in Sessions)
                foreach (var s2 in other.Sessions)
                    if (s1.OverLaps(s2)) return true;
            return false;

        }
    }
}