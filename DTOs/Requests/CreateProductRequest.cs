using Microsoft.AspNetCore.Http;

namespace DTOs.Requests
{
    public record CreateProductRequest
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public double? Price { get; init; }
        public int? Stock { get; init; }
        public IFormFile? Image { get; init; }
        public Guid CategoryId { get; init; }
    }
}
