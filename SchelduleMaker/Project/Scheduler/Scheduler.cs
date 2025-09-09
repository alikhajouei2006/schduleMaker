namespace Scheduler
{
    public class Scheduler
    {
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
    }
}