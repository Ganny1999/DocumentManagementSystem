using System.ComponentModel.DataAnnotations;

namespace UserDocumentAPI.Models
{
    public class UserDocument
    {
        [Key]
        public int UserDocumentID {  get; set; }
        [Required]
        public string UserDocumentName { get; set;}
        [Required]
        public string UserDocumentType { get; set;}
        public string UserID { get; set;}
        public int FamilyID { get; set; }
        public string UploadedBy { get; set;}
        public string DocumentURL { get; set;}  
        public DateTime UploadedDateTime { get; set;}
    }
}
