using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Model;
using DataAccess1;

namespace Services
{
    public class EmployeeServices
    {
        private Repository<Employees> _employeesRepository;

        public EmployeeServices()
        {
            _employeesRepository = new Repository<Employees>();
        }

        public Employee GetOne(string firstName , string lastName )
        {

            var employees = _employeesRepository
                .Set()
                .FirstOrDefault(c => c.FirstName == firstName && c.LastName == lastName);

            Employee employee = null;

            if (employees != null)
            {
                employee = new Employee
                {
                    EmployeeID = employees.EmployeeID,
                    FirstName = employees.FirstName,
                    LastName = employees.LastName
                };
            }

            return employee;

        }


    }
}
