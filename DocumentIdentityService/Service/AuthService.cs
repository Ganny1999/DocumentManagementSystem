using AutoMapper;
using DocumentIdentityService.DataContext;
using DocumentIdentityService.Models;
using DocumentIdentityService.Models.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;

namespace DocumentIdentityService.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthService(AppDbContext appDbContext, UserManager<User> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _configuration = configuration;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<string> LoginUser(LoginUser loginUser)
        {
            if (loginUser != null)
            {
                var isEmailValid = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserName == loginUser.Email);
                if (isEmailValid != null)
                {
                    var result = await _userManager.CheckPasswordAsync(isEmailValid, loginUser.Passward);
                    if (result)
                    {
                        var jwtToken = Generatoken(loginUser);
                        return jwtToken;
                    }
                }
                return null;
            }
            return null;
        }

        public async Task<bool> RegisterUser(UserRegistration userRegistration)
        {
            if (userRegistration != null)
            {
                var isExsists = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == userRegistration.Email);
                if (isExsists == null)
                {
                    var user = new User()
                    {
                        FirstName = userRegistration.FirstName,
                        LastName = userRegistration.LastName,
                        Email = userRegistration.Email,
                        PhoneNumber = userRegistration.Phone,
                        UserName = userRegistration.Email,
                        FamilyAdminID = ""
                    };
                    var result = await _userManager.CreateAsync(user, userRegistration.Passward);

                    var isUserAdded = await _appDbContext.Users.FirstOrDefaultAsync(u=>u.Email==userRegistration.Email);
                    if (isUserAdded != null)
                    {
                        isUserAdded.FamilyAdminID = isUserAdded.Id;
                        //_appDbContext.Users.Update(isUserAdded);
                    }
                    await _appDbContext.SaveChangesAsync();

                    if(!result.Succeeded)
                    {
                        return false;
                    }
                    // Admin User is created  now Create a family entry 
                    var isAdminUserCreated = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserName == userRegistration.Email);
                    if (isAdminUserCreated != null)
                    {
                        var family = new Family()
                        {
                            FamilyAdminID = isAdminUserCreated.Id,
                            FamilyName = $"{user.LastName}" + $"{isAdminUserCreated.Id}",
                        };
                        _appDbContext.FamilyAdmins.Add(family);
                        _appDbContext.SaveChanges();
                    }
                    await _userManager.AddToRoleAsync(user,userRegistration.Role);

                    return result.Succeeded;
                }
            }
            return false;
        }

        public string Generatoken(LoginUser loginUser)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Token:Key").Value));

            var signCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var claimList = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,loginUser.Email),
                new Claim(ClaimTypes.Role,loginUser.Role)
                
            };
            var securityToken = new JwtSecurityToken(
                claims: claimList,
                signingCredentials: signCredentials,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _configuration.GetSection("Token:Issuer").Value,
                audience: _configuration.GetSection("Token:Audience").Value
            );
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

        public async Task<bool> AddMember(AddMember addMember, string adminID)
        {
            if (addMember != null)
            {
                var isExsists = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == addMember.Email);

                if (isExsists == null)
                {
                    var user = new User()
                    {
                        FirstName = addMember.FirstName,
                        LastName = addMember.LastName,
                        Email = addMember.Email,
                        PhoneNumber = addMember.Phone,
                        UserName = addMember.Email,
                        FamilyAdminID = adminID 
                    };
                    var result = await _userManager.CreateAsync(user, addMember.Passward);
                    await _userManager.AddToRoleAsync(user, "MEMBER");

                    return result.Succeeded;
                }
                return false;
            }
            return false;
        }
        public async Task<bool> EnsureRoleCreateRole()
        {
            try
            {
                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin.ToUpper()));
                }
                if (!await _roleManager.RoleExistsAsync(UserRoles.Member))
                {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Member.ToUpper()));
                }
                return true;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<Family> GetFamilyMember(int familyID)
        {
            if(familyID < 0 || familyID>int.MaxValue)
            {
                return null;
            }
            var SearchFamily = await _appDbContext.FamilyAdmins.FirstOrDefaultAsync(u=>u.FamilyID == familyID);
            if(SearchFamily!=null)
            {
                var Users = _appDbContext.Users.Where(u => u.FamilyAdminID == SearchFamily.FamilyAdminID).ToList();
                var UsersDtoList = _mapper.Map<List<UserDto>>(Users);

                SearchFamily.Users = UsersDtoList;
                return SearchFamily;
            }
            throw new ArgumentException(nameof(Family), "Family list id null");
        }
    }
}