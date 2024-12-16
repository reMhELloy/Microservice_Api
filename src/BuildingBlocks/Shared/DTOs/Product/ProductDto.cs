﻿namespace Shared.DTOs.Product;

/// <summary>
/// DTO dùng cho việc trả về thông tin sản phẩm
/// </summary>
public class ProductDto
{
    // ID của sản phẩm trong database
    public long Id { get; set; }

    // Mã sản phẩm
    public string No { get; set; }

    // Tên sản phẩm
    public string Name { get; set; }

    // Tóm tắt về sản phẩm
    public string Summary { get; set; }

    // Mô tả chi tiết sản phẩm 
    public string Description { get; set; }

    // Giá sản phẩm
    public decimal Price { get; set; }
}