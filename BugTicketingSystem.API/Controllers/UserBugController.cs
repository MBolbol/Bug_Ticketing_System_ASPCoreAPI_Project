using BugTicketingSystem.BL.Managers.BugUserManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BugTicketingSystem.API.Controllers
{
    [Route("api/bugs/{bugId}/assignees")]
    [ApiController]
    public class UserBugController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IBugUserManager _bugUserManager;
        public UserBugController(IConfiguration configuration, IBugUserManager bugUserManager)
        {
            _configuration = configuration;
            _bugUserManager = bugUserManager;

        }
        [HttpPost]
        public async Task<IActionResult> AssignUserToBug([FromQuery] Guid bugId, [FromQuery] Guid userId)
        {
            try
            {
                await _bugUserManager.AssignUserToBugAsync(bugId, userId);
                return Ok(new { message = "User assigned to bug successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete("{userId}")]
        public async Task<IActionResult> UnAssignUserToBug([FromRoute] Guid bugId, [FromRoute] Guid userId)
        {
            try
            {
                await _bugUserManager.UnAssignUserToBugAsync(bugId, userId);
                return Ok(new { message = "User unassigned from bug successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
