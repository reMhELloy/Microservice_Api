namespace Product.API.Extensions;

// Lớp static để mở rộng ConfigureHostBuilder
public static class ConfigureHostExtensions
{
   // Extension method thêm cấu hình cho ứng dụng
   public static void AddAppConfigurations(this ConfigureHostBuilder host)
   {
       host.ConfigureAppConfiguration((context, config) =>
       {
           // Lấy thông tin môi trường (Dev/Staging/Prod)
           var env = context.HostingEnvironment;

           // Thêm file appsettings.json (bắt buộc có)
           // reloadOnChange: true - tự động load lại khi file thay đổi
           config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               // Thêm file appsettings theo môi trường (không bắt buộc)
               // VD: appsettings.Development.json
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)

               // Thêm các biến môi trường
               .AddEnvironmentVariables();
       });
   }
}