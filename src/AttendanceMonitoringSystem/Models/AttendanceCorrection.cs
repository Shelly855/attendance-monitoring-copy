using System.ComponentModel.DataAnnotations;

namespace AttendanceMonitoringSystem.Models
{
    public class AttendanceCorrection
    {
        [Key]
        public int CorrectionId { get; set; }
        public int AttendanceId { get; set; }
        public string UserId { get; set; }
        public string PreviousStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime CorrectionDateTime { get; set; }
        public string Reason { get; set; }
        public string? Explanation { get; set; }

        public Attendance Attendance { get; set; }
        public ApplicationUser User { get; set; }
    }
}