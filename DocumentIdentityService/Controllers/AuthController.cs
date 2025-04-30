using DocumentIdentityService.Models;
using DocumentIdentityService.Models.Dtos;
using DocumentIdentityService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections;


namespace DocumentIdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("RegisterUser")]
        public async Task<ActionResult<bool>> RegisterUser([FromBody] UserRegistration userRegistration)
        {
            if(userRegistration != null)
            {                 
                var isRegistrationSuccess = await _authService.RegisterUser(userRegistration);
                if(isRegistrationSuccess)
                {
                    return Ok(isRegistrationSuccess);
                }
                else
                {
                    return Ok(false);
                }
            }
            return BadRequest("Something went wrong!");
        }

        [HttpPost("LoginUser")]
        public async Task<ActionResult<string>> LoginUser([FromBody] LoginUser loginUser)
        {
            if (loginUser != null)
            {
                var isLoginSuccess = await _authService.LoginUser(loginUser);

                // Session - Storing small user-specific data across multiple requests.
                HttpContext.Session.SetString("token-key", isLoginSuccess);
                HttpContext.Session.SetString("login-user", JsonConvert.SerializeObject(loginUser));

                var str = HttpContext.Session.GetString("token-key");

                if (isLoginSuccess!=null)
                {
                    return Ok(isLoginSuccess);
                }
                else
                {
                    return BadRequest("Invalid Credentials!!!");
                }
            }
            return BadRequest("Something went wrong!");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("AddMemberas")]
        public async Task<ActionResult<bool>> AddMemberas([FromBody] AddMember addMember,string AdminID)
        {
           var isAddedUser= await _authService.AddMember(addMember,AdminID);
            if (isAddedUser)
            {
                return Ok(isAddedUser);
            }
            else
            {
                return Ok(false);
            } 
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet("EnsureRoleCreated")]
        public async Task<ActionResult<bool>> EnsureRoleCreated()
        { 
           var result =  await _authService.EnsureRoleCreateRole();

            return result;
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet("GetFamilyMember/{FamilyID:int}")]
        public async Task<ActionResult<Family>> GetFamilyMember(int FamilyID)
        {
            // Get the logged in user details and compre with Family Admin user, if maches then return the Family member details
            var user = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Email);
            var result = await _authService.GetFamilyMembers(FamilyID);
            if(result!=null && result.AdminUser.Email == user.Value)
            {
                return Ok(result);
            }
            return BadRequest("Something went wrong");
        }
        [Authorize(Roles = "MEMBER,ADMIN")]
        [HttpGet("GetUserByEmail/{email}")]
        public async Task<ActionResult<UserDto>> GetMemberByEmail(string email)
        {
            // Only Logged in member can able fetch his own details
            var user = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.Email); 
            var result = await _authService.GetMemberByEmail(email);
            if (result != null && result.Email == user.Value)
            {
                return Ok(result);
            }
            return BadRequest("Something went wrong");
        }
        // Chaneg from remote
        [HttpGet("Logout")]
        public ActionResult<string> LoggedOut()
        {
            HttpContext.Session.Clear();
            return Ok("Logout successful.");
        }
    }
}
