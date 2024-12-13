using Common.Logging;
using Serilog;

// Tạo builder cho ứng dụng web
var builder = WebApplication.CreateBuilder(args);
// Cấu hình Serilog làm logger cho ứng dụng, sử dụng cấu hình từ class Serilogger
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information("Start Hangfire API up");

try
{
    // Đăng ký các service cần thiết
    builder.Services.AddControllers();           // Thêm MVC controllers
    builder.Services.AddEndpointsApiExplorer();  // Thêm API explorer
    builder.Services.AddSwaggerGen();            // Cấu hình Swagger để tạo API documentation
    // Build ứng dụng
    var app = builder.Build();

    // Cấu hình middleware pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();      // Kích hoạt Swagger endpoint
        app.UseSwaggerUI();    // Kích hoạt Swagger UI trong môi trường Development
    }

    app.UseHttpsRedirection(); // Chuyển hướng HTTP sang HTTPS
    app.UseAuthorization();    // Kích hoạt xác thực
    app.MapControllers();      // Map các route cho controllers

    app.Run();
}
catch (Exception ex)
{
    // Ghi log nếu có lỗi không xử lý được
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    // Ghi log khi ứng dụng kết thúc
    Log.Information("Shut down Hangfire API complete");
    // Đảm bảo tất cả log được ghi xong và giải phóng tài nguyên
    Log.CloseAndFlush();
}