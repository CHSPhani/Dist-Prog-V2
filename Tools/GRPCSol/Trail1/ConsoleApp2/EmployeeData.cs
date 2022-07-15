using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class EmployeeData
    {
        public EmployeeName GetEmployeeName(EmployeeNameRequest request)
        {
            EmployeeName empName = new EmployeeName();
            switch (request.EmpId)
            {
                case "1":
                    empName.FirstName = "John";
                    empName.LastName = "Doe";
                    break;
                case "2":
                    empName.FirstName = "Dave";
                    empName.LastName = "Williams";
                    break;
                default:
                    break;
            }
            return empName;
        }
    }
}
