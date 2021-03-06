using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationService.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private IConfiguration _config;

        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(TokenController));
        public TokenController(IConfiguration config)
        {
            _config = config;
        }

       /* public TokenController()
        {
        }*/

        [HttpPost]
        public IActionResult Login([FromBody] Authenticate login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
                _log4net.Info("Token generated for user "+user.Name);
            }

            return response;
        }
        private string GenerateJSONWebToken(Authenticate userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                null,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //Uncomment this public method for nunit testing
        /*public Authenticate AuthenticateUser(Authenticate login)
        {
            Authenticate user = null;
            Dictionary<string,string> ValidUsersDictionary = new Dictionary<string,string>()
            {
                {"kaif","kaif123@"},
               
            };

            if (ValidUsersDictionary.Any(u => u.Key == login.Name && u.Value == login.Password))
            {
                user = new Authenticate { Name = login.Name, Password = login.Password };
            }

           
            return user;
        }*/
       
        private Authenticate AuthenticateUser(Authenticate login)
        {
            Authenticate user = null;
            Dictionary<string, string> ValidUsersDictionary = new Dictionary<string, string>()
            {
               {"kaif","kaif123@"},
               {"mehakpreet","mehak123@"},
               {"anto","anto123@"},
               {"divyansh","divyansh123@"},
               {"bhavani","bhavani123@"}

            };

            if (ValidUsersDictionary.Any(u => u.Key == login.Name && u.Value == login.Password))
            {
                user = new Authenticate { Name = login.Name, Password = login.Password };
            }


            return user;
        }
    }
}
