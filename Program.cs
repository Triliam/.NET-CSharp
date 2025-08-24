
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//cria a aplicação web - hosting - escuta o que é que o user quer acessar
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);


app.MapPost("/products", (ProductRequestDTO productRequestDTO, ApplicationDbContext context) =>
{
    var category = context.Categories.Where(c => c.Id == productRequestDTO.CategoryId).First();

    var product = new Product
    {
        Code = productRequestDTO.Code,
        Name = productRequestDTO.Name,
        Category = category
    };

    if (productRequestDTO.Tags != null)
    {
        product.Tags = new List<Tag>();
        foreach (var item in productRequestDTO.Tags)
        {
            product.Tags.Add(new Tag { Name = item });
        }
    }

    context.Products.Add(product);
    context.SaveChanges();
    return Results.Created("/products", product.Code);
});


app.MapGet("/products/{id}", ([FromRoute] int id, ApplicationDbContext context) =>
{
    var product = context.Products.Include(p => p.Category).Include(p => p.Tags).Where(product => product.Id == id).First();
    if (product != null)
    {
        return Results.Ok(product);
    }
    return Results.NotFound();
});

app.MapPut("/products{id}", ([FromRoute] int id, ProductRequestDTO productRequestDTO, ApplicationDbContext context) =>
{
    var product = context.Products.Include(p => p.Category).Include(p => p.Tags).Where(product => product.Id == id).First();
    var category = context.Categories.Where(c => c.Id == productRequestDTO.CategoryId).First();
    product.Code = productRequestDTO.Code;
    product.Name = productRequestDTO.Name;
    product.Category = category;
    
      if (productRequestDTO.Tags != null)
    {
        product.Tags = new List<Tag>();
        foreach (var item in productRequestDTO.Tags)
        {
            product.Tags.Add(new Tag { Name = item });
        }
    }
    context.SaveChanges();
    return Results.Ok();
});

app.MapDelete("/products/{id}", ([FromRoute] int id, ApplicationDbContext context) =>
{
    var product = context.Products.Where(p => p.Id == id).First();
    context.Products.Remove(product);
    context.SaveChanges();
    return Results.Ok();
});

if (app.Environment.IsStaging())
{
    app.MapGet("/configuration/database", (IConfiguration configuration) =>
{
    return Results.Ok(configuration["database:connection"]);
});
}



app.Run();
