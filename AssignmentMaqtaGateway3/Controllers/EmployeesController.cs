using AssignmentMaqtaGateway3.Model;
using AssignmentMaqtaGateway3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AssignmentMaqtaGateway3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private IEmployeeService employeeRepository;
        public EmployeesController(IEmployeeService employeeRep)
        {
            employeeRepository = employeeRep;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees(int page, int pageSize)
        {
            var emplyees = await employeeRepository.GetEmployeesPagination(page, pageSize);
            return Ok(emplyees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeByID(int id)
        {
            var emplyee = await employeeRepository.GetEmployeeByID(id);
            return Ok(emplyee);
        }

        [HttpPost]
        public async Task<IActionResult> PostCreateEmployee([FromBody] Employee employee)
        {
            await employeeRepository.InsertEmployee(employee);

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEditEmployee(int id, [FromBody] Employee employee)
        {

            await employeeRepository.UpdateEmployee(id, employee);
            return Ok(employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await employeeRepository.DeleteEmployee(id);
            return Ok();
        }
    }
}
