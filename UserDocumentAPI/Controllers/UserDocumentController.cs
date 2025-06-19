using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using UserDocumentAPI.Models;
using UserDocumentAPI.Models.Dtos;

namespace UserDocumentAPI.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class UserDocumentController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserDocumentController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<ActionResult<UserDocument>> GetUserDocument()
        {
            var userDocument = new UserDocument();
            return Ok(userDocument);
        }
        [HttpPost("UploadDocument")]
        public async Task<ActionResult<UserDocument>> UploadDocument([FromBody] UploadUserDocument uploadUserDocument)
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            var httpDocClient = _httpClientFactory.CreateClient("document");
            var httpUserClient = _httpClientFactory.CreateClient("user");

             if (!string.IsNullOrEmpty(accessToken))
            
            {
                httpUserClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken.Replace("Bearer ", ""));
            }


            var docResponse = await httpDocClient.GetAsync($"/api/Documents/GetDocumentByName/{uploadUserDocument.UserDocumentName}");
            var userResponse = await httpUserClient.GetAsync($"/api/Auth/GetUserByEmail/{uploadUserDocument.UserEmail}");

            var contentDoc = await docResponse.Content.ReadAsStringAsync();
            var contentUser = await userResponse.Content.ReadAsStringAsync();
            var docresp = JsonConvert.DeserializeObject<DocumentDto>(contentDoc);
            var userresp = JsonConvert.DeserializeObject<UserDto>(contentUser);

            var userDocument = new UserDocument();
            userDocument.UserDocumentName = docresp.DocumentTitle;
            userDocument.UserDocumentType = docresp.Documenttype.ToString();
            userDocument.UploadedDateTime = DateTime.Now;
            userDocument.DocumentURL = uploadUserDocument.DocumentURL;
            userDocument.UploadedBy = userresp.FirstName +" "+ userresp.LastName;
            userDocument.UserID = userresp.UserID;
            userDocument.FamilyID = 0;
            return Ok(userDocument);
        }

    }
}
