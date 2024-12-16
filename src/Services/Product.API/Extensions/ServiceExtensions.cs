using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.API.Persistence;

namespace Product.API.Extensions;

// Lớp static để mở rộng IServiceCollection 
public static class ServiceExtensions
{
    // Extension method đăng ký các service cần thiết
    // Thêm tham số IConfiguration để đọc cấu hình từ appsettings.json
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Đăng ký các controllers
        services.AddControllers();

        // Cấu hình để URL luôn ở dạng chữ thường
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        // Đăng ký service cho API Explorer (cần cho Swagger)
        services.AddEndpointsApiExplorer();

        // Đăng ký service sinh tài liệu Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Product API",
                Version = "v1",
                Description = "API để quản lý sản phẩm"
            });
        });

        // Cấu hình kết nối database
        services.ConfigureProductDbContext(configuration);

        return services;
    }

    // Extension method riêng để cấu hình DbContext
    private static IServiceCollection ConfigureProductDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Lấy chuỗi kết nối từ file cấu hình
        var connectionString = configuration.GetConnectionString("DefaultConnectionString");

        // Tạo builder để xử lý chuỗi kết nối MySQL
        var builder = new MySqlConnectionStringBuilder(connectionString);

        // Đăng ký DbContext với Entity Framework Core
        services.AddDbContext<ProductContext>(m => m.UseMySql(
            builder.ConnectionString,  // Chuỗi kết nối đã được xử lý
            ServerVersion.AutoDetect(builder.ConnectionString), // Tự động phát hiện phiên bản MySQL Server
            e =>
            {
                // Chỉ định assembly chứa các migrations
                e.MigrationsAssembly("Product.API");

                // Bỏ qua việc kiểm tra schema
                // Hữu ích khi database đã tồn tại và ta không muốn EF Core tạo/sửa schema
                e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            }));

        return services;
    }
}