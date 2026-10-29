namespace AttendanceMonitoringSystem.Models
{
    public class Session
    {
        public int SessionId { get; set; }
        public int ModuleId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string SessionType { get; set; }

        public Module Module { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<TutorSession> TutorSessions { get; set; }
    }
}