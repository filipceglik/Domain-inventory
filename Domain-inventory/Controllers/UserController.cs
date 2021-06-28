using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain_inventory.Helpers;
using Domain_inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Domain_inventory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Domain_inventory.Controllers
{
    [Authorize]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserRepository _userRepository;
        private readonly AppSettings _appSettings;

        public UserController(ILogger<UserController> logger,UserRepository userRepository, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        public IActionResult Login()
        {
            return View();
        }
        
        [Authorize(Roles = Role.Admin)]
        [HttpPost("user/new")]
        public async Task<ActionResult> Create([FromForm] CreateUserViewModel createUserViewModel)
        {
            bool validUsername;
            var hashed = BCrypt.Net.BCrypt.HashPassword(createUserViewModel.Password);
            var user = new User(Guid.NewGuid(),createUserViewModel.UserName,hashed,DateTime.Now,createUserViewModel.Role);
            if (await _userRepository.Create(user))
            {
                return Ok();
            }

            return BadRequest();

        }
        
        [Authorize]
        [HttpPost("user/changepassword")]
        public async Task<ActionResult> ChangePassword([FromForm] UpdateUserViewModel updateUserViewModel)
        {
            ClaimsPrincipal currentUser = this.User;
            var hashed = BCrypt.Net.BCrypt.HashPassword(updateUserViewModel.Password);
            var user = await _userRepository.GetUser(updateUserViewModel.UserName);
            if (BCrypt.Net.BCrypt.Verify(updateUserViewModel.OldPassword,user.Password))
            {
                user.Password = hashed;
                await _userRepository.Update(user);
                return Ok();
            }

            return BadRequest();


        }
        
        [Authorize(Roles = Role.Admin)]
        [HttpPost("user/delete")]
        public async Task<ActionResult> Delete([FromForm] DeleteUserViewModel deleteUserViewModel)
        {
            var user = await _userRepository.GetUser(deleteUserViewModel.UserName);
            await _userRepository.Delete(user);
            return Ok();
        }
        
        [AllowAnonymous]
        [HttpPost("user/login")]
        public async Task<ActionResult> Login([FromForm] LoginViewModel loginViewModel)
        {
            var user = await _userRepository.GetUser(loginViewModel.UserName);

            if (user is null)
            {
                return BadRequest();
            }
            if (!BCrypt.Net.BCrypt.Verify(loginViewModel.Password,user.Password))
            {
                return BadRequest();
            }

            

            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, loginViewModel.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                }),
                Expires = DateTime.UtcNow.AddSeconds(_appSettings.LifetimeInSeconds),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _appSettings.ValidIssuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return Ok
            (
                new {access_token = tokenHandler.WriteToken(token)}
            );
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}