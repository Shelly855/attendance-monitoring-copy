using System.ComponentModel.DataAnnotations;

namespace AttendanceMonitoringSystem.Models
{
    public class CorrectAttendanceViewModel
    {
        public int AttendanceId { get; set; }
        public int SessionId { get; set; }
        public string StudentName { get; set; }
        public string CurrentStatus { get; set; }

        [Required]
        public string NewStatus { get; set; }

        [Required]
        public string Reason { get; set; }

        public string? Explanation { get; set; }
    }
}