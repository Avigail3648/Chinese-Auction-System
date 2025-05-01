using ChineseAuctionApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChineseAuctionApi.Repositories.AuthUsers
{
    public class AuthUserRepository : IAuthUserRepositoty
    {
        private readonly DBContext _DbContext;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly ILogger<AuthUserRepository> _logger;

        public AuthUserRepository(DBContext DbContext, ILogger<AuthUserRepository> logger)
        {
            _DbContext = DbContext;
            _passwordHasher = new PasswordHasher<User>();
            _logger = logger;
        }

        public User? getUserByMail(string mail)
        {
            try
            {
                var u = _DbContext.Users.FirstOrDefault(u => u.Mail == mail);
                if (u == null)
                    throw new BadHttpRequestException("לא קיים " + mail + " משתמש");
                return u;
            }
            catch (BadHttpRequestException e)
            {
                _logger.LogError("", e);
                throw e;
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed Get User By Email {mail}", e);
                return null;
            }
        }
        public User? register(User user)
        {
            try
            {
                user.Password = _passwordHasher.HashPassword(user, user.Password);
                _DbContext.Users.Add(user);
                _DbContext.SaveChanges();
                return _DbContext.Users.FirstOrDefault(u => u.Mail == user.Mail);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed register {user.Name}", ex);
                return null;
            }
        }

        public bool getRole(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userRole = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (userRole == "Admin")
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
