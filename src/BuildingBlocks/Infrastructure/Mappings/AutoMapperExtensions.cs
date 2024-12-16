using System.Reflection;        // Cho BindingFlags và Reflection
using AutoMapper;              // Cho IMappingExpression

namespace Infrastructure.Mappings;

/// <summary>
/// Class static chứa extension method cho AutoMapper
/// </summary>
public static class AutoMapperExtension
{
    /// <summary>
    /// Extension method mở rộng IMappingExpression để ignore các properties không tồn tại trong source
    /// </summary>
    /// <typeparam name="TSource">Kiểu dữ liệu nguồn</typeparam>
    /// <typeparam name="TDestination">Kiểu dữ liệu đích</typeparam>
    /// <param name="expression">IMappingExpression để cấu hình mapping</param>
    public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
        (this IMappingExpression<TSource, TDestination> expression)
    {
        // Flags để lấy public instance properties
        var flags = BindingFlags.Public | BindingFlags.Instance;

        // Lấy type của source object
        var sourceType = typeof(TSource);

        // Lấy tất cả properties của destination type
        var destinationProperties = typeof(TDestination).GetProperties(flags);

        // Duyệt qua từng property của destination
        foreach (var property in destinationProperties)
        {
            // Kiểm tra nếu property không tồn tại trong source
            if (sourceType.GetProperty(property.Name, flags) == null)
                // Cấu hình AutoMapper bỏ qua property này
                expression.ForMember(property.Name, opt => opt.Ignore());
        }

        return expression;
    }
}