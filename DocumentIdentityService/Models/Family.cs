using DocumentIdentityService.Models.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentIdentityService.Models
{
    public class Family
    {
        public int FamilyID { get; set; }
        [Required]
        public string FamilyName { get; set; }
        [Required]
        public string FamilyAdminID {  get; set; }
        [ForeignKey("FamilyAdminID")]
        public User AdminUser { get; set; }
        [NotMapped]
        public IEnumerable<UserDto> Users { get; set; }
    }
}
