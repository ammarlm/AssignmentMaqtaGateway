using AssignmentMaqtaGateway3.ErrorHandler;
using AssignmentMaqtaGateway3.Helpers;
using AssignmentMaqtaGateway3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentMaqtaGateway3.Services
{
    public class EmployeeService : IEmployeeService, IDisposable
    {
        private ApplicationDbContext context;

        public EmployeeService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await context.Employees.ToListAsync();
        }
        public async Task<PaginationHelper<Employee>> GetEmployeesPagination(int page, int pageSize)
        {
            var pagination = new PaginationHelper<Employee>();
            pagination.CurrentPage = page;
            pagination.TotalItems = await context.Employees.CountAsync();
            pagination.TotalPages = pagination.TotalItems / pageSize;
            pagination.Items = await context.Employees.OrderBy(m => m.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return pagination;
        }
        public async Task<Employee> GetEmployeeByID(int id)
        {
            var emplyee = await context.Employees.FindAsync(id);
            if (emplyee == null)
                throw new LogicException("The Employee is not exist");

            return emplyee;
        }

        public async Task InsertEmployee(Employee employee)
        {
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();
        }

        public async Task DeleteEmployee(int employeeID)
        {
            Employee employee = await this.GetEmployeeByID(employeeID);
            context.Employees.Remove(employee);
            await context.SaveChangesAsync();
        }

        public async Task UpdateEmployee(int id, Employee employee)
        {
            var emplyeeExist = await this.GetEmployeeByID(id);
            emplyeeExist.PhoneNumber = employee.PhoneNumber;
            emplyeeExist.Email = employee.Email;
            emplyeeExist.Address = employee.Address;
            emplyeeExist.FullName = employee.FullName;
            context.Entry(emplyeeExist).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
