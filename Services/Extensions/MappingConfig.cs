using DTOs;
using DTOs.Requests;
using Entities;
using Mapster;

namespace Services.Extensions
{
    public static class MappingConfig
    {
        public static void Configure()
        {
            var config = TypeAdapterConfig.GlobalSettings;

            config.NewConfig<CreateCategoryRequest, Category>();
            config.NewConfig<CategoryDTO, Category>().TwoWays();

            config.NewConfig<ShippingDTO, ShippingMethod>().TwoWays();
            
            config.NewConfig<CreateProductRequest, Product>().TwoWays();
            config.NewConfig<ProductDTO, Product>().TwoWays();
        }
    }
}
