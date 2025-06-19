using System.ComponentModel.DataAnnotations;

namespace UserDocumentAPI.Models.Dtos
{
    public class UploadUserDocument
    {
        public int UserDocumentID { get; set; }
        public string UserDocumentName { get; set; }
        public string DocumentURL { get; set; }
        public string UploadedBy { get; set; }
        public string UserEmail { get; set; }
    }
}
