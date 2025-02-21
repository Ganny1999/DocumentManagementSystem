using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentIdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleBaseAuthTestController : ControllerBase
    {
        [HttpGet("AdminCanAccess")]
        [Authorize(Roles ="ADMIN")]
        public string AdminCanAccess()
        {
            return "Admin can access";
        }
        [HttpGet("UserCanAccess")]
        [Authorize(Roles = "MEMBER")]
        public string UserCanAccess()
        {
            return "Member can access";
        }
    }
}
