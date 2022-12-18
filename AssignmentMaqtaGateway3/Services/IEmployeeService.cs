using AssignmentMaqtaGateway3.Helpers;
using AssignmentMaqtaGateway3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentMaqtaGateway3.Services
{
    public interface IEmployeeService : IDisposable
    {
        Task<IEnumerable<Employee>> GetEmployees();
        Task<PaginationHelper<Employee>> GetEmployeesPagination(int page, int pageSize);
        Task<Employee> GetEmployeeByID(int employeeId);
        Task InsertEmployee(Employee employee);
        Task DeleteEmployee(int employeeID);
        Task UpdateEmployee(int id, Employee employee);
    }
}
