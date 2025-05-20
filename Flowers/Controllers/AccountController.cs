using Flowers.Data;
using Flowers.DTOs;
using Flowers.Services;
using Flowers.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Flowers.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            AuthService authService,
            ApplicationDbContext context,
            ILogger<AccountController> logger)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var loginDto = new LoginDto
                {
                    Email = model.Email,
                    Password = model.Password
                };

                var response = await _authService.LoginAsync(loginDto);
                if (response.Success)
                {
                    _logger.LogInformation("User logged in successfully");
                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError(string.Empty, response.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                ModelState.AddModelError(string.Empty, "An error occurred during login");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var registerDto = new RegisterDto
                {
                    Email = model.Email,
                    Password = model.Password,
                    Role = "user"
                };

                var response = await _authService.RegisterAsync(registerDto);
                if (response.Success)
                {
                    _logger.LogInformation("User registered successfully");
                    return RedirectToAction(nameof(Login));
                }

                ModelState.AddModelError(string.Empty, response.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                ModelState.AddModelError(string.Empty, "An error occurred during registration");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            _logger.LogInformation("User logged out");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("/Account/UserAvatar/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> UserAvatar([FromServices] UserManager<Flowers.Entities.ApplicationUser> userManager, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user?.Avatar != null && !string.IsNullOrEmpty(user.MimeType))
            {
                return File(user.Avatar, user.MimeType);
            }
            // Вернуть дефолтную картинку, если аватара нет
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", "default-avatar.jpg");
            var image = await System.IO.File.ReadAllBytesAsync(path);
            return File(image, "image/jpeg");
        }

        [Authorize]
        public async Task<IActionResult> UserInfo([FromServices] UserManager<Flowers.Entities.ApplicationUser> userManager)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();
            return View(user);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
    }
} 