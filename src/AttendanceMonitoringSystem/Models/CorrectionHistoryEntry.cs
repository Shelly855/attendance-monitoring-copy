namespace AttendanceMonitoringSystem.Models
{
    public class CorrectionHistoryEntry
    {
        public DateTime CorrectionDateTime { get; set; }
        public string PreviousStatus { get; set; }
        public string NewStatus { get; set; }
        public string Reason { get; set; }
        public string? Explanation { get; set; }
        public string CorrectedBy { get; set; }
    }
}