using ChineseAuctionApi.Models;
using ChineseAuctionApi.Repositories.AuthUsers;
using Microsoft.AspNetCore.Http;

namespace ChineseAuctionApi.Services.AuthUser
{
    public class AuthUserService : IAuthUserService
    {
        private readonly IAuthUserRepositoty _AuthUserRepositoty;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthUserService(IAuthUserRepositoty AuthUserRepositoty, IHttpContextAccessor httpContextAccessor)
        {
            _AuthUserRepositoty = AuthUserRepositoty;
            _httpContextAccessor = httpContextAccessor;
        }

        public User? getUserByMail(string mail)
        {
            try
            {
                return _AuthUserRepositoty.getUserByMail(mail);
            }
            catch (BadHttpRequestException ex)
            {
                throw ex;
            }
        }

        public bool isMailUniq(string mail)
        {
            try
            {
                _AuthUserRepositoty.getUserByMail(mail);
                return false;
            }
            catch (BadHttpRequestException ex)
            {
                return true;
            }
        }

        public User? register(User user)
        {
            return _AuthUserRepositoty.register(user);
        }

        public bool getRole()
        {
            try
            {
                var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                return _AuthUserRepositoty.getRole(token);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
