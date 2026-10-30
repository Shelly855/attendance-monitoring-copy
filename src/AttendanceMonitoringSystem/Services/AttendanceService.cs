using AttendanceMonitoringSystem.Data;
using AttendanceMonitoringSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceMonitoringSystem.Services
{
    public class AttendanceService
    {
        private readonly AttendanceDbContext _context;

        public AttendanceService(AttendanceDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsTutorAuthorisedAsync(string userId, int attendanceId)
        {
            var attendance = await _context.Attendances.FindAsync(attendanceId);
            if (attendance == null) return false;

            return await _context.TutorSessions
                .AnyAsync(ts => ts.UserId == userId && ts.SessionId == attendance.SessionId);
        }

        public async Task CorrectAttendance(int attendanceId, string newStatus, string reason, string explanation, string userId)
        {
            var attendance = await _context.Attendances.FindAsync(attendanceId);
            if (attendance == null)
            {
                throw new InvalidOperationException("Attendance record not found.");
            }

            var correction = new AttendanceCorrection
            {
                AttendanceId = attendance.AttendanceId,
                UserId = userId,
                PreviousStatus = attendance.Status,
                NewStatus = newStatus,
                CorrectionDateTime = DateTime.Now,
                Reason = reason,
                Explanation = explanation
            };

            _context.AttendanceCorrections.Add(correction);
            attendance.Status = newStatus;

            await _context.SaveChangesAsync();
        }
    }
}