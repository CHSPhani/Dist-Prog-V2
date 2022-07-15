using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace ConsoleApp2.controller
{
    class AccountsImpl : AccountService.AccountServiceBase
    {
        // Server side handler of the GetEmployeeName 
        public override Task<EmployeeName> GetEmployeeName(EmployeeNameRequest request, ServerCallContext context)
        {
            EmployeeData empData = new EmployeeData();
            return Task.FromResult(empData.GetEmployeeName(request) );
        }
}
}
