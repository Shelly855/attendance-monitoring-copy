namespace AttendanceMonitoringSystem.Models
{
    public class TutorSession
    {
        public int TutorSessionId { get; set; }
        public string UserId { get; set; }
        public int SessionId { get; set; }

        public ApplicationUser User { get; set; }
        public Session Session { get; set; }
    }
}