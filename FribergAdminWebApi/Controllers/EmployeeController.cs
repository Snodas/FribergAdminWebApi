using AutoMapper;
using FribergAdminWebApi.Constants;
using FribergAdminWebApi.Data;
using FribergAdminWebApi.Data.Dto;
using FribergAdminWebApi.Data.Interfaces;
using FribergAdminWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FribergAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;

        public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper, UserManager<ApiUser> userManager)
        {
            this._employeeRepository = employeeRepository;
            this._mapper = mapper;
            this._userManager = userManager;
        }

        [HttpPost("create")]
        [Authorize(Roles = $"{ApiRoles.Admin}")]
        public async Task<ActionResult> CreateEmployee(EmployeeCreateDto dto)
        {
            return NotFound("Not implemented yet");
        }

        [HttpPut("{employeeId}/edit")]
        public async Task<ActionResult<EmployeeUpdateDto>> UpdateEmployee(int employeeId, EmployeeUpdateDto dto)
        {
            return NotFound("Not implemented yet");
        }

        [HttpGet("admin/allemployees")]
        [Authorize(Roles = ApiRoles.Admin)]
        public async Task<ActionResult<IEnumerable<EmployeeUserDto>>> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            if (employees == null)
            {
                return NotFound("No employees found.");
            }

            var response = _mapper.Map<IEnumerable<EmployeeUserDto>>(employees);

            return Ok(response);
        }

        //[HttpGet()]
        //public async Task
    }
}
