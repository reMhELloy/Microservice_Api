// Import các namespace cần thiết
using Product.API.Entities;
// Sử dụng alias để tránh xung đột với ILogger của các thư viện khác
using ILogger = Serilog.ILogger;

namespace Product.API.Persistence;

/// <summary>
/// Lớp chịu trách nhiệm khởi tạo dữ liệu mẫu cho database
/// </summary>
public class ProductContextSeed
{
    /// <summary>
    /// Phương thức để seed dữ liệu sản phẩm vào database
    /// </summary>
    /// <param name="productContext">Database context</param>
    /// <param name="logger">Logger để ghi log</param>
    /// <returns>Task bất đồng bộ</returns>
    public static async Task SeedProductAsync(ProductContext productContext, ILogger logger)
    {
        try
        {
            // Kiểm tra xem đã có dữ liệu trong database chưa
            if (!productContext.Products.Any())
            {
                // Lấy danh sách sản phẩm mẫu
                var products = getCatalogProducts();

                // Validate từng sản phẩm trước khi thêm vào database
                foreach (var product in products)
                {
                    ValidateProduct(product);
                }

                // Thêm tất cả sản phẩm vào context
                productContext.AddRange(products);

                // Lưu các thay đổi vào database
                await productContext.SaveChangesAsync();

                // Log thông tin seed data thành công
                logger.Information("Seeded data for Product DB associated with context {DbContextName}",
                    nameof(ProductContext));
            }
            else
            {
                // Log thông báo nếu đã có dữ liệu
                logger.Information("Product data already exists in database");
            }
        }
        catch (Exception ex)
        {
            // Log lỗi nếu có vấn đề trong quá trình seed
            logger.Error(ex, "An error occurred while seeding the database with product data");
            // Throw lại exception để caller có thể xử lý
            throw;
        }
    }

    /// <summary>
    /// Phương thức private để validate dữ liệu sản phẩm
    /// </summary>
    /// <param name="product">Sản phẩm cần validate</param>
    /// <exception cref="ArgumentException">Thrown khi dữ liệu không hợp lệ</exception>
    private static void ValidateProduct(CatalogProduct product)
    {
        // Kiểm tra các trường bắt buộc
        if (string.IsNullOrEmpty(product.No))
            throw new ArgumentException("Product No is required");

        if (string.IsNullOrEmpty(product.Name))
            throw new ArgumentException("Product Name is required");

        if (string.IsNullOrEmpty(product.Description))
            throw new ArgumentException("Product Description is required");

        if (string.IsNullOrEmpty(product.Summary))
            throw new ArgumentException("Product Summary is required");

        // Kiểm tra giá trị hợp lệ
        if (product.Price <= 0)
            throw new ArgumentException("Product Price must be greater than 0");
    }

    /// <summary>
    /// Phương thức private để tạo danh sách sản phẩm mẫu
    /// </summary>
    /// <returns>Danh sách các sản phẩm mẫu</returns>
    private static IEnumerable<CatalogProduct> getCatalogProducts()
    {
        return new List<CatalogProduct>
        {
            new()
            {
                No = "PRD-001",
                Name = "Lotus Esprit Sports Car",
                Summary = "Luxury sports car with exceptional performance",
                Description = "The Lotus Esprit is a British high-performance sports car known for " +
                            "its sleek design and outstanding handling capabilities",
                Price = 177940.49m  // Sử dụng suffix 'm' cho decimal
            },
            new()
            {
                No = "PRD-002",
                Name = "Cadillac CTS Sedan",
                Summary = "Premium luxury sedan with advanced features",
                Description = "The Cadillac CTS is a luxury sedan that combines sophisticated " +
                            "styling with cutting-edge technology and premium comfort",
                Price = 114728.21m
            },
            new()
            {
                No = "PRD-003",
                Name = "Tesla Model S",
                Summary = "High-performance electric vehicle",
                Description = "The Tesla Model S is a premium electric sedan featuring " +
                            "advanced autopilot capabilities and long-range battery",
                Price = 89990.00m
            }
        };
    }
}