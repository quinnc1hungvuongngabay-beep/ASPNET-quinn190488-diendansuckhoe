using System.ComponentModel.DataAnnotations;

namespace HealthForumMVC.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Tiểu sử")]
        public string? Bio { get; set; }

        [Display(Name = "Avatar URL")]
        public string? AvatarUrl { get; set; }
    }
}