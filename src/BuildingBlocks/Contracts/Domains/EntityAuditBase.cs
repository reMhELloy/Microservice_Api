namespace Contracts.Domains.Interfaces;

// Lớp abstract EntityAuditBase kế thừa từ EntityBase và implement IAuditable
// T: kiểu dữ liệu của khóa chính (có thể là int, long, Guid,...)
public abstract class EntityAuditBase<T> : EntityBase<T>, IAuditable
{
    // CreatedDate: Thời điểm entity được tạo
    // Implement từ interface IAuditable
    // Non-nullable vì mọi entity đều phải có thời điểm tạo
    public DateTimeOffset CreatedDate { get; set; }
    
    // LastModifiedDate: Thời điểm entity được cập nhật lần cuối
    // Implement từ interface IAuditable
    // Nullable (?) vì entity mới tạo chưa được cập nhật
    public DateTimeOffset? LastModifiedDate { get; set; }
}