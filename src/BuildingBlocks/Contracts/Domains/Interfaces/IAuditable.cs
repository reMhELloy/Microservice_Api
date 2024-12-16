namespace Contracts.Domains.Interfaces;

// Interface marker cho các entity cần audit
// Kế thừa từ IDateTracking để có các property tracking thời gian
public interface IAuditable : IDateTracking
{
    // Interface này không thêm property mới
    // Chỉ kế thừa từ IDateTracking để có:
    // - DateTimeOffset CreatedDate { get; set; }
    // - DateTimeOffset? LastModifiedDate { get; set; }
}