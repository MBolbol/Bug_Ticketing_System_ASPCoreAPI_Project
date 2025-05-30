﻿using BugTicketingSystem.DAL.Models;

namespace BugTicketingSystem.BL.Dtos.UserDtos
{
    public class UserUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
