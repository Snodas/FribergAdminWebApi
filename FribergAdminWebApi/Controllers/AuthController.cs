using FribergAdminWebApi.Data;
using FribergAdminWebApi.Data.Dto;
using FribergAdminWebApi.Data.Interfaces;
using FribergAdminWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                //UserName = regDto.Email,
                //NormalizedUserName = regDto.Email.ToUpper(),
                //Email = regDto.Email,
                //NormalizedEmail = regDto.Email.ToUpper(),
                //FirstName = regDto.FirstName,
                //LastName = regDto.LastName,
                //EmailConfirmed = false,
            };

            return user;
        }

        private Employee CreateEmployee(RegisterDto regDto)
        {
            Employee employee = new Employee
            {
                //Email = regDto.Email,
                //PhoneNumber = regDto.PhoneNumber,
                //FirstName = regDto.FirstName,
                //LastName = regDto.LastName,
            };

            return employee;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDto regDto)
        {
            return BadRequest("Not implemented yet");
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginUserDto logDto)
        {
            return BadRequest("Not implemented yet");
        }
    }
}
