using BugTicketingSystem.DAL.Models;

namespace BugTicketingSystem.BL.Dtos.BugDtos
{
    public class BugAddDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public BugStatus Status { get; set; } = BugStatus.Open;
        public Guid ProjectId { get; set; }
    }
}
