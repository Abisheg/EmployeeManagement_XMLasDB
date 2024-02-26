using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
              new Employee() { Id = 1, Name = "Mark", Department = Dept.IT, Email = "Mark@hotmil.com" },
              new Employee() { Id = 2, Name = "Param", Department = Dept.IT, Email = "param@hotmil.com" },
              new Employee() { Id = 3, Name = "Tom", Department = Dept.HR, Email = "Tom@hotmil.com" }
           );
        }

    }
}
