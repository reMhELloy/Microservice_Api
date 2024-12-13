// Import logging modules
using Common.Logging;
using Serilog;

// Khởi tạo web app và cấu hình Serilog
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

// Ghi log bắt đầu khởi động 
Log.Information("Start Customer API up");

try
{
   // Đăng ký các service cơ bản
   builder.Services.AddControllers();             // Xử lý Controller
   builder.Services.AddEndpointsApiExplorer();    // API explorer cho Swagger
   builder.Services.AddSwaggerGen();              // Sinh tài liệu API

   var app = builder.Build();

   // Cấu hình middleware pipeline
   if (app.Environment.IsDevelopment())
   {
       app.UseSwagger();      // Endpoint Swagger 
       app.UseSwaggerUI();    // UI Swagger
   }

   app.UseHttpsRedirection(); // HTTPS redirect
   app.UseAuthorization();    // Authorization 
   app.MapControllers();      // Route đến controllers

   app.Run();
}
catch (Exception ex)
{
   // Log lỗi nghiêm trọng
   Log.Fatal(ex, "Unhandled exception");
}
finally
{
   // Log kết thúc và flush buffer
   Log.Information("Shut down Customer API complete");
   Log.CloseAndFlush();
}