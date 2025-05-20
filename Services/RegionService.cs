using DTOs;
using ExceptionHandler.General;
using MapsterMapper;
using Repository.Contracts;
using Service.Contracts;

namespace Services
{
    public class RegionService(IMapper mapper, IRegionRepository repository) : IRegionService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IRegionRepository _repository = repository;

        public async Task<IEnumerable<RegionDTO>> GetAllRegionsAsync()
        {
            var result = await _repository.GetAllRegionsAsync()
                ?? throw new NotFoundListException();

            return _mapper.Map<IEnumerable<RegionDTO>>(result);
        }
    }
}
