namespace Sessions
{
    public class Session
    {
        public string Day {  get; set; }
        private TimeSpan _start;
        public TimeSpan Start
        {
            get=> _start; 
            set
            {
                if ((TimeSpan)value < TimeSpan.FromHours(20) && (TimeSpan)value > TimeSpan.FromHours(7)) 
                    _start = (TimeSpan)value;
                else 
                    throw new ArgumentOutOfRangeException("Start Time must be between 7AM until 8PM !");
            }
        }
        private TimeSpan _end;
        public TimeSpan End
        {
            get => _end;
            set
            {
                if ((TimeSpan)value < TimeSpan.FromHours(20) && (TimeSpan)value > TimeSpan.FromHours(7))
                    _end = (TimeSpan)value;
                else
                    throw new ArgumentOutOfRangeException("End Time must be between 7AM until 8PM !");
            }
        }
        public Session() { }
        public Session(string day, TimeSpan start, TimeSpan end)
        {
            Day = day;
            _end = end;
            _start = start;
        }
        public bool OverLaps(Session other)
        {
            if (other == null) return false;
            if(Day != other.Day) return false;
            return !(_end <= other.Start || other.End <= _start);
        }
    }
}