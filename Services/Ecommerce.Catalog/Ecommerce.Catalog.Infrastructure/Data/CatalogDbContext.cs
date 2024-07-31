using Ecommerce.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Catalog.Infrastructure.Data;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Config Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Slug).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Discount).HasDefaultValue(0).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Stock).IsRequired();

            // Relation many-many with Category
            entity
                .HasMany(e => e.Categories)
                .WithMany(c => c.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductCategory",
                    j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                    j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId"));

            // Index
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Config Product Category entity 
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });

        // Dates configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("GETDATE()")
                  .ValueGeneratedOnAdd();
            entity.Property(e => e.UpdatedAt)
                  .HasDefaultValueSql("GETDATE()")
                  .ValueGeneratedOnAddOrUpdate();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("GETDATE()")
                  .ValueGeneratedOnAdd();
            entity.Property(e => e.UpdatedAt)
                  .HasDefaultValueSql("GETDATE()")
                  .ValueGeneratedOnAddOrUpdate();
        });

    }
}
