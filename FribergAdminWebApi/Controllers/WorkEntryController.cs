using AutoMapper;
using FribergAdminWebApi.Data;
using FribergAdminWebApi.Data.Dto;
using FribergAdminWebApi.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FribergAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkEntryController : ControllerBase
    {
        private readonly IWorkEntryRepository _workEntryRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;

        public WorkEntryController(IWorkEntryRepository workEntryRepository, IEmployeeRepository employeeRepository, IMapper mapper, UserManager<ApiUser> userManager)
        {
            this._workEntryRepository = workEntryRepository;
            this._employeeRepository = employeeRepository;
            this._mapper = mapper;
            this._userManager = userManager;
        }

        [HttpGet("allworkentries")]
        public async Task<ActionResult<IEnumerable<WorkEntryDto>>> GetAllWorkEntries()
        {
            return NotFound("Not implemented yet");
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<WorkEntryDto>>> GetAllWorkEntriesByEmployeeID()
        {
            return NotFound("Not implemented yet");
        }       
    }
}
