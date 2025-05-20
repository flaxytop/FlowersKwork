using Microsoft.AspNetCore.Identity;

namespace Flowers.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; } = "user";
        public byte[]? Avatar { get; set; }
        public string MimeType { get; set; } = string.Empty;
    }
}