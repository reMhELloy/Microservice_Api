namespace Contracts.Domains.Interfaces;

// Interface định nghĩa việc theo dõi người dùng thực hiện thao tác
public interface IUserTracking
{
    // CreatedBy: Lưu thông tin người tạo record
    // Thường là username hoặc id của user
    string CreatedBy { get; set; }

    // LastModifiedBy: Lưu thông tin người cập nhật record lần cuối
    // Thường là username hoặc id của user
    string LastModifiedBy { get; set; }
}