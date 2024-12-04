using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using React.Ecom.API.Utility;

namespace React.Ecom.API.Controllers
{
    [Route("api/AuthTest")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<string>> GetSomething()
        {
            return "You are authenticated";
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<ActionResult<string>> GetSomething(int someIntValue)
        {
            //authorization -> Authentication + Some access/roles
            return "You are Authorized with Role of Admin";
        }
    }
}
