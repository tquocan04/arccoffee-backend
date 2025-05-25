using DTOs;
using Entities;
using Entities.Context;
using ExceptionHandler.Order;
using ExceptionHandler.Product;
using MapsterMapper;
using Repository.Contracts;
using Service.Contracts;

namespace Services
{
    public class OrderService(IMapper mapper,
        Context context,
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUserService userService)
        : IOrderService
    {
        private readonly IMapper _mapper = mapper;
        private readonly Context _context = context;
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

        public async Task AddToCartAsync(string id, Guid productId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
                try
                {
                    var cart = await _orderRepository.GetCartByCustomerIdAsync(id);

                    var cartId = cart.Id;

                    Product? product = await _productRepository.GetProductByIdAsync(productId, true)
                        ?? throw new NotFoundProductException(productId);

                    if (product.Stock <= 0)
                        throw new BadRequestProductOutOfStockException();

                    Item? existingItem = await _orderRepository.GetItemAsync(cartId, productId);

                    if (existingItem == null)
                    {
                        Item item = new()
                        {
                            OrderId = cartId,
                            ProductId = productId,
                            Quantity = 1
                        };
                        cart.TotalPrice += product.Price;
                        _orderRepository.AddItemToCart(item);
                    }
                    else
                    {
                        existingItem.Quantity++;
                        cart.TotalPrice += product.Price;
                        _orderRepository.UpdateQuantityItemToCart(existingItem);
                    }

                    var order = await _orderRepository.GetOrderByIdAsync(cartId, true)
                        ?? throw new NotFoundOrderException(cartId);

                    order.TotalPrice = cart.TotalPrice;

                    product.Stock--;
                    
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
        }
    }
}
