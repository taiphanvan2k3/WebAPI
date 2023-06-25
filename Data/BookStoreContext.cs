using Microsoft.EntityFrameworkCore;

namespace LearnApiWeb.Data
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }

        #region DbSet
        public DbSet<Book> Books { get; set; } = null!;

        public DbSet<BookStore> BookStores { get; set; } = null!;

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<DonHang> DonHangs { get; set; } = null!;

        public DbSet<DonHangChiTiet> DonHangChiTiets { get; set; } = null!;

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

            // Đơn hàng
            modelBuilder.Entity<DonHang>(entity =>
            {
                entity.ToTable("DonHang");

                entity.HasKey(dh => dh.MaDonHang);

                entity.Property(dh => dh.NgayDatHang)
                      .HasDefaultValueSql("getdate()");

                entity.Property(dh => dh.TenNguoiNhan)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(dh => dh.DiaChiGiao)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(dh => dh.SoDienThoai)
                    .HasMaxLength(10)
                    .IsRequired();
            });

            modelBuilder.Entity<DonHangChiTiet>(entity =>
            {
                entity.HasKey(e => new { e.MaDonHang, e.MaHangHoa });

                entity.HasOne(dhct => dhct.DonHang)
                    .WithMany(dh => dh.DonHangChiTiets)
                    .HasForeignKey(dhct => dhct.MaDonHang)
                    .HasConstraintName("FK_DonHangCT_DonHang");

                entity.HasOne(dhct => dhct.Product)
                    .WithMany(p => p.DonHangChiTiets)
                    .HasForeignKey(dhct => dhct.MaHangHoa)
                    .HasConstraintName("FK_DonHangCT_SanPham");
            });

            modelBuilder.Entity<BookStore>(entity =>
            {
                entity.HasKey(b => b.MaCuaHang);

                entity.Property(b => b.TenCuaHang)
                      .HasMaxLength(50);
                      
                entity.HasMany(b => b.Books)
                      .WithOne(book => book.BookStore)
                      .HasForeignKey(book => book.MaCuaHang)
                      .HasConstraintName("FK_Book_BookStore");
            });
        }
    }
}