// Import các namespace cần thiết
using AutoMapper;
using Microsoft.AspNetCore.Mvc;                    // Cho ApiController attribute và IActionResult
using Product.API.Entities;
using Product.API.Repositories.Interfaces;
using Shared.DTOs.Product;
using System.ComponentModel.DataAnnotations;         // Cho IProductRepository

namespace Product.API.Controllers;

/// <summary>
/// API Controller quản lý các thao tác CRUD và các chức năng bổ sung cho Product
/// Cung cấp các endpoints để thao tác với dữ liệu sản phẩm trong hệ thống
/// </summary>
[ApiController]                                    // Đánh dấu đây là API Controller
[Route("api/[controller]")]                        // Route pattern: /api/Products
public class ProductsController : ControllerBase
{
    // Repository để thao tác với database
    private readonly IProductRepository _repository;
    // AutoMapper để chuyển đổi giữa các object types
    private readonly IMapper _mapper;

    /// <summary>
    /// Khởi tạo controller với các dependencies cần thiết
    /// </summary>
    /// <param name="repository">Repository để thao tác với database</param>
    /// <param name="mapper">AutoMapper để mapping giữa các object types</param>
    /// <exception cref="ArgumentNullException">Thrown khi repository hoặc mapper là null</exception>
    public ProductsController(IProductRepository repository, IMapper mapper)
    {
        // Kiểm tra null để đảm bảo các dependencies bắt buộc được inject
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    #region CRUD Operations - Các thao tác CRUD cơ bản

    /// <summary>
    /// Lấy danh sách tất cả sản phẩm trong hệ thống
    /// GET: api/products
    /// </summary>
    /// <returns>
    /// 200 OK với danh sách ProductDto
    /// Mỗi ProductDto chứa thông tin cơ bản của sản phẩm
    /// </returns>
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        // Lấy danh sách sản phẩm từ database
        var products = await _repository.GetProducts();
        // Convert từ domain models sang DTOs
        var result = _mapper.Map<IEnumerable<ProductDto>>(products);
        return Ok(result);
    }

    /// <summary>
    /// Lấy thông tin chi tiết của một sản phẩm theo ID
    /// GET: api/products/{id}
    /// </summary>
    /// <param name="id">ID của sản phẩm cần truy vấn</param>
    /// <returns>
    /// 200 OK với thông tin sản phẩm dạng ProductDto
    /// 404 Not Found nếu không tìm thấy sản phẩm
    /// </returns>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetProduct([Required] long id)
    {
        // Tìm sản phẩm trong database
        var product = await _repository.GetProduct(id);
        if (product == null)
            return NotFound();  // Trả về 404 nếu không tìm thấy

        // Convert sang DTO và trả về
        var result = _mapper.Map<ProductDto>(product);
        return Ok(result);
    }

    /// <summary>
    /// Tạo mới một sản phẩm trong hệ thống
    /// POST: api/products
    /// </summary>
    /// <param name="productDto">Thông tin sản phẩm cần tạo</param>
    /// <returns>
    /// 201 Created với thông tin sản phẩm vừa tạo
    /// 400 Bad Request nếu dữ liệu không hợp lệ
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
    {
        // Kiểm tra tính hợp lệ của dữ liệu đầu vào
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Convert từ DTO sang domain model
        var product = _mapper.Map<CatalogProduct>(productDto);

        // Lưu vào database
        await _repository.CreateProduct(product);
        await _repository.SaveChangesAsync();

        // Convert kết quả sang DTO
        var result = _mapper.Map<ProductDto>(product);

        // Trả về 201 Created với URI của resource mới
        return CreatedAtAction(
            nameof(GetProduct),  // Action để lấy resource
            new { id = product.Id },  // Route params
            result  // Resource data
        );
    }

    /// <summary>
    /// Cập nhật thông tin của một sản phẩm
    /// PUT: api/products/{id}
    /// </summary>
    /// <param name="id">ID của sản phẩm cần cập nhật</param>
    /// <param name="productDto">Thông tin cập nhật</param>
    /// <returns>
    /// 200 OK với thông tin sản phẩm sau khi cập nhật
    /// 404 Not Found nếu không tìm thấy sản phẩm
    /// 400 Bad Request nếu dữ liệu không hợp lệ
    /// </returns>
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateProduct([Required] long id, [FromBody] UpdateProductDto productDto)
    {
        // Kiểm tra tính hợp lệ của dữ liệu
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Tìm sản phẩm cần cập nhật
        var product = await _repository.GetProduct(id);
        if (product == null)
            return NotFound();

        // Cập nhật thông tin sản phẩm
        var updateProduct = _mapper.Map(productDto, product);
        await _repository.UpdateProduct(updateProduct);
        await _repository.SaveChangesAsync();

        // Convert và trả về kết quả
        var result = _mapper.Map<ProductDto>(product);
        return Ok(result);
    }

    /// <summary>
    /// Xóa một sản phẩm khỏi hệ thống
    /// DELETE: api/products/{id}
    /// </summary>
    /// <param name="id">ID của sản phẩm cần xóa</param>
    /// <returns>
    /// 204 No Content nếu xóa thành công
    /// 404 Not Found nếu không tìm thấy sản phẩm
    /// </returns>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteProduct([Required] long id)
    {
        // Kiểm tra sự tồn tại của sản phẩm
        var product = await _repository.GetProduct(id);
        if (product == null)
            return NotFound();

        // Thực hiện xóa
        await _repository.DeleteProduct(id);
        await _repository.SaveChangesAsync();

        // Trả về 204 No Content
        return NoContent();
    }

    #endregion

    #region Additional Resources - Các chức năng bổ sung

    /// <summary>
    /// Tìm kiếm sản phẩm theo mã sản phẩm
    /// GET: api/products/get-product-by-no/{productNo}
    /// </summary>
    /// <param name="productNo">Mã sản phẩm cần tìm</param>
    /// <returns>
    /// 200 OK với thông tin sản phẩm
    /// 404 Not Found nếu không tìm thấy sản phẩm
    /// </returns>
    [HttpGet("get-product-by-no/{productNo}")]
    public async Task<IActionResult> GetProductByNo([Required] string productNo)
    {
        // Tìm sản phẩm theo mã
        var product = await _repository.GetProductByNo(productNo);
        if (product == null)
            return NotFound();

        // Convert và trả về kết quả
        var result = _mapper.Map<ProductDto>(product);
        return Ok(result);
    }

    #endregion

}