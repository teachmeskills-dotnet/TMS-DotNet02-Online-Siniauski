using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Siniauski.WhatIWant.Data.Models;
using Siniauski.WhatIWant.WebData.Contracts.Requests;
using Siniauski.WhatIWant.WebData.Contracts.Responses;
using Siniauski.WhatIWant.WebData.Enums;
using Siniauski.WhatIWant.WebData.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Siniauski.WhatIWant.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest requestInfo)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(requestInfo.Login);
                if (user != null && await _userManager.CheckPasswordAsync(user, requestInfo.Password))
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };
                    var token = GetToken(authClaims);
                    return Ok(new UserAuthModel()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest requestInfo)
        {
            try
            {
                var userExists = await _userManager.FindByNameAsync(requestInfo.Login);
                if (userExists != null)
                    return BadRequest(new Response { Status = ResponseStatus.Failure, Message = "Пользователь с таким логином уже существует!" });

                User user = new()
                {
                    Email = requestInfo.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = requestInfo.Login,
                    FirstName = requestInfo.FirstName,
                    LastName = requestInfo.LastName,
                    Avatar = requestInfo.Avatar,
                    BirthDate = requestInfo.BirthDate,
                    PhoneNumber = requestInfo.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, requestInfo.Password);
                if (!result.Succeeded)
                    return BadRequest(new Response { Status = ResponseStatus.Failure, Message = "Ошибка при регистрации пользователя!" });
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var token = GetToken(authClaims);

                return Ok(new UserAuthModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(12),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}