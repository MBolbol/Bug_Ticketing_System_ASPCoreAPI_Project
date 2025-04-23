using BugTicketingSystem.BL.Dtos.BugDtos;
using BugTicketingSystem.BL.Dtos.Common;
using BugTicketingSystem.BL.Managers.BugManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BugTicketingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IBugManager _bugManager;
        public BugsController(IConfiguration configuration,
            IBugManager bugManager)
        {
            _configuration = configuration;
            _bugManager = bugManager;
        }

        [HttpGet]
        public async Task<Ok<List<BugReadDto>>>
            GetAllProjects()
        {
            var projects = await _bugManager.GetAllBugsAsync();
            return TypedResults.Ok(projects);
        }
        [HttpGet("{id}")]
        public async Task<Results<Ok<BugReadDto>, NotFound>>
            GetProjectById(Guid id)
        {
            var project = await _bugManager.GetBugByIdAsync(id);
            if (project == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(project);
        }
        //[Authorize]
        [HttpPost]
        public async Task<Results<Ok<GeneralResult>, BadRequest<GeneralResult>>> 
            Add([FromBody] BugAddDto bugAddDto)
        {
            var result = await _bugManager.AddBugAsync(bugAddDto);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }
    }
}
