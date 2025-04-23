using System.Security.Claims;
using BugTicketingSystem.BL.Dtos.Common;
using BugTicketingSystem.BL.Dtos.ProjectDtos;
using BugTicketingSystem.BL.Managers.ProjectMananger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BugTicketingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IProjectManager _projectManager;
        public ProjectsController(IConfiguration configuration,
            IProjectManager projectManager)
        {
            _configuration = configuration;
            _projectManager = projectManager;
        }

        [HttpGet]
        public async Task<Ok<List<ProjectReadDto>>>
            GetAllProjects()
        {
            var projects = await _projectManager.GetAllProjectsAsync();
            return TypedResults.Ok(projects);
        }
        [HttpGet("{id}")]
        public async Task<Results<Ok<ProjectReadDto>, NotFound>>
            GetProjectById(Guid id)
        {
            var project = await _projectManager.GetProjectByIdAsync(id);
            if (project == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(project);
        }
        [HttpPost]
        public async Task<ActionResult<GeneralResult>>
            Add([FromBody] ProjectAddDto projectAddDto)
        {
            var result = await _projectManager.AddProjectAsync(projectAddDto);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
