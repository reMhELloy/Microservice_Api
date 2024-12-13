namespace Product.API.Extensions;

// Lớp static để mở rộng IServiceCollection 
public static class ServiceExtensions
{
   // Extension method đăng ký các service cần thiết
   public static IServiceCollection AddInfrastructure(this IServiceCollection services)
   {
       // Đăng ký các controllers
       services.AddControllers();

       // Cấu hình để URL luôn ở dạng chữ thường
       services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

       // Đăng ký service cho API Explorer (cần cho Swagger)
       services.AddEndpointsApiExplorer();

       // Đăng ký service sinh tài liệu Swagger
       services.AddSwaggerGen();

       // Trả về services để có thể chain các method
       return services;
   }
}