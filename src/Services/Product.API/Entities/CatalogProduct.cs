using System.ComponentModel.DataAnnotations;      // Chứa các attribute để validate
using System.ComponentModel.DataAnnotations.Schema;  // Chứa các attribute để map với database
using Contracts.Domains.Interfaces;               // Interface cho entity base

namespace Product.API.Entities;

// CatalogProduct kế thừa từ EntityAuditBase với khóa chính kiểu long
public class CatalogProduct : EntityAuditBase<long>
{
    // Mã sản phẩm
    [Required]                          // Bắt buộc phải có giá trị (not null)
    [Column(TypeName = "varchar(150)")] // Map với cột trong DB có kiểu varchar(150)
    public string No { get; set; }

    // Tên sản phẩm
    [Required]                          // Bắt buộc phải có giá trị
    [Column(TypeName = "nvarchar(250)")] // Kiểu nvarchar(250) để lưu được tiếng Việt
    public string Name { get; set; }

    // Tóm tắt về sản phẩm
    [Column(TypeName = "nvarchar(255)")] // Kiểu nvarchar(255) cho phép lưu Unicode
    public string Summary { get; set; }

    // Mô tả chi tiết sản phẩm
    [Column(TypeName = "text")]          // Kiểu text để lưu nội dung dài
    public string Description { get; set; }

    // Giá sản phẩm
    [Column(TypeName = "decimal(12,2)")] // Decimal với 12 chữ số, 2 số sau dấu phẩy
    public decimal Price { get; set; }
}