using System.ComponentModel.DataAnnotations;

namespace HealthForumMVC.Models
{
    public class Post
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        [Required]
        public string AuthorId { get; set; } = string.Empty;
        
        public ApplicationUser Author { get; set; } = null!;
        
        [Required]
        public string CategoryId { get; set; } = string.Empty;
        
        public Category Category { get; set; } = null!;
        
        public int ViewCount { get; set; } = 0;
        
        public int LikeCount { get; set; } = 0;
        
        public int CommentCount { get; set; } = 0;
        
        public bool IsPinned { get; set; } = false;
        
        public bool IsLocked { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}