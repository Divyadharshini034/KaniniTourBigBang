
using KaniniTourism.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Text;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenControllers : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly KaniniDbContext _context;
        
        private const string TravelerRole = "Traveler";
        
        public TokenControllers(IConfiguration config, KaniniDbContext context)
        {
            _configuration = config;
            _context = context;
        }

        

        [HttpPost("Traveler")]
        public async Task<IActionResult> Post(Travelers _userData)
        {
            if (_userData.TravelerName != null && _userData.TravelerPass!= null)
            {
                var user = await GetUser(_userData.TravelerName, _userData.TravelerPass);

                if (user != null)
                {
                    //create claims details based on the user information
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),

                         new Claim("TravelerName", user.TravelerName),
                        new Claim("TravelerPass",user.TravelerPass),
                        new Claim(ClaimTypes.Role,TravelerRole),

                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var travelertoken = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                       signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(travelertoken));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }

        }
        private async Task<Travelers> GetUser(string username, string password)
        {
            return await _context.Traveler.FirstOrDefaultAsync(u => u.TravelerName == username && u.TravelerPass == password);
        }


       

    }
}

