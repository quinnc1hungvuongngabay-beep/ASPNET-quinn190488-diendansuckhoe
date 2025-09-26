using System.ComponentModel.DataAnnotations;

namespace HealthForumMVC.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string FullName { get; set; } = string.Empty;
        
        public string? AvatarUrl { get; set; }
        
        public string? Bio { get; set; }
        
        public int Reputation { get; set; } = 0;
        
        [Required]
        public UserRole Role { get; set; } = UserRole.User;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime LastActive { get; set; } = DateTime.UtcNow;
        
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }

    public enum UserRole
    {
        User,
        Moderator,
        Admin
    }
}