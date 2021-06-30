using AuthenticationJWT.Context;
using AuthenticationJWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration config;
        public LoginController(IConfiguration config, ApplicationDbContext context)
        {
            this.config = config;
            this.context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            ActionResult response = Unauthorized();

            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);

                response = Ok(new { token = tokenString });
            }

            return response; 
        }

        private string GenerateJSONWebToken(LoginViewModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));

            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Iss, "https://localhost:44330/login/"),
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(config["Jwt.Issuer"], config["Jwt:Issuer"], claims, expires: DateTime.Now.AddMinutes(120), signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private LoginViewModel AuthenticateUser(LoginViewModel login)
        {
            LoginViewModel user = null;

            IEnumerable<Usuarios> usuarios;

            usuarios = from p in context.Users
                       where p.Email == login.Email
                        select p;

            if (login.Email == usuarios.First<Usuarios>().Email )
            {
                user = new LoginViewModel { Email = usuarios.First<Usuarios>().Email, Pass = usuarios.First<Usuarios>().Pass };
            }
            return user;
        }
    }
}
