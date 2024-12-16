using Microsoft.EntityFrameworkCore;  // Để sử dụng DbContext
using Microsoft.Extensions.DependencyInjection;  // Cho IServiceProvider
using Microsoft.Extensions.Hosting;  // Cho IHost
using Microsoft.Extensions.Configuration;  // Cho IConfiguration
using Microsoft.Extensions.Logging;  // Cho ILogger

namespace Product.API.Extensions;

/// <summary>
/// Static class chứa các extension method cho IHost
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// Extension method để tự động migrate database và seed data
    /// </summary>
    /// <typeparam name="TContext">Kiểu của DbContext</typeparam>
    /// <param name="host">IHost instance</param>
    /// <param name="seeder">Action để seed data</param>
    /// <returns>IHost để có thể chain các method</returns>
    public static IHost MigrateDatabase<TContext>(this IHost host,
        Action<TContext, IServiceProvider> seeder) where TContext : DbContext
    {
        // Tạo scope để quản lý lifetime của các service
        using (var scope = host.Services.CreateScope())
        {
            // Lấy service provider từ scope
            var services = scope.ServiceProvider;

            // Lấy các service cần thiết từ DI container
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                // Log thông báo bắt đầu migrate
                logger.LogInformation("Migrating mysql database.");

                // Thực hiện migrate
                ExecuteMigrations(context);

                // Log thông báo migrate thành công
                logger.LogInformation("Migrated mysql database.");

                // Thực hiện seed data
                InvokeSeeder(seeder, context, services);
            }
            catch (Exception ex)
            {
                // Log lỗi nếu có vấn đề trong quá trình migrate hoặc seed
                logger.LogError(ex, "An error occurred while migrating the mysql database");
                throw; // Throw lại exception để caller có thể xử lý
            }
        }

        // Trả về host để có thể chain các method
        return host;
    }

    /// <summary>
    /// Private method thực hiện migrate database
    /// </summary>
    /// <typeparam name="TContext">Kiểu của DbContext</typeparam>
    /// <param name="context">Instance của DbContext</param>
    private static void ExecuteMigrations<TContext>(TContext context)
        where TContext : DbContext
    {
        // Gọi method Migrate() để apply các pending migrations
        context.Database.Migrate();
    }

    /// <summary>
    /// Private method thực hiện seed data
    /// </summary>
    /// <typeparam name="TContext">Kiểu của DbContext</typeparam>
    /// <param name="seeder">Action để seed data</param>
    /// <param name="context">Instance của DbContext</param>
    /// <param name="services">Service provider để inject các dependency</param>
    private static void InvokeSeeder<TContext>(
        Action<TContext, IServiceProvider> seeder,
        TContext context,
        IServiceProvider services)
        where TContext : DbContext
    {
        // Gọi action seeder với context và service provider
        seeder(context, services);
    }
}