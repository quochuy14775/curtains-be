using curtains_be.Models;
using Microsoft.EntityFrameworkCore;

namespace curtains_be.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<ContactInfo> ContactInfos => Set<ContactInfo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Phone unique — tìm customer theo SĐT khi điền form lần 2
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Phone)
            .IsUnique();

        // Soft delete global filter
        modelBuilder.Entity<Category>()
            .HasQueryFilter(c => !c.IsDeleted);

        modelBuilder.Entity<Product>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<Customer>()
            .HasQueryFilter(c => !c.IsDeleted);

        modelBuilder.Entity<Appointment>()
            .HasQueryFilter(a => !a.IsDeleted);

        // Store enum as string cho dễ đọc trong DB
        modelBuilder.Entity<Appointment>()
            .Property(a => a.Status)
            .HasConversion<string>();

        // Seed bản ghi thông tin liên hệ mặc định (Id = 1)
        modelBuilder.Entity<ContactInfo>().HasData(new ContactInfo
        {
            Id = 1,
            CompanyName = "Maison Drapé",
            Phone = "0901 234 567",
            Email = "hello@maisondrage.vn",
            AddressLine1 = "123 Đường Nội Thất, Q.1",
            AddressLine2 = "TP. Hồ Chí Minh",
            ZaloUrl = "https://zalo.me/0901234567",
            WhatsappUrl = "https://wa.me/84901234567",
            FacebookUrl = null
        });
    }
}
