using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController(ApplicationDBContext applecationDBContext) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> test()
        {
            return Ok( await applecationDBContext.Users.ToListAsync());
        }
    }
}
