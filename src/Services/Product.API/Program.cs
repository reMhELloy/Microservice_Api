// Import các namespace cần thiết
using Common.Logging;           // Cho Serilogger
using Product.API.Extensions;   // Cho các extension method
using Product.API.Persistence;  // Cho ProductContext
using Serilog;                 // Cho logging

// Khởi tạo builder cho web application
var builder = WebApplication.CreateBuilder(args);

// Log thông tin bắt đầu khởi động API
Log.Information("Start Product API up");

try
{
    // Cấu hình Serilog làm logging provider
    builder.Host.UseSerilog(Serilogger.Configure);

    // Thêm cấu hình từ appsettings.json và biến môi trường
    builder.Host.AddAppConfigurations();

    // Đăng ký các service cần thiết (Controllers, Swagger, DbContext, etc)
    builder.Services.AddInfrastructure(builder.Configuration);

    // Build ứng dụng
    var app = builder.Build();

    // Cấu hình middleware pipeline
    app.UseInfrastructure();

    // Thực hiện migration database và seed data
    app.MigrateDatabase<ProductContext>((context, _) =>
    {
        // Sử dụng Log.Logger trực tiếp từ Serilog thay vì injection
        ProductContextSeed.SeedProductAsync(context, Log.Logger).Wait();
    })
    .Run(); // Chain với Run() để khởi động ứng dụng
}
catch (Exception ex)
{
    // Lấy tên loại exception
    string type = ex.GetType().Name;

    // Nếu là StopTheHostException thì throw lại
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    // Log lỗi fatal nếu là exception khác
    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    // Đảm bảo log được thông tin shutdown và flush buffer
    Log.Information("Shut down Product API complete");
    Log.CloseAndFlush();
}