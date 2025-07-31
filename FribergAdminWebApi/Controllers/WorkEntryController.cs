using FribergAdminWebApi.Constants;
using FribergAdminWebApi.Data.Dto;
using FribergAdminWebApi.Data.Interfaces;
using FribergAdminWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FribergAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkEntryController : ControllerBase
    {
        private readonly IWorkEntryRepository _workEntryRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public WorkEntryController(IWorkEntryRepository workEntryRepository, IEmployeeRepository employeeRepository)
        {
            _workEntryRepository = workEntryRepository;
            _employeeRepository = employeeRepository;
        }

        // GET: api/WorkEntry/my-entries
        [HttpGet("my-entries")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<List<WorkEntryDto>>> GetMyWorkEntries()
        {
            try
            {
                var userId = User.FindFirst(CustomClaimTypes.Uid)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in token");
                }

                var employee = await _employeeRepository.GetEmployeeByUserIdAsync(userId);
                if (employee == null)
                {
                    return NotFound("Employee not found");
                }

                var workEntries = await _workEntryRepository.GetWorkEntriesByEmployeeIdAsync(employee.Id);

                var workEntryDtos = workEntries.Select(we => new WorkEntryDto
                {
                    Id = we.Id,
                    Date = we.Date,
                    WorkDuration = we.WorkDuration,
                    EmployeeId = we.EmployeeId,
                    HourlyRateAtTimeOfWork = we.HourlyRateAtTimeOfWork,
                    EmployeeName = $"{we.Employee?.FirstName} {we.Employee?.LastName}"
                }).ToList();

                return Ok(workEntryDtos);
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong in {nameof(GetMyWorkEntries)}: {ex.Message}", statusCode: 500);
            }
        }

        // GET: api/WorkEntry (Admin only - all entries)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<WorkEntryDto>>> GetAllWorkEntries()
        {
            try
            {
                var workEntries = await _workEntryRepository.GetAllAsync();

                var workEntryDtos = workEntries.Select(we => new WorkEntryDto
                {
                    Id = we.Id,
                    Date = we.Date,
                    WorkDuration = we.WorkDuration,
                    EmployeeId = we.EmployeeId,
                    HourlyRateAtTimeOfWork = we.HourlyRateAtTimeOfWork,
                    EmployeeName = $"{we.Employee?.FirstName} {we.Employee?.LastName}"
                }).ToList();

                return Ok(workEntryDtos);
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong in {nameof(GetAllWorkEntries)}: {ex.Message}", statusCode: 500);
            }
        }

        // POST: api/WorkEntry
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<WorkEntryDto>> CreateWorkEntry(WorkEntryCreateDto createDto)
        {
            try
            {
                var userId = User.FindFirst(CustomClaimTypes.Uid)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User ID not found in token");
                }

                var employee = await _employeeRepository.GetEmployeeByUserIdAsync(userId);
                if (employee == null)
                {
                    return NotFound("Employee not found");
                }

                var workEntry = new WorkEntry
                {
                    Date = createDto.Date,
                    WorkDuration = createDto.WorkDuration,
                    EmployeeId = employee.Id,
                    HourlyRateAtTimeOfWork = employee.HourlyRate // Use current hourly rate
                };

                await _workEntryRepository.AddAsync(workEntry);

                var resultDto = new WorkEntryDto
                {
                    Id = workEntry.Id,
                    Date = workEntry.Date,
                    WorkDuration = workEntry.WorkDuration,
                    EmployeeId = workEntry.EmployeeId,
                    HourlyRateAtTimeOfWork = workEntry.HourlyRateAtTimeOfWork,
                    EmployeeName = $"{employee.FirstName} {employee.LastName}"
                };

                return CreatedAtAction(nameof(GetWorkEntryById), new { id = workEntry.Id }, resultDto);
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong in {nameof(CreateWorkEntry)}: {ex.Message}", statusCode: 500);
            }
        }

        // GET: api/WorkEntry/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkEntryDto>> GetWorkEntryById(int id)
        {
            try
            {
                var workEntry = await _workEntryRepository.GetByIdAsync(id);
                if (workEntry == null)
                {
                    return NotFound();
                }

                // Check if user is admin or owns this work entry
                var userId = User.FindFirst(CustomClaimTypes.Uid)?.Value;
                if (!User.IsInRole("Admin"))
                {
                    var employee = await _employeeRepository.GetEmployeeByUserIdAsync(userId);
                    if (employee == null || workEntry.EmployeeId != employee.Id)
                    {
                        return Forbid();
                    }
                }

                var workEntryDto = new WorkEntryDto
                {
                    Id = workEntry.Id,
                    Date = workEntry.Date,
                    WorkDuration = workEntry.WorkDuration,
                    EmployeeId = workEntry.EmployeeId,
                    HourlyRateAtTimeOfWork = workEntry.HourlyRateAtTimeOfWork,
                    EmployeeName = $"{workEntry.Employee?.FirstName} {workEntry.Employee?.LastName}"
                };

                return Ok(workEntryDto);
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong in {nameof(GetWorkEntryById)}: {ex.Message}", statusCode: 500);
            }
        }

        // DELETE: api/WorkEntry/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkEntry(int id)
        {
            try
            {
                var workEntry = await _workEntryRepository.GetByIdAsync(id);
                if (workEntry == null)
                {
                    return NotFound();
                }

                // Check if user is admin or owns this work entry
                var userId = User.FindFirst(CustomClaimTypes.Uid)?.Value;
                if (!User.IsInRole("Admin"))
                {
                    var employee = await _employeeRepository.GetEmployeeByUserIdAsync(userId);
                    if (employee == null || workEntry.EmployeeId != employee.Id)
                    {
                        return Forbid();
                    }
                }

                await _workEntryRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong in {nameof(DeleteWorkEntry)}: {ex.Message}", statusCode: 500);
            }
        }
    }
}