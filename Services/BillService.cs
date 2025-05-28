using DTOs;
using DTOs.Enums;
using DTOs.Requests;
using Entities;
using Entities.Context;
using ExceptionHandler.Address;
using ExceptionHandler.Bill;
using ExceptionHandler.General;
using ExceptionHandler.Payment;
using ExceptionHandler.Shipping;
using ExceptionHandler.User;
using ExceptionHandler.Voucher;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Repository.Contracts;
using Service.Contracts;

namespace Services
{
    public class BillService(IMapper mapper,
        Context context,
        UserManager<User> userManager,
        IOrderRepository orderRepository,
        IVoucherRepository voucherRepository,
        IPaymentRepository paymentRepository,
        IShippingRepository shippingRepository,
        IAddressRepository addressRepository,
        IProductRepository productRepository,
        IBillRepository billRepository) : IBillService
    {
        private async Task UpdateBillItemsWithProductInfoAsync(BillDTO billDTO)
        {
            if (billDTO.Items != null && billDTO.Items.Count > 0)
            {
                foreach (var item in billDTO.Items)
                {
                    var product = await productRepository.GetProductByIdAsync(item.ProductId);
                    mapper.Map(product, item);
                }
            }
        }

        public async Task<BillDTO> CreateNewBillAsync(string email, BillRequest req)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var user = await userManager.FindByEmailAsync(email)
                    ?? throw new NotFoundUserByEmailException(email);

                if (string.IsNullOrEmpty(req.PaymentId) || string.IsNullOrWhiteSpace(req.PaymentId))
                    throw new NotFoundPaymentByIdException(req.PaymentId);

                if (string.IsNullOrEmpty(req.ShippingId) || string.IsNullOrWhiteSpace(req.ShippingId))
                    throw new NotFoundShippingByIdException(req.ShippingId);

                var payment = await paymentRepository.GetPaymentByIdAsync(req.PaymentId)
                    ?? throw new NotFoundPaymentByIdException(req.PaymentId);

                var shipping = await shippingRepository.GetShippingByIdAsync(req.ShippingId)
                    ?? throw new NotFoundShippingByIdException(req.ShippingId);

                var order = await orderRepository.GetCartByCustomerIdAsync(user.Id, true);

                if (order.TotalPrice <= 0)
                {
                    throw new BadRequestBillByTotalPriceException();
                }

                mapper.Map(req, order);

                order.IsCart = false;
                order.Status = OrderStatus.Pending.ToString();
                order.OrderDate = DateTime.Now;
                order.ShippingMethodId = req.ShippingId;

                if (!string.IsNullOrEmpty(req.VoucherCode) || !string.IsNullOrWhiteSpace(req.VoucherCode))
                {
                    var voucher = await voucherRepository.GetVoucherByCodeAsync(req.VoucherCode, true)
                        ?? throw new NotFoundVoucherByCodeException(req.VoucherCode);

                    bool isValidVoucher = await voucherRepository.IsValidVoucherByIdAsync(voucher.Id,
                        DateOnly.FromDateTime(DateTime.Now), voucher.MinOrderValue);

                    if (isValidVoucher)
                    {
                        decimal discount = (order.TotalPrice * voucher.Percentage / 100);
                        if (discount > voucher.MaxDiscount)
                        {
                            order.TotalPrice -= voucher.MaxDiscount;
                        }
                        else
                        {
                            order.TotalPrice -= discount;
                        }
                        order.VoucherId = voucher.Id;
                        voucher.Quantity -= 1;
                    }
                }

                var addressDefault = await addressRepository.GetAddressByObjectIdAsync(Guid.Parse(user.Id));

                if (addressDefault == null)
                {
                    await transaction.CommitAsync();
                    throw new NotFoundAddressException(Guid.Parse(user.Id));
                }

                if (req.DistrictId != addressDefault.DistrictId
                    || req.Street != addressDefault.Street)
                {
                    List<Address> addresses = await addressRepository.GetListAddressOfCustomerAsync(user.Id);
                    bool check = true;
                    for (int i = 0; i < addresses.Count; i++)
                    {
                        if (req.DistrictId == addresses[i].DistrictId
                            && req.Street == addresses[i].Street)
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check)
                    {
                        Address address = new()
                        {
                            Id = Guid.NewGuid(),
                            Street = req.Street,
                            DistrictId = req.DistrictId,
                            UserId = user.Id,
                            IsDefault = false,
                        };

                        await addressRepository.Create(address);
                    }
                }

                Order newOrder = new()
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    TotalPrice = 0,
                };

                await orderRepository.Create(newOrder);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                var result = mapper.Map<BillDTO>(order);
                await UpdateBillItemsWithProductInfoAsync(result);

                if (!string.IsNullOrEmpty(result.CustomerId))
                    result.Name = user.Name;

                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<BillDTO>> GetBillListAsync(string? email, string status)
        {
            if (email != null)
            {
                var user = await userManager.FindByEmailAsync(email)
                    ?? throw new NotFoundUserByEmailException(email);

                var bills = await billRepository.GetBillListAsync(user.Id, status);

                if (bills == null || !bills.Any())
                    throw new NotFoundListException();

                var result = mapper.Map<IList<BillDTO>>(bills);

                int length = result.Count;

                for (int i = 0; i < length; i++)
                {
                    await UpdateBillItemsWithProductInfoAsync(result[i]);
                    result[i].Name = user.Name;
                }

                return result;
            }
            else
            {
                var bills = await billRepository.GetBillListAsync(null, status);

                if (bills == null || !bills.Any())
                    throw new NotFoundListException();

                var result = mapper.Map<IList<BillDTO>>(bills);

                int length = result.Count;

                for (int i = 0; i < length; i++)
                {
                    await UpdateBillItemsWithProductInfoAsync(result[i]);

                    if (!string.IsNullOrEmpty(result[i].CustomerId))
                    {
                        var user = await userManager.FindByIdAsync(result[i].CustomerId)
                            ?? throw new NotFoundUserByEmailException(result[i].CustomerId);

                        result[i].Name = user.Name;
                    }
                }

                return result;
            }
        }
    }
}
