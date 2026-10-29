using AttendanceMonitoringSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AttendanceMonitoringSystem.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<AttendanceDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Only seed if database is empty
            if (context.Modules.Any())
            {
                return;
            }

            // 1. Create Tutor role
            if (!await roleManager.RoleExistsAsync("Tutor"))
            {
                await roleManager.CreateAsync(new IdentityRole("Tutor"));
            }

            // 2. Create Tutor account
            var tutor = new ApplicationUser
            {
                UserName = "jane.tutor",
                Email = "jane.tutor@university.ac.uk",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(tutor, "Tutor@123!");
            await userManager.AddToRoleAsync(tutor, "Tutor");

            // 3. Create Module
            var module = new Module
            {
                Name = "Software Architecture & Design",
                Course = "BEng Software Engineering",
                Semester = "Semester 2, 2027"
            };
            context.Modules.Add(module);
            await context.SaveChangesAsync();

            // 4. Create Sessions for that module
            var session1 = new Session
            {
                ModuleId = module.ModuleId,
                Date = new DateTime(2027, 2, 22),
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(11, 0, 0),
                SessionType = "Lecture"
            };
            var session2 = new Session
            {
                ModuleId = module.ModuleId,
                Date = new DateTime(2027, 3, 1),
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(11, 0, 0),
                SessionType = "Lecture"
            };
            context.Sessions.AddRange(session1, session2);
            await context.SaveChangesAsync();

            // 5. Create Students
            var student1 = new Student { Name = "Thomas Holmes" };
            var student2 = new Student { Name = "John Johnson" };
            var student3 = new Student { Name = "Emma Watson" };
            context.Students.AddRange(student1, student2, student3);
            await context.SaveChangesAsync();

            // 6. Authorise Tutor for both sessions
            context.TutorSessions.AddRange(
                new TutorSession { UserId = tutor.Id, SessionId = session1.SessionId },
                new TutorSession { UserId = tutor.Id, SessionId = session2.SessionId }
            );

            // 7. Seed initial Attendance records
            context.Attendances.AddRange(
                new Attendance { StudentId = student1.StudentId, SessionId = session1.SessionId, Status = "Present" },
                new Attendance { StudentId = student2.StudentId, SessionId = session1.SessionId, Status = "Absent" },
                new Attendance { StudentId = student3.StudentId, SessionId = session1.SessionId, Status = "Late Arrival" },
                new Attendance { StudentId = student1.StudentId, SessionId = session2.SessionId, Status = "Present" },
                new Attendance { StudentId = student2.StudentId, SessionId = session2.SessionId, Status = "Present" },
                new Attendance { StudentId = student3.StudentId, SessionId = session2.SessionId, Status = "Absent" }
            );

            await context.SaveChangesAsync();
        }
    }
}