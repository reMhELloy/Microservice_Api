using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Product;

/// <summary>
/// DTO class để tạo mới sản phẩm, kế thừa từ CreateOrUpdateProductDto
/// </summary>
/// <param name="No">Mã sản phẩm, bắt buộc và phải là duy nhất</param>
public class CreateProductDto : CreateOrUpdateProductDto
{
    // Mã sản phẩm - required, unique
    [Required]
    public string No { get; set; }
}