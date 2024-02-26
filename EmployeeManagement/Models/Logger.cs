using System;

namespace EmployeeManagement.Models
{
    public class Logger
    {
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
        public Employee Employee { get; set; }
    }

}
