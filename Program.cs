//cria a aplicação web - hosting - escuta o que é que o user quer acessar
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>();

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
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}

//classe que funciona como serviço e configura para classes que são tabelas no banco de dados
public class ApplicationDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(120).IsRequired();

        modelBuilder.Entity<Product>().Property(p => p.Code).HasMaxLength(20).IsRequired();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("Server=localhost;Database=Products;User Id=sa;Password=@Sql2019;MultipleActiveResultSets=true;Encrypt=YES;TrustServerCertificate=YES");

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