using Azure.Core;
using ChineseAuctionApi.Models;
using ChineseAuctionApi.Services;
using ChineseAuctionApi.Services.AuthUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ChineseAuctionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthUserController : ControllerBase
    {
        private readonly IAuthUserService _AuthUserService;
        private readonly JwtTokenService _JwtTokenService;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public AuthUserController(IAuthUserService AuthUserService, JwtTokenService jwtTokenService, IConfiguration configuration)
        { 
            _AuthUserService = AuthUserService;
            _JwtTokenService = jwtTokenService;
            _passwordHasher = new PasswordHasher<User>();
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestt request)
        {
            try
            {
                var managerMail = _configuration.GetSection("managerMail");
                if (request.Mail == "*@gmail.com")
                    request.Mail = managerMail["mail"];

                  User? user = _AuthUserService.getUserByMail(request.Mail);
                if (user == null)
                    return BadRequest("failed");

                if (_passwordHasher.VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Success) 
                { 
                    var roles = new List<string> { $"{user.Role}" };
                    var token = _JwtTokenService.GenerateJwtToken(request.Mail, roles);
                    return Ok(new { Token = token });
                }
                return Unauthorized("משתמש לא רשום");
            }

            catch (BadHttpRequestException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("register")]
        public IActionResult register([FromBody] User user)
        {
            bool isMailExsit = _AuthUserService.isMailUniq(user.Mail);
            if (isMailExsit == false)
                return BadRequest("קיים במערכת. יש להכניס מייל אחר " + user.Mail + "מייל ");
            User u = _AuthUserService.register(user);
            if (u == null)
                return BadRequest("failed");
            var roles = new List<string> { $"{u.Role}" };
            var token = _JwtTokenService.GenerateJwtToken(u.Mail, roles);

            return Ok(new { Token = token });       
        }

        [HttpGet("getRole")]
        public IActionResult getRole()
        {
            try
            {
                var isAdmin = _AuthUserService.getRole();
                if (isAdmin == null)
                    return BadRequest("failed");
                return Ok(isAdmin);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
