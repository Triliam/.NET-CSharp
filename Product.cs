//cria a aplicação web - hosting - escuta o que é que o user quer acessar
public class Product
{

    public Category category { get; set; }

    public int CategoryId { get; set; }

    public List<Tag> Tags { get; set; }

    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}
