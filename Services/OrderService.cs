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
        IItemRepository itemRepository,
        IUserService userService)
        : IOrderService
    {
        private readonly IMapper _mapper = mapper;
        private readonly Context _context = context;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IItemRepository _itemRepository = itemRepository;
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

                    Item? existingItem = await _itemRepository.GetItemAsync(cartId, productId);

                    if (existingItem == null)
                    {
                        Item item = new()
                        {
                            OrderId = cartId,
                            ProductId = productId,
                            Quantity = 1
                        };
                        cart.TotalPrice += product.Price;
                        await _itemRepository.Create(item);
                    }
                    else
                    {
                        existingItem.Quantity++;
                        cart.TotalPrice += product.Price;
                        _itemRepository.Update(existingItem);
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

        public async Task DeleteItemInCartAsync(string id, Guid productId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var cart = await _orderRepository.GetCartByCustomerIdAsync(id);

                var cartId = cart.Id;

                Product? product = await _productRepository.GetProductByIdAsync(productId)
                    ?? throw new NotFoundProductException(productId);

                Item? existingItem = await _itemRepository.GetItemAsync(cartId, productId, true)
                    ?? throw new NotFoundItemException();

                cart.TotalPrice -= (product.Price * existingItem.Quantity);
                if (cart.TotalPrice < 0)
                    cart.TotalPrice = 0;

                product.Stock += existingItem.Quantity;

                _itemRepository.Delete(existingItem);
                await _context.SaveChangesAsync();

                // Gắn cart và product vào Change Tracker
                _context.Attach(cart);
                _context.Attach(product);

                // Đánh dấu chỉ các thuộc tính đã thay đổi
                _context.Entry(cart).Property(o => o.TotalPrice).IsModified = true;
                _context.Entry(product).Property(p => p.Stock).IsModified = true;

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
