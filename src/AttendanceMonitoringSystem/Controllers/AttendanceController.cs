using AttendanceMonitoringSystem.Data;
using AttendanceMonitoringSystem.Models;
using AttendanceMonitoringSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceMonitoringSystem.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly AttendanceDbContext _context;
        private readonly AttendanceService _attendanceService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AttendanceController(AttendanceDbContext context, AttendanceService attendanceService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _attendanceService = attendanceService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Correct(int attendanceId)
        {
            var userId = _userManager.GetUserId(User);

            if (!await _attendanceService.IsTutorAuthorisedAsync(userId, attendanceId))
            {
                return Forbid();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.AttendanceId == attendanceId);

            if (attendance == null)
            {
                return NotFound();
            }

            var model = new CorrectAttendanceViewModel
            {
                AttendanceId = attendance.AttendanceId,
                SessionId = attendance.SessionId,
                StudentName = attendance.Student.Name,
                CurrentStatus = attendance.Status
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Correct(CorrectAttendanceViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            if (!await _attendanceService.IsTutorAuthorisedAsync(userId, model.AttendanceId))
            {
                return Forbid();
            }

            if (model.Reason == "Other" && string.IsNullOrWhiteSpace(model.Explanation))
            {
                ModelState.AddModelError(nameof(model.Explanation), "Please provide an explanation.");
            }

            if (!ModelState.IsValid)
            {
                var attendance = await _context.Attendances
                    .Include(a => a.Student)
                    .FirstOrDefaultAsync(a => a.AttendanceId == model.AttendanceId);
                model.StudentName = attendance?.Student.Name;
                model.CurrentStatus = attendance?.Status;
                model.SessionId = attendance?.SessionId ?? 0;
                return View(model);
            }

            await _attendanceService.CorrectAttendance(model.AttendanceId, model.NewStatus, model.Reason, model.Explanation, userId);

            var correctedAttendance = await _context.Attendances.FindAsync(model.AttendanceId);
            return RedirectToAction("Index", "Students", new { sessionId = correctedAttendance.SessionId });
        }
    }
}