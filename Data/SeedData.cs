using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HealthForumMVC.Models;

namespace HealthForumMVC.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            // Tạo admin user
            if (!await userManager.Users.AnyAsync(u => u.UserName == "admin"))
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@healthforum.com",
                    FullName = "Quản trị viên",
                    Role = UserRole.Admin,
                    Reputation = 5000,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Admin123!");
            }

            // Tạo demo users
            var demoUsers = new[]
            {
                new { Username = "bac_si_nam", Email = "doctor@example.com", FullName = "BS. Nguyễn Văn Nam", Role = UserRole.Admin },
                new { Username = "me_hai_con", Email = "mom@example.com", FullName = "Lê Thị Hạnh", Role = UserRole.User },
                new { Username = "huan_luyen_vien", Email = "trainer@example.com", FullName = "Trần Minh Tuấn", Role = UserRole.User }
            };

            foreach (var userData in demoUsers)
            {
                if (!await userManager.Users.AnyAsync(u => u.UserName == userData.Username))
                {
                    var user = new ApplicationUser
                    {
                        UserName = userData.Username,
                        Email = userData.Email,
                        FullName = userData.FullName,
                        Role = userData.Role,
                        Reputation = userData.Role == UserRole.Admin ? 2500 : 850,
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(user, "Demo123!");
                }
            }

            // Tạo demo posts
            if (!context.Posts.Any())
            {
                var adminUser = await userManager.FindByNameAsync("admin");
                var doctorUser = await userManager.FindByNameAsync("bac_si_nam");
                var momUser = await userManager.FindByNameAsync("me_hai_con");
                var trainerUser = await userManager.FindByNameAsync("huan_luyen_vien");

                var demoPosts = new[]
                {
                    new Post
                    {
                        Title = "10 thực phẩm tăng cường hệ miễn dịch hiệu quả nhất",
                        Content = "Hệ miễn dịch mạnh mẽ là chìa khóa để duy trì sức khỏe tốt. Dưới đây là 10 thực phẩm được khoa học chứng minh có khả năng tăng cường hệ miễn dịch:\n\n1. Cam, chanh: Giàu vitamin C\n2. Tỏi: Chứa allicin kháng khuẩn\n3. Gừng: Chống viêm tự nhiên\n4. Rau xanh: Cung cấp vitamin A, C, E\n5. Sữa chua: Probiotic tốt cho đường ruột\n\nHãy bổ sung những thực phẩm này vào chế độ ăn hàng ngày để có sức khỏe tốt nhất!",
                        AuthorId = doctorUser?.Id ?? "",
                        CategoryId = "1",
                        ViewCount = 1245,
                        LikeCount = 89,
                        CommentCount = 23,
                        IsPinned = true,
                        CreatedAt = DateTime.UtcNow.AddDays(-5),
                        UpdatedAt = DateTime.UtcNow.AddDays(-4)
                    },
                    new Post
                    {
                        Title = "Trầm cảm sau sinh - Dấu hiệu nhận biết và cách điều trị",
                        Content = "Trầm cảm sau sinh là tình trạng phổ biến ảnh hưởng đến 10-20% phụ nữ sau khi sinh. Các dấu hiệu cần chú ý:\n\n• Buồn bã kéo dài hơn 2 tuần\n• Mất hứng thú với mọi hoạt động\n• Cảm giác tội lỗi, vô giá trị\n• Khó ngủ hoặc ngủ quá nhiều\n• Thay đổi cảm xúc đột ngột\n\nNếu có các triệu chứng này, hãy tìm đến sự hỗ trợ từ gia đình và bác sĩ chuyên khoa.",
                        AuthorId = momUser?.Id ?? "",
                        CategoryId = "2",
                        ViewCount = 892,
                        LikeCount = 67,
                        CommentCount = 45,
                        CreatedAt = DateTime.UtcNow.AddDays(-3),
                        UpdatedAt = DateTime.UtcNow.AddDays(-3)
                    },
                    new Post
                    {
                        Title = "Chế độ ăn Keto có thực sự hiệu quả cho giảm cân?",
                        Content = "Chế độ ăn Keto (Ketogenic) đang rất được quan tâm trong cộng đồng giảm cân. Đây là chế độ ăn ít carb, nhiều chất béo:\n\n**Ưu điểm:**\n- Giảm cân nhanh trong giai đoạn đầu\n- Kiểm soát đường huyết tốt\n- Giảm cảm giác thèm ăn\n\n**Nhược điểm:**\n- Có thể gây mệt mỏi ban đầu\n- Khó duy trì lâu dài\n- Cần theo dõi y tế\n\nTrước khi áp dụng, nên tham khảo ý kiến bác sĩ dinh dưỡng.",
                        AuthorId = trainerUser?.Id ?? "",
                        CategoryId = "3",
                        ViewCount = 654,
                        LikeCount = 42,
                        CommentCount = 28,
                        CreatedAt = DateTime.UtcNow.AddDays(-2),
                        UpdatedAt = DateTime.UtcNow.AddDays(-2)
                    },
                    new Post
                    {
                        Title = "Tập thể dục tại nhà hiệu quả với 15 phút mỗi ngày",
                        Content = "Không có thời gian đến phòng gym? Đây là bộ bài tập 15 phút tại nhà:\n\n**Khởi động (3 phút):**\n- Chạy tại chỗ: 1 phút\n- Xoay vai, cổ: 1 phút\n- Duỗi cơ: 1 phút\n\n**Bài tập chính (10 phút):**\n- Squat: 30 giây x 3 hiệp\n- Push-up: 30 giây x 3 hiệp\n- Plank: 30 giây x 2 hiệp\n- Burpee: 30 giây x 2 hiệp\n\n**Thư giãn (2 phút):**\n- Duỗi cơ toàn thân\n\nKiên trì 30 ngày sẽ thấy kết quả rõ rệt!",
                        AuthorId = trainerUser?.Id ?? "",
                        CategoryId = "1",
                        ViewCount = 1156,
                        LikeCount = 98,
                        CommentCount = 34,
                        CreatedAt = DateTime.UtcNow.AddDays(-1),
                        UpdatedAt = DateTime.UtcNow.AddDays(-1)
                    },
                    new Post
                    {
                        Title = "Stress và cách quản lý hiệu quả trong cuộc sống hiện đại",
                        Content = "Stress là vấn đề phổ biến trong xã hội hiện đại. Dưới đây là những cách quản lý stress hiệu quả:\n\n**1. Kỹ thuật th呼吸:**\n- Hít thở sâu 4-7-8\n- Thiền chánh niệm 10 phút/ngày\n\n**2. Hoạt động thể chất:**\n- Đi bộ 30 phút\n- Yoga buổi sáng\n\n**3. Quản lý thời gian:**\n- Lập danh sách ưu tiên\n- Học cách nói 'không'\n\n**4. Kết nối xã hội:**\n- Chia sẻ với bạn bè\n- Tham gia hoạt động nhóm\n\nHãy thử áp dụng và chia sẻ kết quả nhé!",
                        AuthorId = adminUser?.Id ?? "",
                        CategoryId = "2",
                        ViewCount = 743,
                        LikeCount = 56,
                        CommentCount = 19,
                        CreatedAt = DateTime.UtcNow.AddHours(-12),
                        UpdatedAt = DateTime.UtcNow.AddHours(-12)
                    }
                };

                context.Posts.AddRange(demoPosts);
                await context.SaveChangesAsync();
            }
        }
    }
}