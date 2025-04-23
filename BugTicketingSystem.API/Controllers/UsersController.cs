using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BugTicketingSystem.BL.Dtos.AuthDtos;
using BugTicketingSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BugTicketingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private UserManager<User> _userManager;

        public UsersController(IConfiguration configuration,
            UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        #region Login
        [HttpPost("login")]
        public async Task<Results<Ok<TokenDto>, UnauthorizedHttpResult>>
        Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                return TypedResults.Unauthorized();
            }
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                return TypedResults.Unauthorized();
            }
            var claims = await _userManager.GetClaimsAsync(user);
            var tokenDto = GenerateToken(claims.ToList());
            return TypedResults.Ok(tokenDto);

        }
        #endregion
        #region Register
        [HttpPost("register")]
        public async Task<Results<NoContent, BadRequest<List<string>>>>
            Register([FromBody] RegisterDto registerDto)
        {
            var user = new User()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Name = registerDto.UserName,
                Role = registerDto.Role
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return TypedResults.BadRequest(errors);
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            await _userManager.AddClaimsAsync(user, claims);
            return TypedResults.NoContent();
        }
        #endregion
        #region Generate token
        private TokenDto GenerateToken(List<Claim> claims)
        {
            var secretKey = _configuration.GetValue<string>("SecretKey");
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            var key = new SymmetricSecurityKey(secretKeyBytes);

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(1);

            var token = new JwtSecurityToken(
                
                claims: claims,
                expires: expires,
                signingCredentials: creds);
            var ToKenString = new JwtSecurityTokenHandler().WriteToken(token);
            return new TokenDto(ToKenString, token.ValidTo);
        }
        #endregion
    }
}
