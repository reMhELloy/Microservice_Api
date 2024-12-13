using Microsoft.Extensions.Hosting;
using Serilog;

namespace Common.Logging;

// Định nghĩa một lớp tĩnh để cấu hình Serilog 
public static class Serilogger
{
        // Định nghĩa một Action delegate nhận vào HostBuilderContext và LoggerConfiguration
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
        (context, configuration) =>
        {
            // Lấy tên ứng dụng từ context và chuyển đổi thành chữ thường, thay thế dấu '.' bằng '-'
            var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
            // Lấy tên môi trường, nếu null thì mặc định là "Development" 
            var environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";

            configuration
                // Ghi log ra Debug window
                .WriteTo.Debug()
                // Ghi log ra Console với định dạng được chỉ định
                // Format: [Thời gian Level] Context
                //         Message
                //         Exception (nếu có)
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                // Thêm thông tin từ LogContext vào log
                .Enrich.FromLogContext()
                // Thêm tên máy tính vào log
                .Enrich.WithMachineName()
                // Thêm thông tin môi trường vào log
                .Enrich.WithProperty("Environment", environmentName)
                // Thêm tên ứng dụng vào log
                .Enrich.WithProperty("Application", applicationName)
                // Đọc cấu hình bổ sung từ file cấu hình của ứng dụng
                .ReadFrom.Configuration(context.Configuration);
        };
}