namespace Product.API.Extensions;

// Lớp static để mở rộng IApplicationBuilder
public static class ApplicationExtensions
{
   // Extension method cho IApplicationBuilder, cấu hình middleware pipeline
   public static void UseInfrastructure(this IApplicationBuilder app)
   {
       // Kích hoạt Swagger endpoint để generate OpenAPI specification
       app.UseSwagger();

       // Kích hoạt Swagger UI để hiển thị documentation và test API
       app.UseSwaggerUI();

       // Thêm middleware routing để xử lý các request URL
       app.UseRouting();

       // Chuyển hướng HTTP sang HTTPS (đã comment, chỉ dùng cho production)
       // app.UseHttpsRedirection();

       // Thêm middleware xác thực/phân quyền
       app.UseAuthorization();

       // Cấu hình endpoint routing
       app.UseEndpoints(endpoints =>
       {
           // Map route mặc định: {controller}/{action}/{id?}
           endpoints.MapDefaultControllerRoute();
       });
   }
}