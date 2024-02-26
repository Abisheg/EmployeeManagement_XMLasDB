using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }       
        public string Name { get; set; }
        [Required]        
        public string Email { get; set; }       
        public string Password { get; set; }       
        public string ConfirmPassword { get; set; }
        [Required]
        public Dept? Department { get; set; }

        public string ToXmlString()
        {
            XElement employeeElement = new XElement("Employee",
                new XElement("Id", Id),
                new XElement("Name", Name),
                new XElement("Email", Email),
                new XElement("Department", Department.ToString())
            );

            return employeeElement.ToString();
        }
    }
}
