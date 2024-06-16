using ImageInDbApi.Context;
using ImageInDbApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PostgresContext>();
builder.Services.AddAntiforgery();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
app.UseHttpsRedirection();


app.MapPost("/uploadimage", async (IFormFile file) =>
{
  MemoryStream memoryStream = new();
  file.CopyTo(memoryStream);
  DbImage dbImage = new();
  dbImage.DbImageBytea = memoryStream.ToArray();
  PostgresContext dbcontext = new();
  dbcontext.DbImages.Add(dbImage);
  dbcontext.SaveChanges();
  TypedResults.Ok();
}).DisableAntiforgery();
 
app.MapGet("/getimage{id}", async (int id, PostgresContext dbcontext) =>
{
  byte[] byteArray = dbcontext.DbImages.First(i => i.DbImageId == id).DbImageBytea;
  if (byteArray != null)
  {
    return Results.File(byteArray, "image/png");
  }
  else
  {
    return TypedResults.NotFound();
  }
  
});


app.Run();
