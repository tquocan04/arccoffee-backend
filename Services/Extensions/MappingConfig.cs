using DTOs;
using DTOs.Requests;
using DTOs.Responses;
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

            config.NewConfig<ItemDTO, Product>()
                .Map(dest => dest.Id, src => src.ProductId)
                .TwoWays();
            
            config.NewConfig<RegionDTO, Region>().TwoWays();
            config.NewConfig<CityDTO, City>().TwoWays();
            config.NewConfig<DistrictDTO, District>().TwoWays();

            config.NewConfig<RegisterRequest, User>().TwoWays();
            config.NewConfig<UserDTO, User>().TwoWays();
            config.NewConfig<RegisterRequest, Address>().TwoWays();
            config.NewConfig<UpdateUserRequest, Address>().TwoWays();
            config.NewConfig<RegisterRequest, CustomerResponse>()
                .Ignore(dest => dest.Picture)
                .TwoWays();
        }
    }
}
