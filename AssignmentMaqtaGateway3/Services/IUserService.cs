using AssignmentMaqtaGateway3.DTO;
using AssignmentMaqtaGateway3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentMaqtaGateway3.Services
{
    public interface IUserService : IDisposable
    {
        Task<IEnumerable<User>> GetAllUsers(int page, int pageSize);
        Task InsertUser(User user);
        Task<LoginDTO> Login(LoginRequestDTO loginRequestDTO);
    }
}
