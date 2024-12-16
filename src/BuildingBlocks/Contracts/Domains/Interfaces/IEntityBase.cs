namespace Contracts.Domains.Interfaces;

// Interface chung cho toàn bộ hệ thống. Định nghĩa thuộc tính Id cho entity
// T: kiểu dữ liệu của Id (generic type parameter)
public interface IEntityBase<T>
{
    // Property Id với kiểu dữ liệu được xác định bởi T
    // get: cho phép đọc giá trị Id
    // set: cho phép gán giá trị cho Id
    T Id { get; set; }
}