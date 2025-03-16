using System.ComponentModel.DataAnnotations;

namespace DocumentAPI.Models
{
    public class Documents
    {
        [Key]
        public int DocumentID {  get; set; }
        public string DocumentTitle { get; set; } 
        public string DocumentDescription { get; set; }
        public DocumentType Documenttype { get; set; }
        public enum DocumentType
        {
            Personal,
            Financial,
            Educational,
            Employee,
            Common
        }
    }
}
