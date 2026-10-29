namespace AttendanceMonitoringSystem.Models
{
    public class Module
    {
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Course { get; set; }
        public string Semester { get; set; }

        public ICollection<Session> Sessions { get; set; }
    }
}