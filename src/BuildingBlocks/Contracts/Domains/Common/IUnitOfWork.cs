using Microsoft.EntityFrameworkCore;      // Để sử dụng DbContext

namespace Contracts.Common.Interfaces;

/// <summary>
/// Interface định nghĩa Unit of Work pattern
/// </summary>
/// <typeparam name="TContext">Kiểu của DbContext</typeparam>
public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
{
    /// <summary>
    /// Commit tất cả các thay đổi vào database
    /// </summary>
    /// <returns>Số lượng records bị ảnh hưởng</returns>
    Task<int> CommitAsync();
}