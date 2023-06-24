using Microsoft.EntityFrameworkCore;

namespace LearnApiWeb.Data
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }

        #region DbSet
        public DbSet<Book> Books { get; set; } = null!;

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product
            modelBuilder.Entity<Product>()
                        .Property(p => p.Name)
                        .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                        .Property(p => p.Description)
                        .HasMaxLength(100);

            // Category
            modelBuilder.Entity<Category>()
                        .Property(c => c.Name)
                        .HasMaxLength(50);

            modelBuilder.Entity<Category>()
                        .HasMany(c => c.Products)
                        .WithOne(p => p.Category)
                        .HasForeignKey(p => p.CategoryId);
        }
    }
}