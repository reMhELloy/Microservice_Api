// Import các namespace cần thiết
using Contracts.Common.Interfaces;        // Cho IRepositoryBaseAsync
using Product.API.Entities;              // Cho CatalogProduct
using Product.API.Persistence;           // Cho ProductContext

namespace Product.API.Repositories.Interfaces;

/// <summary>
/// Interface định nghĩa các thao tác cụ thể cho Product repository
/// Kế thừa từ IRepositoryBaseAsync để có các phương thức CRUD cơ bản
/// </summary>
public interface IProductRepository : IRepositoryBaseAsync<CatalogProduct, long, ProductContext>
{
    /// <summary>
    /// Lấy danh sách tất cả sản phẩm
    /// </summary>
    /// <returns>Danh sách các sản phẩm</returns>
    Task<IEnumerable<CatalogProduct>> GetProducts();

    /// <summary>
    /// Lấy thông tin một sản phẩm theo id
    /// </summary>
    /// <param name="id">Id của sản phẩm</param>
    /// <returns>Thông tin sản phẩm</returns>
    Task<CatalogProduct> GetProduct(long id);

    /// <summary>
    /// Lấy thông tin sản phẩm theo mã sản phẩm
    /// </summary>
    /// <param name="productNo">Mã sản phẩm</param>
    /// <returns>Thông tin sản phẩm</returns> 
    Task<CatalogProduct> GetProductByNo(string productNo);

    /// <summary>
    /// Tạo mới một sản phẩm
    /// </summary>
    /// <param name="product">Thông tin sản phẩm cần tạo</param>
    Task CreateProduct(CatalogProduct product);

    /// <summary>
    /// Cập nhật thông tin sản phẩm
    /// </summary>
    /// <param name="product">Thông tin sản phẩm cần cập nhật</param>
    Task UpdateProduct(CatalogProduct product);

    /// <summary>
    /// Xóa một sản phẩm
    /// </summary>
    /// <param name="id">Id của sản phẩm cần xóa</param>
    Task DeleteProduct(long id);
}