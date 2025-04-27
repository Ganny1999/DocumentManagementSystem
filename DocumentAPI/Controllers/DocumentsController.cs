using Azure;
using DocumentAPI.Data;
using DocumentAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Immutable;
using System.Threading.Tasks.Dataflow;

namespace DocumentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        public readonly AppDbContext _context;
        public readonly IMemoryCache _memoryCache;
        public DocumentsController(AppDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        [HttpGet("GetAllDocuments")]
        public ActionResult<IEnumerable<Documents>> GetAllDocuments()
        {
            // Before API call reaches to DB, cheack if cache data is exist or not.
            if (!_memoryCache.TryGetValue("DocumentDataList", out List<Documents> documents))
            {
                documents = _context.Document.ToList();
                _memoryCache.Set("DocumentDataList", documents, TimeSpan.FromMinutes(60));
                return documents;   
            }

            // if cache is expire or empty, API call reaches to DB.
            return documents;
        }
        [HttpPost("AddDocument")]
        public ActionResult<Documents> AddDocument([FromBody] Documents documents)
        {   
            if(documents == null)
            {
                return BadRequest("Data is empty, kindly provide the valid data.");
            }

            var isExists = _context.Document.FirstOrDefault(u=>u.DocumentTitle == documents.DocumentTitle);
            if(isExists == null)
            {
                _context.Document.Add(documents);
                _context.SaveChanges();
                var addedData = _context.Document.FirstOrDefault(u => u.DocumentTitle == documents.DocumentTitle);

                // Update the cache data
                var documentList = _context.Document.ToList();
                _memoryCache.Set("DocumentDataList", documentList, TimeSpan.FromMinutes(60));

                return Ok(addedData);
            }
            return BadRequest("Data already exists.");
        }
        [HttpPatch("UpdateDocument/{DocumentID:int}")]
        public async Task<ActionResult<Documents>> UpdateDocument(int DocumentID, [FromBody] JsonPatchDocument<Documents> patchDocument)
        {
            var docToEdit = await _context.Document.FirstOrDefaultAsync(u=>u.DocumentID==DocumentID);
            if(docToEdit == null)
            {
                return BadRequest();
            }
            patchDocument.ApplyTo(docToEdit);
            await _context.SaveChangesAsync();

            // Update the cache data
            var documentList = _context.Document.ToList();
            _memoryCache.Set("DocumentDataList", documentList, TimeSpan.FromMinutes(60));

            var updatedDoc = await _context.Document.FirstOrDefaultAsync(u=>u.DocumentID == DocumentID);

            return Ok(updatedDoc);
        }
    }
}