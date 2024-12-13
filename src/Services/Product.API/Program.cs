// Import các namespace cần thiết
using Common.Logging;
using Product.API.Extensions;
using Serilog;

// Khởi tạo builder cho web application
var builder = WebApplication.CreateBuilder(args);

// Log thông tin bắt đầu khởi động API
Log.Information("Start Product API up");

try
{
   // Cấu hình logging sử dụng Serilog
   builder.Host.UseSerilog(Serilogger.Configure);

   // Thêm cấu hình từ appsettings.json và biến môi trường
   builder.Host.AddAppConfigurations();

   // Đăng ký các service cần thiết (Controllers, Swagger, etc)
   builder.Services.AddInfrastructure();
  
   // Build ứng dụng
   var app = builder.Build();

   // Cấu hình middleware pipeline 
   app.UseInfrastructure();

   // Chạy ứng dụng
   app.Run();
}
catch (Exception ex)
{
   // Log lỗi nghiêm trọng nếu có
   Log.Fatal(ex, "Unhandled exception");
}
finally
{
   // Log kết thúc và đảm bảo flush buffer
   Log.Information("Shut down Product API complete");
   Log.CloseAndFlush();
}