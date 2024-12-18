// Import thư viện MVC từ ASP.NET Core
using Microsoft.AspNetCore.Mvc;

// Định nghĩa namespace cho controller
namespace Product.API.Controllers;

// Controller kế thừa từ ControllerBase (base class cho API controllers)
public class HomeController : ControllerBase
{
    // Action method xử lý request GET
    // IActionResult cho phép trả về nhiều loại HTTP response khác nhau
    public IActionResult Index()
    {
        // Chuyển hướng người dùng đến trang Swagger UI
        // "~/" đại diện cho root URL của ứng dụng
        return Redirect("~/swagger");
    }
}