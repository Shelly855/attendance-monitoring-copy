namespace AttendanceMonitoringSystem.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public int StudentId { get; set; }
        public int SessionId { get; set; }
        public string Status { get; set; }

        public Student Student { get; set; }
        public Session Session { get; set; }
        public ICollection<AttendanceCorrection> AttendanceCorrections { get; set; }
    }
}