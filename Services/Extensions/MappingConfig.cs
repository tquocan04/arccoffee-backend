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
        }
    }
}
