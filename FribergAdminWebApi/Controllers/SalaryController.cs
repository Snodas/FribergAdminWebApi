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
using System.Security.Claims;

namespace FribergAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController : ControllerBase //klar
    {
        private readonly ISalaryRepository _salaryRepository;
        private readonly IWorkEntryRepository _workEntryRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;

        public SalaryController(ISalaryRepository salaryRepository, IWorkEntryRepository workEntryRepository, IEmployeeRepository employeeRepository, IMapper mapper, UserManager<ApiUser> userManager)
        {
            _salaryRepository = salaryRepository;
            _workEntryRepository = workEntryRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _userManager = userManager;
        }



        [HttpGet("my-salaries/{year}")]
        //[Authorize(Roles = ApiRoles.Employee)]
        public async Task<ActionResult<IEnumerable<Salary>>> GetMySalaries(int year)
        {
            var userIdClaim = User.FindFirst(CustomClaimTypes.Uid);
            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
                return Unauthorized("User ID not found in token");

            var userId = userIdClaim.Value;
            var salaries = await _salaryRepository.GetSalariesByIdAndYearAsync(userId, year);
            return Ok(salaries);
        }

        [HttpGet("employee/{employeeId}/salaries")]
        //[Authorize(Roles = ApiRoles.Admin)]
        public async Task<ActionResult<IEnumerable<SalaryDto>>> GetSalariesByEmployeeId(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null) 
                return NotFound("Employee not found");

            var salaries = await _salaryRepository.GetSalariesByEmployeeIdAsync(employeeId);
            var salaryDtos = _mapper.Map<IEnumerable<SalaryDto>>(salaries);

            return Ok(salaryDtos);
        }

        [HttpGet("employee/{employeeId}/salaries/{year}")]
        //[Authorize(Roles = ApiRoles.Admin)]
        public async Task<ActionResult<IEnumerable<SalaryDto>>> GetEmployeeSalariesByYear(int employeeId, int year)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                return NotFound("Employee not found");

            var salaries = await _salaryRepository.GetSalariesByEmployeeAndYearAsync(employeeId, year);
            var salaryDtos = _mapper.Map<IEnumerable<SalaryDto>>(salaries);

            return Ok(salaryDtos);
        }
    }
}
