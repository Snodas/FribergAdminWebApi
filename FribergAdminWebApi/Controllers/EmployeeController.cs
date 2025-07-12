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
        //[Authorize(Roles = $"{ApiRoles.Admin}")]
        public async Task<ActionResult> CreateEmployee(EmployeeCreateDto dto)
        {
            try
            {
                if (await _employeeRepository.AnyAsync(e => e.Email == dto.Email))
                {
                    return BadRequest("Emplyee with this email already exists");
                }

                var employee = _mapper.Map<Employee>(dto);

                var createdEmployee = await _employeeRepository.AddAsync(employee);

                return CreatedAtAction(
                    nameof(GetEmployee),
                    new { id = createdEmployee.Id },
                    createdEmployee
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the employee");
            }           
        }

        [HttpPut("{employeeId}/edit")]
        public async Task<ActionResult<EmployeeUpdateDto>> UpdateEmployee(int employeeId, EmployeeUpdateDto dto)
        {
            return NotFound("Not implemented yet");
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
           
            return Ok(employee);
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
