using Flowers.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flowers.Controllers
{
    public class AvatarController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        IWebHostEnvironment _env;
        public AvatarController(UserManager<ApplicationUser>
       userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _env = env;
        }
        public async Task<FileResult> GetAvatar()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user?.Avatar != null && user.Avatar.Length > 0)
            {
                return File(user.Avatar, string.IsNullOrEmpty(user.MimeType) ? "image/jpeg" : user.MimeType);
            }
            else
            {
                var avatarPath = "/Images/anonymous.png";
                return File(_env.WebRootFileProvider.GetFileInfo(avatarPath).CreateReadStream(), "image/png");
            }
        }

    }
}
 