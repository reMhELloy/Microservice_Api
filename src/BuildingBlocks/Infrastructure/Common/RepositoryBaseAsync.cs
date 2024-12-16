// Import các thư viện cần thiết
using System.Linq.Expressions;                // Cho Expression
using Contracts.Common.Interfaces;            // Cho IUnitOfWork
using Contracts.Domains;                      // Cho EntityBase
using Microsoft.EntityFrameworkCore;          // Cho DbContext
using Microsoft.EntityFrameworkCore.Storage;  // Cho IDbContextTransaction

namespace Infrastructure.Common;

/// <summary>
/// Generic Repository base class hỗ trợ CRUD và transaction
/// T: Entity type
/// K: Kiểu dữ liệu của ID
/// TContext: DbContext type
/// </summary>
public class RepositoryBaseAsyncAsync<T, K, TContext> : IRepositoryBaseAsync<T, K, TContext>
    where T : EntityBase<K>      // T phải kế thừa từ EntityBase
    where TContext : DbContext   // TContext phải là DbContext
{
    // Inject DbContext và UnitOfWork
    private readonly TContext _dbContext;
    private readonly IUnitOfWork<TContext> _unitOfWork;

    /// <summary>
    /// Constructor với null check cho dependencies
    /// </summary>
    public RepositoryBaseAsyncAsync(TContext dbContext, IUnitOfWork<TContext> unitOfWork)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <summary>
    /// Lấy tất cả entities với tùy chọn tracking changes
    /// </summary>
    public IQueryable<T> FindAll(bool trackChanges = false) =>
        !trackChanges ? _dbContext.Set<T>().AsNoTracking() : // Không track thay đổi
            _dbContext.Set<T>();                             // Track thay đổi

    /// <summary>
    /// Lấy tất cả entities với eager loading properties
    /// </summary>
    public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindAll(trackChanges);
        // Thêm các Include để eager loading
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    /// <summary>
    /// Tìm kiếm theo điều kiện với tùy chọn tracking
    /// </summary> 
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) =>
        !trackChanges
            ? _dbContext.Set<T>().Where(expression).AsNoTracking()
            : _dbContext.Set<T>().Where(expression);

    /// <summary>
    /// Tìm kiếm theo điều kiện với eager loading
    /// </summary>
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
        return items;
    }

    /// <summary>
    /// Lấy entity theo ID
    /// </summary>
    public async Task<T?> GetByIdAsync(K id) =>
        await FindByCondition(x => x.Id.Equals(id))
        .FirstOrDefaultAsync();

    /// <summary>
    /// Lấy entity theo ID với eager loading
    /// </summary>
    public async Task<T?> GetByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties) =>
        await FindByCondition(x => x.Id.Equals(id), trackChanges: false, includeProperties)
            .FirstOrDefaultAsync();

    // Các phương thức quản lý transaction
    public Task<IDbContextTransaction> BeginTransactionAsync() => _dbContext.Database.BeginTransactionAsync();
    public async Task EndTransactionAsync()
    {
        await SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
    }
    public Task RollbackTransactionAsync() => _dbContext.Database.RollbackTransactionAsync();

    /// <summary>
    /// Tạo mới một entity
    /// </summary>
    public async Task<K> CreateAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity.Id;
    }

    /// <summary>
    /// Tạo mới nhiều entities
    /// </summary>
    public async Task<IList<K>> CreateListAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
        return entities.Select(x => x.Id).ToList();
    }

    /// <summary>
    /// Cập nhật entity
    /// </summary>
    public Task UpdateAsync(T entity)
    {
        // Kiểm tra nếu entity không có thay đổi
        if (_dbContext.Entry(entity).State == EntityState.Unchanged)
            return Task.CompletedTask;

        // Cập nhật giá trị mới
        T exist = _dbContext.Set<T>().Find(entity.Id);
        _dbContext.Entry(exist).CurrentValues.SetValues(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Cập nhật nhiều entities
    /// </summary>
    public Task UpdateListAsync(IEnumerable<T> entities) =>
        _dbContext.Set<T>().AddRangeAsync(entities);

    /// <summary>
    /// Xóa một entity
    /// </summary>
    public Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Xóa nhiều entities
    /// </summary>
    public Task DeleteListAsync(IEnumerable<T> entities)
    {
        _dbContext.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Lưu thay đổi xuống database
    /// </summary>
    public Task<int> SaveChangesAsync() => _unitOfWork.CommitAsync();
}