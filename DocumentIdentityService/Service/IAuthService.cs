using DocumentIdentityService.Models;
using DocumentIdentityService.Models.Dtos;

namespace DocumentIdentityService.Service
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(UserRegistration userRegistration);
        Task<string> LoginUser(LoginUser loginUser);
        Task<bool> AddMember(AddMember addMember, string adminID);
        Task<bool> EnsureRoleCreateRole();
        Task<Family> GetFamilyMembers(int familyID);
        Task<UserDto> GetMemberByEmail(string email);
    }
}
