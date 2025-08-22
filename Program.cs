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

//api.app.com/user/{code}
app.MapGet("/getproduct/{code}", ([FromRoute]string code) =>
{
    return code;
});

//parametro pelo body
app.MapPost("/saveproduct", (Product product) =>
{
    return product.Code + " - " + product.Name;
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
