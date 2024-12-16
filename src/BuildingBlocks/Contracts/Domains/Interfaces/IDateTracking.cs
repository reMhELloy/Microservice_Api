namespace Contracts.Domains.Interfaces;

// Interface chung định nghĩa các property để theo dõi thời gian tạo và cập nhật entity
public interface IDateTracking
{
    // CreatedDate: Thời điểm entity được tạo
    // DateTimeOffset: Lưu trữ thời gian có tính đến múi giờ
    // Không nullable vì mọi entity đều phải có thời điểm tạo
    DateTimeOffset CreatedDate { get; set; }

    // LastModifiedDate: Thời điểm entity được cập nhật lần cuối
    // Nullable (?) vì entity mới tạo chưa có cập nhật
    // DateTimeOffset để lưu trữ thời gian có tính đến múi giờ
    DateTimeOffset? LastModifiedDate { get; set; }
}