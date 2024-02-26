using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository 
    {
        private readonly List<Employee> _empList;

        public MockEmployeeRepository()
        {
            _empList = new List<Employee>()
            {
                new Employee(){Id=1, Name="Mark", Department =Dept.IT, Email="Mark@hotmil.com"},
                new Employee(){Id=2, Name="Param", Department=Dept.IT, Email="param@hotmil.com"},
                new Employee(){Id=3, Name="Tom", Department= Dept.HR, Email="Tom@hotmil.com"}
            };
        }


        public IEnumerable<Employee> GetAllEmployee()
        {
            return _empList;
        }

        public Employee GetEmployee(int Id)
        {
            return _empList.FirstOrDefault(emp => emp.Id == Id);
        }
        public Employee Add(Employee employee)
        {
            employee.Id = _empList.Max(e => e.Id) + 1;
            _empList.Add(employee);
            return employee;
        }

        public Employee Update(Employee employeeUpdate)
        {
            var emp = GetEmployee(employeeUpdate.Id);
            if (employeeUpdate != null)
            {

                emp.Name = employeeUpdate.Name;
                emp.Email = employeeUpdate.Email;
                emp.Department = employeeUpdate.Department;          
            }

            return emp;
        }
        public Employee Remove(int Id)
        {
            var EmpRemove = GetEmployee(Id);

            if (EmpRemove !=null)
            {
            _empList.Remove(EmpRemove);
            }

            return EmpRemove;
        }

    }
}
