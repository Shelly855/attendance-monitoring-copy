using AttendanceMonitoringSystem.Models;
using AttendanceMonitoringSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceMonitoringSystem.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly AttendanceReportService _reportService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(AttendanceReportService reportService, UserManager<ApplicationUser> userManager)
        {
            _reportService = reportService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int studentId, int moduleId, int? sessionId)
        {
            var userId = _userManager.GetUserId(User);

            if (!await _reportService.IsTutorAuthorisedForModuleAsync(userId, moduleId))
            {
                return Forbid();
            }

            var report = await _reportService.GetStudentReport(studentId, moduleId);

            if (report == null)
            {
                return NotFound();
            }

            ViewData["SessionId"] = sessionId;
            ViewData["StudentId"] = studentId;
            ViewData["ModuleId"] = moduleId;

            return View(report);
        }

        public async Task<IActionResult> History(int studentId, int moduleId, int? sessionId)
        {
            var userId = _userManager.GetUserId(User);

            if (!await _reportService.IsTutorAuthorisedForModuleAsync(userId, moduleId))
            {
                return Forbid();
            }

            var report = await _reportService.GetStudentReport(studentId, moduleId);

            if (report == null)
            {
                return NotFound();
            }

            var history = await _reportService.GetCorrectionHistory(studentId, moduleId);

            ViewData["SessionId"] = sessionId;
            ViewData["StudentId"] = studentId;
            ViewData["ModuleId"] = moduleId;
            ViewData["StudentName"] = report.StudentName;
            ViewData["ModuleName"] = report.ModuleName;

            return View(history);
        }
    }
}