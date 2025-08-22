//cria a aplicação web - hosting - escuta o que é que o user quer acessar
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//endpoint - rotas
//get post
app.MapGet("/", () => "Hello World!");

app.MapPost("/", () => new { Name = "Taís", Age = 40 });

app.MapGet("/AddHeader", (HttpResponse response) =>
{
    response.Headers.Add("Testando", "Taís");
    return new { Name = "Talita", Age = 40 };
});

//parametro por URL
//api.app.com/users?datastart={date}&dataend={date}
app.MapGet("/getproduct", ([FromQuery] string dateStart, [FromQuery] string dateEnd) =>
{
    return dateStart + " - " + dateEnd;
});


//parametro pelo body
app.MapPost("/saveproduct", (Product product) =>
{
    //CRUD - C
    ProductRepository.Add(product);
});

//api.app.com/user/{code}
app.MapGet("/getproduct/{code}", ([FromRoute]string code) =>
{
    //CRUD - R
    var product = ProductRepository.GetByCode(code);
    return product;
  
});

app.MapPut("/editproduct", (Product product) =>
{
    //CRUD - U
    var productSaved = ProductRepository.GetByCode(product.Code);
    productSaved.Name = product.Name;
});

app.MapDelete("/deleteproduct/{code}", ([FromRoute] string code) =>
{
    //CRUD - D
    var product = ProductRepository.GetByCode(code);
    ProductRepository.Remove(product);

});


//parametro pelo header
app.MapGet("/getproductbyheader", (HttpRequest request) =>
{
    return request.Headers["product-code"].ToString;
});


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