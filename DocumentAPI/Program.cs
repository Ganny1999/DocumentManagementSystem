using DocumentAPI.Data;
using DocumentAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();

// Service registration for DbContext
builder.Services.AddDbContext<AppDbContext>(options=>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
});

//
builder.Services.AddMemoryCache();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Minimal Web API code snippet for Get, Post & Put.
List<Documents> listdoc = new();

app.MapGet("/GetDoc",()=> listdoc);

app.MapPost("AddDoc", (Documents doc) => {
    listdoc.Add(doc);
    return Results.Ok(doc);
});

app.MapGet("GetDocByID", (int DocID) =>
{
    var doc=  listdoc.Find(u => u.DocumentID == DocID);
    if(doc==null)
        return Results.NotFound();
    return Results.Ok(doc);
});
app.MapPut("PutDoc", (int DocID, Documents doc) =>
{
    var docFound = listdoc.Find(u => u.DocumentID == DocID);
    if(docFound==null)
        return Results.NotFound();
    docFound.DocumentTitle=doc.DocumentTitle;
    docFound.Documenttype = doc.Documenttype;
    docFound.DocumentDescription = doc.DocumentDescription;
    return Results.Ok(docFound);
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
