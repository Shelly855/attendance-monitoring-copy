using AttendanceMonitoringSystem.Data;
using AttendanceMonitoringSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceMonitoringSystem.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly AttendanceDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentsController(AttendanceDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int sessionId)
        {
            var userId = _userManager.GetUserId(User);

            // Resource-based check: Tutor must be authorised for this specific session
            var isAuthorised = await _context.TutorSessions
                .AnyAsync(ts => ts.UserId == userId && ts.SessionId == sessionId);

            if (!isAuthorised)
            {
                return Forbid();
            }

            var session = await _context.Sessions
                .Include(s => s.Module)
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);

            if (session == null)
            {
                return NotFound();
            }

            var students = await _context.Attendances
                .Where(a => a.SessionId == sessionId)
                .Include(a => a.Student)
                .Select(a => new StudentAttendanceViewModel
                {
                    StudentId = a.Student.StudentId,
                    StudentName = a.Student.Name,
                    AttendanceId = a.AttendanceId,
                    Status = a.Status
                })
                .ToListAsync();

            ViewData["ModuleName"] = session.Module.Name;
            ViewData["ModuleId"] = session.ModuleId;
            ViewData["SessionId"] = sessionId;
            ViewData["SessionDate"] = session.Date.ToString("dd MMM yyyy");
            ViewData["SessionTime"] = $"{session.StartTime} - {session.EndTime}";
            ViewData["SessionType"] = session.SessionType;

            return View(students);

        }
    }
}