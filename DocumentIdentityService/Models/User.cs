using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DocumentIdentityService.Models
{
    public class User:IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FamilyAdminID { get; set; }  = ""; // if 0 it is Admin User.
    }
}