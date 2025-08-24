public class Product
{

    public Category Category { get; set; } 
  
    public int CategoryId { get; set; }

    public List<Tag> Tags { get; set; }

    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}
