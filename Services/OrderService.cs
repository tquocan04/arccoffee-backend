using DTOs;
using MapsterMapper;
using Repository.Contracts;
using Service.Contracts;

namespace Services
{
    public class OrderService(IMapper mapper, 
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUserService userService)
        : IOrderService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IUserService _userService = userService;

        public async Task<CartDTO> GetCartAsync(string email)
        {
            var user = await _userService.GetProfileAsync(email);

            var cart = await _orderRepository.GetCartByCustomerIdAsync(user.Id);

            var cartDTO = _mapper.Map<CartDTO>(cart);

            if (cartDTO.Items != null)
            {
                foreach (var item in cartDTO.Items)
                {
                    var product = await _productRepository.GetProductByIdAsync(item.ProductId);
                    _mapper.Map(product, item);
                }
            }

            return cartDTO;
        }
    }
}
