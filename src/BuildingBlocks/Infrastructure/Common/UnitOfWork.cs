// Import các namespace cần thiết
using Contracts.Common.Interfaces;        // Cho interface IUnitOfWork
using Microsoft.EntityFrameworkCore;      // Cho DbContext

namespace Infrastructure.Common;

/// <summary>
/// Generic Unit of Work pattern implementation
/// TContext: Type của DbContext được sử dụng
/// </summary>
public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
    // Lưu trữ instance của DbContext
    private readonly TContext _context;

    /// <summary>
    /// Constructor nhận vào DbContext instance
    /// </summary>
    public UnitOfWork(TContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Giải phóng tài nguyên của DbContext
    /// Implement từ IDisposable interface
    /// </summary>
    public void Dispose() => _context.Dispose();

    /// <summary>
    /// Lưu tất cả thay đổi xuống database
    /// </summary>
    public Task<int> CommitAsync() => _context.SaveChangesAsync();
}