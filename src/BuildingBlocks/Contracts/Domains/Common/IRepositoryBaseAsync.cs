// Import các thư viện cần thiết
using System.Linq.Expressions;          // Cho Expression
using Contracts.Domains;                // Cho EntityBase
using Microsoft.EntityFrameworkCore;    // Cho DbContext
using Microsoft.EntityFrameworkCore.Storage;  // Cho IDbContextTransaction

namespace Contracts.Common.Interfaces;

/// <summary>
/// Interface định nghĩa các phương thức truy vấn cơ bản
/// </summary>
/// <typeparam name="T">Kiểu của entity, phải kế thừa từ EntityBase</typeparam>
/// <typeparam name="K">Kiểu của khóa chính</typeparam>
/// <typeparam name="TContext">Kiểu của DbContext</typeparam>
public interface IRepositoryQueryBase<T, K, TContext>
    where T : EntityBase<K>
    where TContext : DbContext
{
    /// <summary>
    /// Lấy tất cả entities
    /// </summary>
    /// <param name="trackChanges">Có track thay đổi hay không</param>
    IQueryable<T> FindAll(bool trackChanges = false);

    /// <summary>
    /// Lấy tất cả entities với các properties được include
    /// </summary>
    /// <param name="trackChanges">Có track thay đổi hay không</param>
    /// <param name="includeProperties">Các properties cần include</param>
    IQueryable<T> FindAll(bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties);

    /// <summary>
    /// Tìm entities theo điều kiện
    /// </summary>
    /// <param name="expression">Biểu thức điều kiện</param>
    /// <param name="trackChanges">Có track thay đổi hay không</param>
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges = false);

    /// <summary>
    /// Tìm entities theo điều kiện với các properties được include
    /// </summary>
    /// <param name="expression">Biểu thức điều kiện</param>
    /// <param name="trackChanges">Có track thay đổi hay không</param>
    /// <param name="includeProperties">Các properties cần include</param>
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges = false,
        params Expression<Func<T, object>>[] includeProperties);

    /// <summary>
    /// Lấy entity theo id
    /// </summary>
    /// <param name="id">Id của entity</param>
    Task<T?> GetByIdAsync(K id);

    /// <summary>
    /// Lấy entity theo id với các properties được include
    /// </summary>
    /// <param name="id">Id của entity</param>
    /// <param name="includeProperties">Các properties cần include</param>
    Task<T?> GetByIdAsync(K id,
        params Expression<Func<T, object>>[] includeProperties);
}

/// <summary>
/// Interface định nghĩa các phương thức CRUD và transaction
/// Kế thừa từ IRepositoryQueryBase để có các phương thức truy vấn
/// </summary>
public interface IRepositoryBaseAsync<T, K, TContext>
    : IRepositoryQueryBase<T, K, TContext>
    where T : EntityBase<K>
    where TContext : DbContext
{
    /// <summary>
    /// Tạo mới một entity
    /// </summary>
    /// <param name="entity">Entity cần tạo</param>
    /// <returns>Id của entity mới</returns>
    Task<K> CreateAsync(T entity);

    /// <summary>
    /// Tạo mới nhiều entities
    /// </summary>
    /// <param name="entities">Danh sách entities cần tạo</param>
    /// <returns>Danh sách id của các entities mới</returns>
    Task<IList<K>> CreateListAsync(IEnumerable<T> entities);

    /// <summary>
    /// Cập nhật một entity
    /// </summary>
    /// <param name="entity">Entity cần cập nhật</param>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Cập nhật nhiều entities
    /// </summary>
    /// <param name="entities">Danh sách entities cần cập nhật</param>
    Task UpdateListAsync(IEnumerable<T> entities);

    /// <summary>
    /// Xóa một entity
    /// </summary>
    /// <param name="entity">Entity cần xóa</param>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Xóa nhiều entities
    /// </summary>
    /// <param name="entities">Danh sách entities cần xóa</param>
    Task DeleteListAsync(IEnumerable<T> entities);

    /// <summary>
    /// Lưu các thay đổi vào database
    /// </summary>
    /// <returns>Số lượng records bị ảnh hưởng</returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Bắt đầu một transaction mới
    /// </summary>
    Task<IDbContextTransaction> BeginTransactionAsync();

    /// <summary>
    /// Kết thúc và commit transaction
    /// </summary>
    Task EndTransactionAsync();

    /// <summary>
    /// Rollback transaction
    /// </summary>
    Task RollbackTransactionAsync();
}