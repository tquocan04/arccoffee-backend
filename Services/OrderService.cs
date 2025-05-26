using DTOs;
using DTOs.Requests;
using Entities;
using Entities.Context;
using ExceptionHandler.Order;
using ExceptionHandler.Product;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
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

        private async Task<CartDTO> UpdateCartItemsWithProductInfoAsync(CartDTO cartDTO)
        {
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

        public async Task<CartDTO> GetCartAsync(string email)
        {
            var user = await _userService.GetProfileAsync(email);

            var cart = await _orderRepository.GetCartByCustomerIdAsync(user.Id);

            var cartDTO = _mapper.Map<CartDTO>(cart);

            return await UpdateCartItemsWithProductInfoAsync(cartDTO);
        }

        public async Task AddToCartAsync(string id, Guid productId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
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

        public async Task<CartDTO> MergeCartFromClientAsync(string customerId, List<ItemRequest> req)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Order order = await _orderRepository.GetCartWithoutItemsByCustomerIdAsync(customerId);

                Guid cartId = order.Id;

                var existingItems = await _context.Items
                    .Where(i => i.OrderId == cartId)
                    .ToListAsync();

                foreach (var itemReq in req)
                {
                    Product? product = await _productRepository.GetProductByIdAsync(itemReq.ProductId)
                        ?? throw new NotFoundProductException(itemReq.ProductId);

                    int quantity = Math.Min(itemReq.Quantity, product.Stock);

                    var existingItem = existingItems.FirstOrDefault(i => i.ProductId == itemReq.ProductId);

                    if (existingItem != null)
                    {
                        order.TotalPrice -= existingItem.Quantity * product.Price;
                        if (order.TotalPrice < 0)
                            order.TotalPrice = 0;

                        existingItem.Quantity = Math.Min(existingItem.Quantity + itemReq.Quantity, product.Stock);
                        order.TotalPrice += existingItem.Quantity * product.Price;

                        _context.Entry(existingItem).Property(i => i.Quantity).IsModified = true;
                    }
                    else
                    {
                        Item newItem = new()
                        {
                            OrderId = cartId,
                            ProductId = itemReq.ProductId,
                            Quantity = quantity
                        };
                        order.TotalPrice += newItem.Quantity * product.Price;

                        await _itemRepository.Create(newItem);
                    }

                    product.Stock -= quantity;
                    _context.Attach(product);
                    _context.Entry(product).Property(p => p.Stock).IsModified = true;
                }

                _context.Attach(order);
                _context.Entry(order).Property(o => o.TotalPrice).IsModified = true;

                await _context.SaveChangesAsync();

                Order newOrder = await _orderRepository.GetCartByCustomerIdAsync(customerId);

                CartDTO cartDTO = await UpdateCartItemsWithProductInfoAsync(_mapper.Map<CartDTO>(newOrder));

                await transaction.CommitAsync();
                return cartDTO;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Concurrency Error: {ex.Message}, Inner: {ex.InnerException?.Message}");
                await transaction.RollbackAsync();
                throw new Exception("Dữ liệu đã bị thay đổi. Vui lòng thử lại.", ex);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"DbUpdate Error: {ex.Message}, Inner: {ex.InnerException?.Message}");
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}, Inner: {ex.InnerException?.Message}");
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task UpdateQuantityItemAsync(string customerId, Guid productId, int quantity)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var cart = await _orderRepository.GetCartWithoutItemsByCustomerIdAsync(customerId);

                var cartId = cart.Id;

                Product product = await _productRepository.GetProductByIdAsync(productId)
                    ?? throw new NotFoundProductException(productId);

                Item existingItem = await _itemRepository.GetItemAsync(cartId, productId)
                    ?? throw new NotFoundItemException();

                if (quantity > product.Stock)
                    throw new BadRequestProductOutOfStockException();

                cart.TotalPrice -= existingItem.Quantity * product.Price;

                if (cart.TotalPrice < 0)
                    cart.TotalPrice = 0;

                product.Stock += existingItem.Quantity;

                existingItem.Quantity = quantity;

                _context.Entry(existingItem).Property(i => i.Quantity).IsModified = true;

                //update total price
                cart.IncreaseTotalPrice(existingItem.Quantity, product.Price);

                _context.Attach(cart);
                _context.Entry(cart).Property(o => o.TotalPrice).IsModified = true;

                //update product stock
                product.Stock -= existingItem.Quantity;

                _context.Attach(product);
                _context.Entry(product).Property(p => p.Stock).IsModified = true;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}, Inner: {ex.InnerException?.Message}");
                await transaction.RollbackAsync();
                throw;
            }
            
        }
    }
}
