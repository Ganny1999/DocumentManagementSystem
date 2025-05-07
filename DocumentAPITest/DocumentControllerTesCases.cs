using DocumentAPI.Controllers;
using DocumentAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAPITest
{
    public class DocumentControllerTesCases
    {
        // Create In-Memory database to test

        private AppDbContext GetInMemoryDatabse()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();
            if(context.Document.Count() >=0)
            {
                for(int i=1;i<=5;i++)
                {
                    context.Document.Add(
                        new DocumentAPI.Models.Documents()
                        {
                            DocumentID= i,
                            DocumentDescription= "Document Description " + i.ToString(),
                            DocumentTitle = $"Title{i}",
                            Documenttype = DocumentAPI.Models.Documents.DocumentType.Personal
                        });
                    context.SaveChanges();
                } 
            }
            return context;
        }

        [Fact]
        public void Get_All_Documents_Success()
        {
            var _context = GetInMemoryDatabse();
            var _mockCache = new Mock<IMemoryCache>();

            var _controller = new DocumentsController(_context,_mockCache.Object);

            var result = _controller.GetAllDocuments();
            var okResult =  Assert.IsType<OkObjectResult>(result.Result);

            Assert.Equal(5,okResult.Value);
        }
    }
}
