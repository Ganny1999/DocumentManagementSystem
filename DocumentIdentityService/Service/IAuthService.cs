using DocumentIdentityService.Models;

namespace DocumentIdentityService.Service
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(UserRegistration userRegistration);
        Task<string> LoginUser(LoginUser loginUser);
        Task<bool> AddMember(AddMember addMember, string adminID);
        Task<bool> EnsureRoleCreateRole();
        Task<Family> GetFamilyMember(int familyID);
    }
}
