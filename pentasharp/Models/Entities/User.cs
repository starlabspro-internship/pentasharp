using System;

namespace pentasharp.Models.Entities
{
    public class User
    {
        public int UserId { get; set; } 
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Role { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;
    }
}