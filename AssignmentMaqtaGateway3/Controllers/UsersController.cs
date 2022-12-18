using AssignmentMaqtaGateway3.DTO;
using AssignmentMaqtaGateway3.Model;
using AssignmentMaqtaGateway3.Services;
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
    public class UsersController : ControllerBase
    {
        private IUserService userService;
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUser(int page, int pageSize)
        {
            return Ok(await userService.GetAllUsers(page, pageSize));
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> PostCreateUser([FromBody] LoginRequestDTO user)
        {
            await userService.InsertUser(new User() { UserName = user.UserName, Password = user.Password });
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> PostLogin([FromBody] LoginRequestDTO loginRequestDTO)
        {
            return Ok(await userService.Login(loginRequestDTO));
        }


    }
}
