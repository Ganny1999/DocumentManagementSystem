namespace DocumentIdentityService.Models
{
    public class AddMember
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Passward { get; set; }
        public string Role { get; set; } = "Member";
    }
}
