using DTOs;
using ExceptionHandler.General;
using MapsterMapper;
using Repository.Contracts;
using Service.Contracts;

namespace Services
{
    public class ShippingService : IShippingService
    {
        private readonly IMapper _mapper;
        private readonly IShippingRepository _shippingRepository;

        public ShippingService(IMapper mapper,
            IShippingRepository shippingRepository)
        {
            _mapper = mapper;
            _shippingRepository = shippingRepository;
        }

        public async Task<IEnumerable<ShippingDTO>> GetShippingListAsync()
        {
            var result = await _shippingRepository.GetShippingListAsync();

            if (result == null || !result.Any())
            {
                throw new NotFoundListException();
            }

            return _mapper.Map<IEnumerable<ShippingDTO>>(result);
        }
    }
}
