using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flowers.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        [Authorize(Policy = "admin")]
        [HttpGet("secret")]
        public IActionResult AdminOnly()
        {
            return Ok("This is admin-only content");
        }
    }
}
