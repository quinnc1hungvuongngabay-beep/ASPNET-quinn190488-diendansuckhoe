using System.ComponentModel.DataAnnotations;

namespace HealthForumMVC.Models
{
    public class Comment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        [Required]
        public string PostId { get; set; } = string.Empty;
        
        public Post Post { get; set; } = null!;
        
        [Required]
        public string AuthorId { get; set; } = string.Empty;
        
        public ApplicationUser Author { get; set; } = null!;
        
        public int LikeCount { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}