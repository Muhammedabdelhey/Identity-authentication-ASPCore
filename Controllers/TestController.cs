using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController(ApplicationDBContext applecationDBContext) : ControllerBase
    {
        [HttpGet]
        // how make authorize based on roles 
        [Authorize(Roles = "Admin")]
        // how make Authorization based on policy , policy can have mane role relation will be role1 or role2
        //[Authorize(policy: "AdminOrUser")]
        public async Task<IActionResult> Test()
        {

            return Ok(await applecationDBContext.Users.ToListAsync());
        }
    }
}
