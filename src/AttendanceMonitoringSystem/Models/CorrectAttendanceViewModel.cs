using System.ComponentModel.DataAnnotations;

namespace AttendanceMonitoringSystem.Models
{
    public class CorrectAttendanceViewModel : IValidatableObject
    {
        public int AttendanceId { get; set; }
        public int SessionId { get; set; }
        public string? StudentName { get; set; }
        public string? CurrentStatus { get; set; }

        [Display(Name = "New status")]
        [Required(ErrorMessage = "Please select a new status.")]
        public string NewStatus { get; set; }

        [Required(ErrorMessage = "Please select a reason.")]
        public string Reason { get; set; }

        public string? Explanation { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Reason == "Other" && string.IsNullOrWhiteSpace(Explanation))
            {
                yield return new ValidationResult(
                    "Please enter an explanation.",
                    new[] { nameof(Explanation) });
            }
        }
    }
}