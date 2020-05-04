using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee(){Id=1, Name="Mary", Department="HR", Email="mary@pragim.com"},
                new Employee(){Id=2, Name="John", Department="IT", Email="john@pragim.com"},
                new Employee(){Id=3, Name="Sam", Department="IT", Email="sam@pragim.com"},
            };
        }

        public IEnumerable<Employee> GetAllemployee()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }
    }
}
