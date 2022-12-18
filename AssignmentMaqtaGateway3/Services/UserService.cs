using AssignmentMaqtaGateway3.DTO;
using AssignmentMaqtaGateway3.ErrorHandler;
using AssignmentMaqtaGateway3.Helpers;
using AssignmentMaqtaGateway3.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentMaqtaGateway3.Services
{
    public class UserService : IUserService
    {
        private ApplicationDbContext context;
        private readonly JwtConfig jwtConfig;
        private int itr = 1;
        public UserService(ApplicationDbContext dbContext, IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            context = dbContext;
            jwtConfig = optionsMonitor.CurrentValue;
        }
        public async Task InsertUser(User user)
        {
            var userExist = await context.Users.FirstOrDefaultAsync(m => m.UserName == user.UserName);
            if (userExist != null)
                throw new LogicException("User is exist");

            var salt = PasswordHasher.GenerateSalt();
            var hashPassword = PasswordHasher.ComputeHash(user.Password, salt, itr);
            user.Password = hashPassword;
            user.Salt = salt;
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsers(int page, int pageSize)
        {

            return await context.Users.OrderBy(m => m.Id).Skip(page * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<LoginDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await context.Users.FirstOrDefaultAsync(user => user.UserName == loginRequestDTO.UserName);
            if (user == null)
                throw new LogicException("Username or password did not match.");

            var passwordHash = PasswordHasher.ComputeHash(loginRequestDTO.Password, user.Salt, itr);
            if (user.Password != passwordHash)
                throw new LogicException("Username or password did not match.");

            return GenerateJwtToken(user);

        }
        private LoginDTO GenerateJwtToken(User user)
        {
            byte[] secret = Encoding.ASCII.GetBytes(jwtConfig.Key);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtConfig.Issuer,
                Audience = jwtConfig.Audience,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Role, "Admin"),
                    //for refrash token
                    //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(jwtConfig.Expired),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = handler.CreateToken(descriptor);
            //handler.WriteToken(token);
            return new LoginDTO()
            {
                Token = handler.WriteToken(token),
                UserName = user.UserName,
                ExpiredInMinute = jwtConfig.Expired,
            };
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
