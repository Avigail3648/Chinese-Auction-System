using ChineseAuctionApi.Models;

namespace ChineseAuctionApi.Repositories.AuthUsers
{
    public interface IAuthUserRepositoty
    {
        public User? getUserByMail(string mail);
        public User? register(User user);
        bool getRole(string token);
    }
}
