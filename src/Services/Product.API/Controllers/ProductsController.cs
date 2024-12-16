// Import các namespace cần thiết
using Microsoft.AspNetCore.Mvc;                    // Cho ApiController attribute và IActionResult
using Product.API.Repositories.Interfaces;         // Cho IProductRepository

namespace Product.API.Controllers;

/// <summary>
/// Controller xử lý các request liên quan đến Product
/// </summary>
[ApiController]                                    // Đánh dấu đây là API Controller
[Route("api/[controller]")]                        // Route pattern: /api/Products
public class ProductsController : ControllerBase
{
    // Inject Product Repository
    private readonly IProductRepository _repository;

    /// <summary>
    /// Constructor nhận dependency IProductRepository
    /// </summary>
    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// API endpoint GET /api/products
    /// Lấy danh sách tất cả sản phẩm
    /// </summary>
    [HttpGet]                                      // HTTP GET method
    public async Task<IActionResult> GetProducts()
    {
        // Gọi repository để lấy data
        var result = await _repository.GetProducts();
        // Trả về status 200 OK với data
        return Ok(result);
    }
}