﻿using DocumentIdentityService.Models;
using DocumentIdentityService.Models.Dtos;
using DocumentIdentityService.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
                    return false;
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
                if (isLoginSuccess!=null)
                {
                    return Ok(isLoginSuccess);
                }
                else
                {
                    return "Invalid Credentials!!!";
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
                return false;
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
            var result = await _authService.GetFamilyMembers(FamilyID);
            if(result!=null)
            {
                return Ok(result);
            }
            return BadRequest("Something went wrong");
        }
        [Authorize(Roles = "MEMBER")]
        [HttpGet("GetUserByEmail")]
        public async Task<ActionResult<UserDto>> GetMemberByEmail(string email)
        {
            var result = await _authService.GetMemberByEmail(email);
            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("Something went wrong");
        }
    }
}