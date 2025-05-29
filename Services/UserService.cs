using DTOs;
using DTOs.Requests;
using DTOs.Responses;
using Entities;
using Entities.Context;
using ExceptionHandler.Address;
using ExceptionHandler.Branch;
using ExceptionHandler.General;
using ExceptionHandler.User;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IAddressService<UserUpdateDTO> _addressUpdateService;
        private readonly IAddressService<StaffDTO> _addressStaffService;
        private readonly IBranchRepository _branchRepository;
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
            IAddressService<UserUpdateDTO> addressUpdateService,
            IAddressService<StaffDTO> addressStaffService,
            IBranchRepository branchRepository,
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
            _addressUpdateService = addressUpdateService;
            _addressStaffService = addressStaffService;
            _branchRepository = branchRepository;
            _cloudinary = cloudinary;
        }

        private static (int? year, int? month, int? day) GetDobFromDateOnly(DateOnly dateOnly)
        {
            return (dateOnly.Year, dateOnly.Month, dateOnly.Day);
        }

        public async Task<(string, string?, string?)> LoginAsync(LoginRequest req)
        {
            var user = await _userManager.FindByEmailAsync(req.Login)
                ?? throw new BadRequestLoginException();

            var login = await _signInManager.PasswordSignInAsync(user, req.Password, false, false);

            if (!login.Succeeded)
                throw new BadRequestLoginException();

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            string token = _tokenService.GenerateToken(req, role, user.Id);

            return (token, user.Picture, role);
        }
        
        public async Task<(string, string?)> LoginByGoogleAsync(string googleId)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.GoogleId == googleId)
                ?? throw new NotFoundUserByGoogleIdException();

            LoginRequest req = new()
            {
                Login = user.Email,
                Password = ""
            };

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            string token = _tokenService.GenerateToken(req, role, user.Id);

            return (token, user.Picture);
        }

        private async Task CreateAddressAsync<T>(T req, User user) where T : class
        {
            Address address = new()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                IsDefault = true,
            };
            _mapper.Map(req, address);

            await _addressRepository.Create(address);
        }

        private async Task<CustomerResponse> CreateAddressAndOrderAsync<T>(T req, User user) where T : class
        {
            await CreateAddressAsync<T>(req, user);
            
            Order order = new()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                TotalPrice = 0,
            };

            var addressTask = CreateAddressAsync<T>(req, user);
            var orderTask = _orderRepository.Create(order);
            await Task.WhenAll(addressTask, orderTask);

            CustomerResponse response = new()
            {
                Id = user.Id,
                Picture = user.Picture,
                OrderId = order.Id,
            };

            _mapper.Map(req, response);

            return response;
        }

        public async Task<CustomerResponse> SignUpGoogleAsync(SignupGoogleRequest req)
        {
            User? user = await _userManager.FindByEmailAsync(req.Email);

            if (user != null)
                throw new BadRequestUserExistsByEmailException(req.Email);

            if (!_userRepository.CheckValidDob(req.Day, req.Month, req.Year))
                throw new BadRequestInvalidDobException();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                user = _mapper.Map<User>(req);

                user.Dob = new DateOnly(req.Year, req.Month, req.Day);
                user.UserName = req.Email;
                user.Id = Guid.NewGuid().ToString();

                var result = await _userManager.CreateAsync(user, req.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Customer");
                }

                var response = await CreateAddressAndOrderAsync<SignupGoogleRequest>(req, user);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                
                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<CustomerResponse> CreateNewCustomerAsync(RegisterRequest req)
        {
            User? user = await _userManager.FindByEmailAsync(req.Email);

            if (user != null)
                throw new BadRequestUserExistsByEmailException(req.Email);

            if (!_userRepository.CheckValidDob(req.Day, req.Month, req.Year))
                throw new BadRequestInvalidDobException();

            using var transaction = await _context.Database.BeginTransactionAsync();
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

                var response = await CreateAddressAndOrderAsync<RegisterRequest>(req, user);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<UserDTO> GetProfileAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundUserByEmailException(email);

            UserDTO result = _mapper.Map<UserDTO>(user);

            (result.Year, result.Month, result.Day) = GetDobFromDateOnly(user.Dob);

            result = await _addressService.SetAddressAsync(result, Guid.Parse(user.Id));

            return result;
        }

        public async Task<UserUpdateDTO> UpdateUserAsync(string email, UpdateUserRequest req)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundUserByEmailException(email);

            if (!_userRepository.CheckValidDob(req.Day, req.Month, req.Year))
                throw new BadRequestInvalidDobException();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var address = await _addressRepository.GetAddressByObjectIdAsync(Guid.Parse(user.Id))
                    ?? throw new NotFoundAddressException(Guid.Parse(user.Id));

                var picture = user.Picture;

                _mapper.Map(req, user);

                if (req.Picture != null && req.Picture.Length > 0)
                {
                    var url = await _cloudinary.UploadImageCustomerAsync(req.Picture);
                    user.Picture = url;
                }
                else
                {
                    user.Picture = picture;
                }

                user.Dob = new DateOnly(req.Year, req.Month, req.Day);

                var resultUpdate = await _userManager.UpdateAsync(user);

                if (!resultUpdate.Succeeded)
                {
                    await transaction.RollbackAsync();
                    throw new Exception();
                }

                if (address.DistrictId != req.DistrictId || address.Street != req.Street)
                {
                    _mapper.Map(req, address);
                    _addressRepository.Update(address);
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                var result = _mapper.Map<UserUpdateDTO>(user);

                result = await _addressUpdateService.SetAddressAsync(result, Guid.Parse(user.Id));

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task ChangePasswordAsync(string email, ChangePasswordRequest req)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundUserByEmailException(email);

            bool checkCurrentPassword = await _userManager.CheckPasswordAsync(user, req.CurrentPassword);

            if (!checkCurrentPassword)
            {
                throw new BadRequestCurrentPasswordException();
            }

            var result = await _userManager.ChangePasswordAsync(user, req.CurrentPassword,
                req.NewPassword);

            if (!result.Succeeded)
                throw new BadRequestChangePasswordException();
        }

        public async Task<StaffDTO> CreateNewStaffAsync(CreateStaffRequest req)
        {
            User? user = await _userManager.FindByEmailAsync(req.Email);

            if (user != null)
                throw new BadRequestUserExistsByEmailException(req.Email);

            if (!_userRepository.CheckValidDob(req.Day, req.Month, req.Year))
                throw new BadRequestInvalidDobException();

            Branch? branch = await _branchRepository.GetBranchByIdAsync(req.BranchId)
                ?? throw new NotFoundBranchException();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                user = _mapper.Map<User>(req);

                user.Dob = new DateOnly(req.Year, req.Month, req.Day);
                user.UserName = req.Email;
                user.Id = Guid.NewGuid().ToString();

                var result = await _userManager.CreateAsync(user, req.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Staff");
                }

                await CreateAddressAsync<CreateStaffRequest>(req, user);

                var response = _mapper.Map<StaffDTO>(req);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                
                response = await _addressStaffService.SetAddressAsync(response, Guid.Parse(user.Id));
                response.BranchName = branch.Name;
                response.Id = user.Id;

                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

                response.Role = !string.IsNullOrEmpty(role) ? role : "Staff";

                return response;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<StaffDTO> GetStaffProfileAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundUserByEmailException(email);

            var result = _mapper.Map<StaffDTO>(user);

            result = await _addressStaffService.SetAddressAsync(result, Guid.Parse(user.Id));

            if (user.BranchId == Guid.Empty || user.BranchId == null)
                throw new NotFoundBranchException();

            Branch? branch = await _branchRepository.GetBranchByIdAsync((Guid)user.BranchId)
                ?? throw new NotFoundBranchException();

            result.BranchName = branch.Name;

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            result.Role = !string.IsNullOrEmpty(role) ? role : "Default";

            result.Year = user.Dob.Year;
            result.Month = user.Dob.Month;
            result.Day = user.Dob.Day;

            return result;
        }

        public async Task DeleteStaffAsync(string email)
        {
            User? user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundUserByEmailException(email);

            await _userManager.DeleteAsync(user);
        }

        public async Task UpdateStaffProfileAsync(string email, CreateStaffRequest req)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(email)
                ?? throw new NotFoundUserByEmailException(email);

                if (!_userRepository.CheckValidDob(req.Day, req.Month, req.Year))
                    throw new BadRequestInvalidDobException();

                _mapper.Map(req, user);
                
                user.Dob = new DateOnly(req.Year, req.Month, req.Day);

                var resultUpdate = await _userManager.UpdateAsync(user);

                if (!resultUpdate.Succeeded)
                {
                    await transaction.RollbackAsync();
                    throw new Exception();
                }

                var defaultAddress = await _addressRepository.GetAddressByObjectIdAsync(Guid.Parse(user.Id))
                    ?? throw new NotFoundAddressException(Guid.Parse(user.Id));

                if (defaultAddress.DistrictId != req.DistrictId || defaultAddress.Street != req.Street)
                {
                    _mapper.Map(req, defaultAddress);
                    _addressRepository.Update(defaultAddress);
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<StaffDTO>> GetStaffListAsync()
        {
            IList<User> staffList = await _userManager.GetUsersInRoleAsync("Staff");

            if (staffList == null || staffList.Count == 0)
                throw new NotFoundListException();

            var result = _mapper.Map<IList<StaffDTO>>(staffList);

            int length = result.Count;

            for (int i = 0; i < length; i++)
            {
                (result[i].Year, result[i].Month, result[i].Day) = GetDobFromDateOnly(staffList[i].Dob);

                if (await _addressRepository.GetAddressByObjectIdAsync(Guid.Parse(result[i].Id)) == null)
                {
                    throw new NotFoundAddressException(Guid.Parse(result[i].Id));
                }

                if (result[i].BranchId != null && result[i].BranchId != Guid.Empty)
                {
                    var branch = await _branchRepository.GetBranchByIdAsync((Guid)result[i].BranchId)
                        ?? throw new NotFoundBranchException();

                    result[i].BranchName = branch.Name;
                }

                result[i] = await _addressStaffService.SetAddressAsync(result[i], Guid.Parse(result[i].Id));

                var role = (await _userManager.GetRolesAsync(staffList[i])).FirstOrDefault();

                result[i].Role = role ?? "Default";
            }

            return result;
        }
    }
}
