using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Product;

/// <summary>
/// Base class chứa các thuộc tính chung cho việc tạo mới hoặc cập nhật Product
/// </summary>
/// <param name="Name">Tên sản phẩm, bắt buộc và tối đa 250 ký tự</param> 
/// <param name="Summary">Tóm tắt sản phẩm, tối đa 255 ký tự</param>
/// <param name="Description">Mô tả chi tiết sản phẩm</param>
/// <param name="Price">Giá sản phẩm kiểu decimal</param>
public abstract class CreateOrUpdateProductDto
{
    // Tên sản phẩm - required, max 250 ký tự
    [Required]
    [MaxLength(250, ErrorMessage = "Maximum length for Product Name is 250 characters.")]
    public string Name { get; set; }

    // Tóm tắt sản phẩm - optional, max 255 ký tự 
    [MaxLength(255, ErrorMessage = "Maximum length for Product Summary is 255 characters.")]
    public string Summary { get; set; }

    // Mô tả chi tiết sản phẩm - optional
    public string Description { get; set; }

    // Giá sản phẩm kiểu decimal
    public decimal Price { get; set; }
}