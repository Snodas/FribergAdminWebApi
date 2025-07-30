using FribergAdminWebApi.Constants;
using FribergAdminWebApi.Data;
using FribergAdminWebApi.Data.Dto;
using FribergAdminWebApi.Data.Interfaces;
using FribergAdminWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FribergAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeRepository _employeeRepository;

        public AuthController(UserManager<ApiUser> userManager, IConfiguration configuration, IEmployeeRepository employeeRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _employeeRepository = employeeRepository;
        }

        private ApiUser CreateApiUser(RegisterDto regDto)
        {
            ApiUser user = new ApiUser
            {
                UserName = regDto.Email,
                NormalizedUserName = regDto.Email.ToUpper(),
                Email = regDto.Email,
                NormalizedEmail = regDto.Email.ToUpper(),
                FirstName = regDto.FirstName,
                LastName = regDto.LastName,
                EmailConfirmed = true,
            };

            return user;
        }

        private Employee CreateEmployee(RegisterDto regDto, string userId)
        {
            Employee employee = new Employee
            {
                Email = regDto.Email,
                PhoneNumber = regDto.PhoneNumber,
                FirstName = regDto.FirstName,
                LastName = regDto.LastName,
                HourlyRate = 0,
                SocialSecurityNumber = regDto.SocialSecurityNumber,
                Address = regDto.Address,
                EmergencyContactName = regDto.EmergencyContactName,
                EmergencyContactPhone = regDto.EmergencyContactPhone,
                ApiUserId = userId
            };
            return employee;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDto regDto)
        {
            try
            {
                if (await _userManager.FindByEmailAsync(regDto.Email) != null)
                {
                    return BadRequest("User with this email already exists");
                }

                var newUser = CreateApiUser(regDto);
                var userResult = await _userManager.CreateAsync(newUser, regDto.Password);

                if (!userResult.Succeeded)
                {
                    return BadRequest($"Failed to create user: {string.Join(", ", userResult.Errors.Select(e => e.Description))}");
                }

                await _userManager.AddToRoleAsync(newUser, ApiRoles.Employee);

                var newEmployee = CreateEmployee(regDto, newUser.Id);
                await _employeeRepository.AddAsync(newEmployee);

                return Ok(new { Message = "User registered successfully", UserId = newUser.Id });
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong in the {nameof(Register)}: {ex.Message}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginUserDto logDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(logDto.Email);
                var passwordValid = await _userManager.CheckPasswordAsync(user, logDto.Password);
                
                if (!passwordValid || user == null)
                {
                    return Unauthorized();
                }
                
                string tokenString = await GenerateToken(user);
                var response = new AuthResponse
                {
                    Email = user.Email,
                    Token = tokenString,
                    UserId = user.Id
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong in the {nameof(Login)}", statusCode: 500);
            }
        }



        private async Task<string> GenerateToken(ApiUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();

            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomClaimTypes.Uid, user.Id)
            }
            .Union(roleClaims)
            .Union(userClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
