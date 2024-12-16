// Import các namespace cần thiết
using Contracts.Domains.Interfaces;    // Chứa interface IDateTracking
using Microsoft.EntityFrameworkCore;   // Namespace chính của EF Core
using Product.API.Entities;           // Chứa entity CatalogProduct

namespace Product.API.Persistence;

// ProductContext kế thừa từ DbContext - lớp chính để tương tác với database trong EF Core
public class ProductContext : DbContext
{
    // Constructor nhận vào options để cấu hình context (ví dụ: connection string)
    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {
    }
    
    // Định nghĩa DbSet để thao tác với bảng Products trong database
    // CatalogProduct là entity class map với bảng trong database
    public DbSet<CatalogProduct> Products { get; set; }

    // Override phương thức SaveChangesAsync để thêm custom logic trước khi lưu vào DB
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        // Lấy tất cả các entity đang được theo dõi có trạng thái là Modified, Added hoặc Deleted
        var modified = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified ||
                       e.State == EntityState.Added ||
                       e.State == EntityState.Deleted);

        // Duyệt qua từng entity để xử lý
        foreach (var item in modified)
        {
            switch (item.State)
            {
                case EntityState.Added:
                    // Nếu entity implement interface IDateTracking
                    if (item.Entity is IDateTracking addedEntity)
                    {
                        // Tự động set CreatedDate là thời điểm hiện tại (UTC)
                        addedEntity.CreatedDate = DateTime.UtcNow;
                        item.State = EntityState.Added;
                    }
                    break;
            
                case EntityState.Modified:
                    // Đảm bảo trường Id không bị modify
                    Entry(item.Entity).Property("Id").IsModified = false;
                    // Nếu entity implement interface IDateTracking
                    if (item.Entity is IDateTracking modifiedEntity)
                    {
                        // Tự động cập nhật LastModifiedDate
                        modifiedEntity.LastModifiedDate = DateTime.UtcNow;
                        item.State = EntityState.Modified;
                    }
                    break; 
            }
        }
        // Gọi phương thức SaveChangesAsync của lớp base để lưu thay đổi vào DB
        return base.SaveChangesAsync(cancellationToken);
    }
}