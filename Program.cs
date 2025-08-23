//cria a aplicação web - hosting - escuta o que é que o user quer acessar
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var configuration = app.Configuration;
ProductRepository.Init(configuration);


app.MapPost("/products", (Product product) =>
{
    ProductRepository.Add(product);
    return Results.Created("/products", product.Code);
});


app.MapGet("/products/{code}", ([FromRoute] string code) =>
{
    var product = ProductRepository.GetByCode(code);
    if (product != null)
    {
        return Results.Ok(product);
    }
    return Results.NotFound();
});

app.MapPut("/products", (Product product) =>
{
    var productSaved = ProductRepository.GetByCode(product.Code);
    productSaved.Name = product.Name;
    return Results.Ok();
});

app.MapDelete("/products/{code}", ([FromRoute] string code) =>
{
    var product = ProductRepository.GetByCode(code);
    ProductRepository.Remove(product);
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

public class Product
{
    public string Code { get; set; }
    public string Name { get; set; }
}

//iniciando CRUD
public static class ProductRepository
{
    public static List<Product> Products { get; set; }

    public static void Init(IConfiguration configuration)
    {
        var product = configuration.GetSection("Products").Get<List<Product>>();
        Products = product;
    }

    public static void Add(Product product)
    {
        if (Products == null)
        {
            Products = new List<Product>();

        }
        Products.Add(product);
    }


    public static Product GetByCode(string code)
    {
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static void Remove(Product product)
    {
        Products.Remove(product);
    }
}