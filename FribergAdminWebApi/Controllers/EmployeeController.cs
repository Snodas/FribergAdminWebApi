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
        private readonly IWorkEntryRepository _workEntryRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;

        public EmployeeController(IEmployeeRepository employeeRepository, IWorkEntryRepository workEntryRepository, IMapper mapper, UserManager<ApiUser> userManager)
        {
            this._employeeRepository = employeeRepository;
            this._workEntryRepository = workEntryRepository;
            this._mapper = mapper;
            this._userManager = userManager;
        }

        // optional
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

        [HttpGet("my-profile")]
        [Authorize(Roles = ApiRoles.Employee)]
        public async Task<ActionResult<EmployeeUserDto>> GetMyProfile()
        {
            try
            {
                var userIdClaim = User.FindFirst(CustomClaimTypes.Uid);
                if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                    return Unauthorized("User ID not found in token");

                var employee = await _employeeRepository.GetByUserIdAsync(userIdClaim.Value);
                if (employee == null)
                    return NotFound("Employee profile not found");

                var profileDto = _mapper.Map<EmployeeUserDto>(employee);
                return Ok(profileDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving your profile");
            }
        }

        [HttpPut("update-my-profile")]
        [Authorize(Roles = ApiRoles.Employee)]
        public async Task<ActionResult<EmployeeUserDto>> UpdateMyProfile(EmployeeUpdateDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(CustomClaimTypes.Uid);
                if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                    return Unauthorized("User ID not found in token");

                var employee = await _employeeRepository.GetByUserIdAsync(userIdClaim.Value);
                if (employee == null)
                    return NotFound("Employee profile not found");

                // Check if email is being changed and if it already exists
                if (employee.Email != dto.Email &&
                    await _employeeRepository.AnyAsync(e => e.Email == dto.Email))
                {
                    return BadRequest("An employee with this email already exists");
                }

                // Map the update DTO to the employee (excludes HourlyRate)
                _mapper.Map(dto, employee);

                // Update ApiUser email if it exists and has changed
                if (employee.ApiUser != null && employee.ApiUser.Email != dto.Email)
                {
                    employee.ApiUser.Email = dto.Email;
                    employee.ApiUser.UserName = dto.Email;
                    employee.ApiUser.NormalizedEmail = dto.Email.ToUpper();
                    employee.ApiUser.NormalizedUserName = dto.Email.ToUpper();
                    await _userManager.UpdateAsync(employee.ApiUser);
                }

                await _employeeRepository.UpdateAsync(employee);
                var result = _mapper.Map<EmployeeUserDto>(employee);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating your profile");
            }
        }


        [HttpPut("admin/update-employee/{employeeId}")]
        [Authorize(Roles = ApiRoles.Admin)]
        public async Task<ActionResult<EmployeeUserDto>> UpdateEmployeeByAdmin(int employeeId, EmployeeAdminUpdateDto dto)
        {
            try
            {
                var existingEmployee = await _employeeRepository.GetByIdAsync(employeeId);
                if (existingEmployee == null)
                {
                    return NotFound("Employee not found");
                }

                // Check if email is being changed and if it already exists
                if (existingEmployee.Email != dto.Email &&
                    await _employeeRepository.AnyAsync(e => e.Email == dto.Email && e.Id != employeeId))
                {
                    return BadRequest("An employee with this email already exists");
                }

                // Map the admin update DTO to the employee (includes HourlyRate)
                _mapper.Map(dto, existingEmployee);

                // Update ApiUser email if it exists and has changed
                if (existingEmployee.ApiUser != null && existingEmployee.ApiUser.Email != dto.Email)
                {
                    existingEmployee.ApiUser.Email = dto.Email;
                    existingEmployee.ApiUser.UserName = dto.Email;
                    existingEmployee.ApiUser.NormalizedEmail = dto.Email.ToUpper();
                    existingEmployee.ApiUser.NormalizedUserName = dto.Email.ToUpper();
                    await _userManager.UpdateAsync(existingEmployee.ApiUser);
                }

                await _employeeRepository.UpdateAsync(existingEmployee);
                var result = _mapper.Map<EmployeeUserDto>(existingEmployee);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the employee");
            }
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


        [HttpGet("admin/all-employees")]
        [Authorize(Roles = ApiRoles.Admin)]
        public async Task<ActionResult<IEnumerable<EmployeeUserDto>>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeRepository.GetAllEmployeesAsync();
                if (employees == null || !employees.Any())
                {
                    return Ok(new List<EmployeeUserDto>()); 
                }

                var response = _mapper.Map<IEnumerable<EmployeeUserDto>>(employees);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving employees");
            }
        }
    }
}
