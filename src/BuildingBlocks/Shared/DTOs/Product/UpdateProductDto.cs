namespace Shared.DTOs.Product;

/// <summary>
/// DTO dùng cho việc cập nhật sản phẩm
/// </summary>
public class UpdateProductDto
{
    // Tên sản phẩm cần cập nhật
    public string Name { get; set; }

    // Tóm tắt mới của sản phẩm
    public string Summary { get; set; }

    // Mô tả mới của sản phẩm
    public string Description { get; set; }

    // Giá mới của sản phẩm
    public decimal Price { get; set; }
}