namespace Sessions
{
    public class Session
    {
        public string Day { get; set; }
        private TimeSpan _start;
        public TimeSpan Start
        {
            get => _start;
            set
            {
                if (value >= TimeSpan.FromHours(7) && value <= TimeSpan.FromHours(20))
                    _start = value;
                else
                    throw new ArgumentOutOfRangeException("Start Time must be between 7AM and 8PM!");
            }
        }

        private TimeSpan _end;
        public TimeSpan End
        {
            get => _end;
            set
            {
                if (value >= TimeSpan.FromHours(7) && value <= TimeSpan.FromHours(20))
                    _end = value;
                else
                    throw new ArgumentOutOfRangeException("End Time must be between 7AM and 8PM!");
            }
        }

        public Session() { }
        public Session(string day, TimeSpan start, TimeSpan end)
        {
            Day = day;
            Start = start;
            End = end;
        }

        public bool Overlaps(Session other)
        {
            if (other == null) return false;
            if (Day != other.Day) return false;
            return !(End <= other.Start || other.End <= Start);
        }
    }
}