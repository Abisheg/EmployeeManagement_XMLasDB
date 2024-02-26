using EmployeeManagement.Models;
using System;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Dept? Department { get; set; }

        public static explicit operator EmployeeViewModel(Employee v)
        {
            return new EmployeeViewModel
            {
                Id = v.Id,
                Name = v.Name, 
                Email = v.Email,
                Department = v.Department
            };
        }
    }
}
