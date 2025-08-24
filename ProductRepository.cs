//cria a aplicação web - hosting - escuta o que é que o user quer acessar
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