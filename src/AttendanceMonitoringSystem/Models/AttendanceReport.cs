namespace AttendanceMonitoringSystem.Models
{
    public class AttendanceReport
    {
        public string StudentName { get; set; } = string.Empty;
        public string ModuleName { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int AuthorisedAbsenceCount { get; set; }
        public int LateArrivalCount { get; set; }
        public int LeftEarlyCount { get; set; }
        public int ScheduledSessions { get; set; }
        public decimal Percentage { get; set; }
        public bool IsCompliant { get; set; }
        public int CorrectionCount { get; set; }
        public DateTime? LastCorrectionDate { get; set; }
    }
}