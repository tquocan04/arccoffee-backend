using DTOs.Requests;
using DTOs.Responses;
using Entities;
using Entities.Context;
using ExceptionHandler.User;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Repository.Contracts;
using Service.Contracts;
using Services.Extensions;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly Context _context;
        private readonly IAddressRepository _addressRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly CloudinaryService _cloudinary;

        public UserService(IMapper mapper,
            UserManager<User> userManager,
            Context context,
            IAddressRepository addressRepository,
            IOrderRepository orderRepository,
            CloudinaryService cloudinary) 
        {
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _cloudinary = cloudinary;
        }

        public async Task<CustomerResponse> CreateNewCustomerAsync(RegisterRequest req)
        {
            User? user = await _userManager.FindByEmailAsync(req.Email);

            if (user != null)
                throw new BadRequestUserExistsByEmailException(req.Email);

            using (var transaction = await _context.Database.BeginTransactionAsync())
                try
                {
                    user = _mapper.Map<User>(req);

                    if (req.Picture != null && req.Picture.Length != 0)
                    {
                        var url = await _cloudinary.UploadImageCustomerAsync(req.Picture);
                        user.Picture = url;
                    }

                    user.Dob = new DateOnly(req.Year, req.Month, req.Day);
                    user.UserName = req.Email;
                    user.Id = Guid.NewGuid().ToString();

                    var result = await _userManager.CreateAsync(user);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Customer");
                    }

                    Address address = new()
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        IsDefault = true,
                    };

                    _mapper.Map(req, address);

                    Order order = new()
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        TotalPrice = 0,
                    };

                    var addressTask = _addressRepository.Create(address);
                    var orderTask = _orderRepository.Create(order);

                    await Task.WhenAll(addressTask, orderTask);

                    await transaction.CommitAsync(); 

                    await _context.SaveChangesAsync();

                    CustomerResponse response = new()
                    {
                        Id = user.Id,
                        Picture = user.Picture,
                        OrderId = order.Id,
                    };
                    
                    _mapper.Map(req, response);

                    return response;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine(ex.Message);
                    throw new Exception();
                }
                

        }
    }
}
