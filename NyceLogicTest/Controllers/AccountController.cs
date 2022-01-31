using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using NyceLogicTest.Models;
using NyceLogicTest.Repository.Interfaces;

namespace NyceLogicTest.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private IConfiguration _config;
        private IUserService _userService;

        public AccountController(IConfiguration config, IUserService userService)
        {
            _userService = userService;
            _config = config;
        }
        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [Produces("text/plain")]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginModel data)
        {
            
            try
            {
                var user = await _userService.Login(data);
                if (user != null)
                {
                    var tokenString = "Bearer " + GenerateJSONWebToken(user);
                    return Ok(tokenString);
                }
                else
                {
                    return Unauthorized("Wrong username or password");
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register([FromBody] LoginModel data)
        {          
            try
            {
                var user = await _userService.Register(data);
                return Ok("Registered succesfully");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }       
    }
}

