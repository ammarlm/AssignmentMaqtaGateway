using AssignmentMaqtaGateway3.Controllers;
using AssignmentMaqtaGateway3.ErrorHandler;
using AssignmentMaqtaGateway3.Helpers;
using AssignmentMaqtaGateway3.Model;
using AssignmentMaqtaGateway3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestAssignment
{
    public class UnitEmployee
    {

        private EmployeesController employeesController;
        IEmployeeService _service;

        public UnitEmployee()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(databaseName: "FakeDatabase")
                 .EnableSensitiveDataLogging()
                 .Options;

            var db = new ApplicationDbContext(options);
            _service = new EmployeeService(db);
            employeesController = new EmployeesController(_service);

        }
        [Fact]
        public async Task GetAllEmployees_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = await employeesController.GetAllEmployees(0, 100);
            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }
        [Fact]
        public async Task GetAllEmployees_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = await employeesController.GetAllEmployees(0, 100) as OkObjectResult;
            // Assert
            var items = Assert.IsType<PaginationHelper<Employee>>(okResult.Value);
            var list = new List<Employee>(items.Items);
            Assert.Equal(items.TotalItems, list.Count);
        }
        [Fact]
        public async Task GetEmployee()
        {

            Random rnd = new Random();
            int num = rnd.Next(100);
            // Act
            try
            {
                // Act
                var okResult = await employeesController.GetEmployeeByID(num);
                // Assert
                Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
            }
            catch (LogicException ex)
            {
                // Assert
                Assert.Equal("The Employee is not exist", ex.Message);
                //Assert.IsType<LogicException>(ex);
            }
        }

        [Fact]
        public async Task DeleteEmployee()
        {

            Random rnd = new Random();
            int num = rnd.Next(100);
            // Act
            try
            {
                // Act
                var okResult = await employeesController.DeleteEmployee(num);
                // Assert
                Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
            }
            catch (LogicException ex)
            {
                // Assert
                Assert.Equal("The Employee is not exist", ex.Message);
            }
        }

        [Fact]
        public async Task CreateEmployee()
        {

            Random rnd = new Random();
            int num = rnd.Next(100);

            // Act
            var okResult = await employeesController.PostCreateEmployee(new Employee()
            {
                Address = "Test",
                Email = "ammar@test.com",
                FullName = "Test",
                PhoneNumber = "0987654321"
            });
            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);

        }

        [Fact]
        public async Task CreateEmployeeWithCheckExist()
        {

            // Act
            var okResult = await employeesController.PostCreateEmployee(new Employee()
            {
                Address = "Test",
                Email = "ammar@test.com",
                FullName = "Test",
                PhoneNumber = "0987654321"
            }) as OkObjectResult;
            // Assert
            Assert.IsType<OkObjectResult>(okResult);

            var emp = okResult.Value as Employee;
            var checkCreatedResult = await employeesController.GetEmployeeByID(emp.Id) as OkObjectResult;
            Assert.IsType<OkObjectResult>(checkCreatedResult);
        }


        [Fact]
        public async Task EditEmployeeWithCheckExist()
        {

            // Act
            var okResult = await employeesController.PostCreateEmployee(new Employee()
            {
                Address = "Test",
                Email = "ammar@test.com",
                FullName = "Test",
                PhoneNumber = "0987654321"
            }) as OkObjectResult;
            // Assert
            Assert.IsType<OkObjectResult>(okResult);

            var emp = okResult.Value as Employee;
            var checkCreatedResult = await employeesController.GetEmployeeByID(emp.Id) as OkObjectResult;
            Assert.IsType<OkObjectResult>(checkCreatedResult);


            emp.FullName = "Updated";
            var updatedResult = await employeesController.PutEditEmployee(emp.Id, emp) as OkObjectResult;
            Assert.IsType<OkObjectResult>(updatedResult);


            var checkUpdatedResult = await employeesController.GetEmployeeByID(emp.Id) as OkObjectResult;
            var empUpdated = checkUpdatedResult.Value as Employee;
            Assert.Equal(emp.FullName, empUpdated.FullName);
        }
    }
}
