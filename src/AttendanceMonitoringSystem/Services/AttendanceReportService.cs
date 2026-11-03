using AttendanceMonitoringSystem.Data;
using AttendanceMonitoringSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceMonitoringSystem.Services
{
    public class AttendanceReportService
    {
        private readonly AttendanceDbContext _context;

        public AttendanceReportService(AttendanceDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsTutorAuthorisedForModuleAsync(string userId, int moduleId)
        {
            return await _context.TutorSessions
                .AnyAsync(ts => ts.UserId == userId && ts.Session.ModuleId == moduleId);
        }

        public async Task<AttendanceReport?> GetStudentReport(int studentId, int moduleId)
        {
            var student = await _context.Students.FindAsync(studentId);
            var module = await _context.Modules.FindAsync(moduleId);

            if (student == null || module == null) return null;

            var attendances = await _context.Attendances
                .Where(a => a.StudentId == studentId && a.Session.ModuleId == moduleId)
                .Include(a => a.AttendanceCorrections)
                .ToListAsync();

            var presentCount = attendances.Count(a => a.Status == "Present");
            var absentCount = attendances.Count(a => a.Status == "Absent");
            var authorisedAbsenceCount = attendances.Count(a => a.Status == "Authorised Absence");
            var lateArrivalCount = attendances.Count(a => a.Status == "Late Arrival");
            var leftEarlyCount = attendances.Count(a => a.Status == "Left Early");
            var scheduledSessions = attendances.Count;

            var percentage = scheduledSessions > 0
                ? Math.Round((decimal)presentCount / scheduledSessions * 100, 1)
                : 0;

            var isCompliant = percentage >= 75;

            var allCorrections = attendances.SelectMany(a => a.AttendanceCorrections).ToList();
            var correctionCount = allCorrections.Count;
            var lastCorrectionDate = allCorrections.Any() ? allCorrections.Max(c => c.CorrectionDateTime) : (DateTime?)null;

            return new AttendanceReport
            {
                StudentName = student.Name,
                ModuleName = module.Name,
                Course = module.Course,
                Semester = module.Semester,
                PresentCount = presentCount,
                AbsentCount = absentCount,
                AuthorisedAbsenceCount = authorisedAbsenceCount,
                LateArrivalCount = lateArrivalCount,
                LeftEarlyCount = leftEarlyCount,
                ScheduledSessions = scheduledSessions,
                Percentage = percentage,
                IsCompliant = isCompliant,
                CorrectionCount = correctionCount,
                LastCorrectionDate = lastCorrectionDate
            };
        }

        public async Task<List<CorrectionHistoryEntry>> GetCorrectionHistory(int studentId, int moduleId)
        {
            return await _context.AttendanceCorrections
                .Where(c => c.Attendance.StudentId == studentId && c.Attendance.Session.ModuleId == moduleId)
                .OrderByDescending(c => c.CorrectionDateTime)
                .Select(c => new CorrectionHistoryEntry
                {
                    CorrectionDateTime = c.CorrectionDateTime,
                    PreviousStatus = c.PreviousStatus,
                    NewStatus = c.NewStatus,
                    Reason = c.Reason,
                    Explanation = c.Explanation,
                    CorrectedBy = c.User.UserName
                })
                .ToListAsync();
        }
    }
}