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
        //[Authorize(Roles = ApiRoles.Employee)]
        public async Task<ActionResult<EmployeeUserDto>> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(CustomClaimTypes.Uid);
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                return Unauthorized("User ID not found in token");

            var employee = await _employeeRepository.GetByUserIdAsync(userIdClaim.Value);
            if (employee == null)
                return NotFound("Employee not found");

            var profileDto = _mapper.Map<EmployeeUserDto>(employee);
            return Ok(profileDto);
        }

        //[HttpPut("update-my-profile")]
        //[Authorize(Roles = ApiRoles.Employee)]
        //public async Task<ActionResult<EmployeeUserDto>> UpdateMyProfile(EmployeeUserDto dto)
        //{
        //    try
        //    {
        //        var userIdClaim = User.FindFirst(CustomClaimTypes.Uid);
        //        if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value)) 
        //            return Unauthorized("User ID not found in token");

        //        var employee = await _employeeRepository.GetByUserIdAsync(userIdClaim.Value);
        //        if (employee == null)
        //            return NotFound("Employee profile not found");

        //        if (employee.Email != dto.Email &&
        //            await _employeeRepository.AnyAsync(e => e.Email == dto.Email))
        //        {
        //            return BadRequest("An employee with this email already exists.");
        //        }

        //        _mapper.Map(dto, employee);

        //        if (employee.ApiUser != null && employee.ApiUser.Email != dto.Email)
        //        {
        //            employee.ApiUser.Email = dto.Email;
        //            employee.ApiUser.UserName = dto.Email;
        //            employee.ApiUser.NormalizedEmail = dto.Email.ToUpper();
        //            employee.ApiUser.NormalizedUserName = dto.Email.ToUpper();
        //            await _userManager.UpdateAsync(employee.ApiUser);
        //        }

        //        var updatedEmployee = await _employeeRepository.UpdateAsync(employee);
        //        var result = _mapper.Map<EmployeeProfileDto>(updatedEmployee);

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "An error occurred while updating the employee profile");
        //    }
        //}


        //[HttpPut("{employeeId}/edit")]
        ////[Authorize(Roles = $"{ApiRoles.Admin}, {ApiRoles.Employee}")]
        //public async Task<ActionResult<Employee>> UpdateEmployee(int employeeId, EmployeeUserDto dto)
        //{
        //    try
        //    {
        //        var existingEmployee = await _employeeRepository.GetByIdAsync(employeeId);
        //        if (existingEmployee == null)
        //        {
        //            return NotFound("Employee not found");
        //        }

        //        if (existingEmployee.Email != dto.Email &&
        //            await _employeeRepository.AnyAsync(e => e.Email == dto.Email))
        //        {
        //            return BadRequest("An employee with this email already exists.");
        //        }

        //        _mapper.Map(dto, existingEmployee);
        //        existingEmployee.Id = employeeId;

        //        var updatedEmployee = await _employeeRepository.UpdateAsync(existingEmployee);

        //        return Ok(updatedEmployee);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "An error occurred while updating the employee");
        //    }
        //}


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
        //[Authorize(Roles = ApiRoles.Admin)]
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
