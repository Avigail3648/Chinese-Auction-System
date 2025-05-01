using ChineseAuctionApi.Models;

namespace ChineseAuctionApi.Services.AuthUser
{
    public interface IAuthUserService
    {
        public User? getUserByMail(string mail);
        public User? register(User user);
        public bool isMailUniq(string mail);
        bool getRole();
    }
}
