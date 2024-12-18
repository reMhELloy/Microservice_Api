namespace Shared.DTOs.Product;

/// <summary>
/// DTO class để cập nhật thông tin sản phẩm, kế thừa từ CreateOrUpdateProductDto
/// </summary>
/// <param name="Name">Tên sản phẩm cần cập nhật</param>
/// <param name="Summary">Tóm tắt sản phẩm cần cập nhật</param>  
/// <param name="Description">Mô tả chi tiết cần cập nhật</param>
/// <param name="Price">Giá sản phẩm cần cập nhật</param>
public class UpdateProductDto : CreateOrUpdateProductDto
{
}