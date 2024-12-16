using AutoMapper;
using Infrastructure.Mappings;
using Product.API.Entities;
using Shared.DTOs.Product;

namespace Product.API
{
    /// <summary>
    /// Cấu hình mapping giữa các entities và DTOs
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map từ CatalogProduct sang ProductDto
            // AutoMapper sẽ tự động map các properties có tên giống nhau
            CreateMap<CatalogProduct, ProductDto>();

            // Map từ CreateProductDto sang CatalogProduct
            CreateMap<CreateProductDto, CatalogProduct>();

            // Map từ UpdateProductDto sang CatalogProduct
            CreateMap<UpdateProductDto, CatalogProduct>()
                .IgnoreAllNonExisting();

        }
    }
}