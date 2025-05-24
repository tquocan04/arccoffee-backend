using DTOs;
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
        private readonly SignInManager<User> _signInManager;
        private readonly Context _context;
        private readonly IAddressRepository _addressRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IAddressService<UserDTO> _addressService;
        private readonly CloudinaryService _cloudinary;

        public UserService(IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            Context context,
            IAddressRepository addressRepository,
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            ITokenService tokenService,
            IAddressService<UserDTO> addressService,
            CloudinaryService cloudinary)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _addressRepository = addressRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _addressService = addressService;
            _cloudinary = cloudinary;
        }

        public async Task<CustomerResponse> CreateNewCustomerAsync(RegisterRequest req)
        {
            User? user = await _userManager.FindByEmailAsync(req.Email);

            if (user != null)
                throw new BadRequestUserExistsByEmailException(req.Email);

            if (!_userRepository.CheckValidDob(req.Day, req.Month, req.Year))
                throw new BadRequestInvalidDobException();

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

                    var result = await _userManager.CreateAsync(user, req.Password);

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

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

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
                    throw;
                }
        }

        public async Task<(string?, string?)> LoginAsync(LoginRequest req)
        {
            var user = await _userManager.FindByEmailAsync(req.Login)
                ?? throw new NotFoundUserByEmailException(req.Login);

            var login = await _signInManager.PasswordSignInAsync(user, req.Password, false, false);

            if (!login.Succeeded)
                return (null, null);

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            string token = _tokenService.GenerateToken(req, role, user.Id);

            return (token, user.Picture);
        }

        public async Task<UserDTO> GetProfileAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundUserByEmailException(email);

            UserDTO result = _mapper.Map<UserDTO>(user);

            result.Year = user.Dob.Year;
            result.Month = user.Dob.Month;
            result.Day = user.Dob.Day;

            result = await _addressService.SetAddressAsync(result, Guid.Parse(user.Id));

            return result;
        }
    }
}
