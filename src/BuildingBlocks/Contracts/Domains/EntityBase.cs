using Contracts.Domains.Interfaces;  // Import interface IEntityBase

namespace Contracts.Domains;

// Lớp abstract base cho tất cả các entity
// TKey: kiểu dữ liệu của khóa chính (generic type parameter)
public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    // Property Id: khóa chính của entity
    // Kiểu dữ liệu được xác định bởi TKey
    // Implement từ interface IEntityBase<TKey>
    public TKey Id { get; set; }
}