
using Microsoft.EntityFrameworkCore;
//classe que funciona como serviço e configura para classes que são tabelas no banco de dados
public class ApplicationDbContext : DbContext
{


    public DbSet<Product> Products { get; set; }
    
    public DbSet<Category> Categories { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(120).IsRequired();

        modelBuilder.Entity<Product>().Property(p => p.Code).HasMaxLength(20).IsRequired();
        modelBuilder.Entity<Category>().ToTable("Categories");
    }



}
