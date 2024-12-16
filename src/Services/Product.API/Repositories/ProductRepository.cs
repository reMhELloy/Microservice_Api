// Import các namespace cần thiết
using Contracts.Common.Interfaces;        // Cho IUnitOfWork
using Infrastructure.Common;             // Cho RepositoryBaseAsync
using Microsoft.EntityFrameworkCore;     // Cho ToListAsync, SingleOrDefaultAsync
using Product.API.Entities;             // Cho CatalogProduct
using Product.API.Persistence;          // Cho ProductContext  
using Product.API.Repositories.Interfaces; // Cho IProductRepository

namespace Product.API.Repositories;

/// <summary>
/// Implement cụ thể của Product Repository
/// </summary>
public class ProductRepository : RepositoryBaseAsyncAsync<CatalogProduct, long, ProductContext>, IProductRepository
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="dbContext">Instance của ProductContext</param>
    /// <param name="unitOfWork">Instance của UnitOfWork</param>
    public ProductRepository(ProductContext dbContext, IUnitOfWork<ProductContext> unitOfWork)
        : base(dbContext, unitOfWork)
    {
    }

    /// <summary>
    /// Lấy danh sách tất cả sản phẩm
    /// </summary>
    public async Task<IEnumerable<CatalogProduct>> GetProducts() =>
        await FindAll().ToListAsync();

    /// <summary>
    /// Lấy sản phẩm theo id
    /// </summary>
    public Task<CatalogProduct> GetProduct(long id) =>
        GetByIdAsync(id);

    /// <summary>
    /// Lấy sản phẩm theo mã sản phẩm
    /// </summary>
    public Task<CatalogProduct> GetProductByNo(string productNo) =>
        FindByCondition(x => x.No.Equals(productNo)).SingleOrDefaultAsync();

    /// <summary>
    /// Tạo mới sản phẩm
    /// </summary>
    public Task CreateProduct(CatalogProduct product) =>
        CreateAsync(product);

    /// <summary>
    /// Cập nhật sản phẩm
    /// </summary>
    public Task UpdateProduct(CatalogProduct product) =>
        UpdateAsync(product);

    /// <summary>
    /// Xóa sản phẩm theo id
    /// </summary>
    public async Task DeleteProduct(long id)
    {
        var product = await GetProduct(id);
        if (product != null) DeleteAsync(product);
    }
}