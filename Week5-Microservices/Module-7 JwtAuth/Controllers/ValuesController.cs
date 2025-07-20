using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Module_7_JwtAuth.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok( new
            {
                Message = "This is a protected resource. You have access because you provided a valid JWT token.",
                Username = User.Identity?.Name
            });
        }
    }
}
