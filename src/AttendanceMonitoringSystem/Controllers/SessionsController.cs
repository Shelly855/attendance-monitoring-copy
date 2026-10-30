using AttendanceMonitoringSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceMonitoringSystem.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        private readonly AttendanceDbContext _context;
        private readonly UserManager<Models.ApplicationUser> _userManager;

        public SessionsController(AttendanceDbContext context, UserManager<Models.ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var sessions = await _context.Sessions
                .Include(s => s.Module)
                .Where(s => s.TutorSessions.Any(ts => ts.UserId == userId))
                .OrderBy(s => s.Date)
                .ThenBy(s => s.StartTime)
                .ToListAsync();

            return View(sessions);
        }
    }
}