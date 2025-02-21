using System.ComponentModel.DataAnnotations;

namespace DocumentIdentityService.Models.Dtos
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Contact {  get; set; }
    }
}
