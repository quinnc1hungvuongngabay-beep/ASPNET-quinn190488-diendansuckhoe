using System.ComponentModel.DataAnnotations;

namespace HealthForumMVC.Models
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public string Icon { get; set; } = string.Empty;
        
        public string Color { get; set; } = string.Empty;
        
        public int PostCount { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}