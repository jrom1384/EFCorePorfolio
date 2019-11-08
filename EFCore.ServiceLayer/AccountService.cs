using AutoMapper;
using EFCore.DataLayer.EFClasses;
using EFCore.DTO;
using EFCore.Utilities.SendGrid;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EFCore.ServiceLayer
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public async Task<RegisterUserDTO> RegisterAsync(RegisterUserDTO registerDto)
        {
            var user = new ApplicationUser 
            { 
                LastName = registerDto.LastName, 
                FirstName = registerDto.FirstName, 
                Age = registerDto.Age, 
                UserName = registerDto.Email, 
                Email = registerDto.Email 
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            registerDto.RequireConfirmedEmail = _userManager.Options.SignIn.RequireConfirmedEmail;

            registerDto.Succeeded = result.Succeeded;
            if (registerDto.Succeeded)
            {
                registerDto.Id = user.Id;
                registerDto.Code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }
            else
            {
                registerDto.Errors = result.Errors.Select(e => e.Description).ToList();
            }

            return registerDto;
        }

        public async Task<HttpStatusCode> SendEmailAsync(EmailDTO emailDto)
        {
            return await _emailSender.SendEmailAsync(
                emailDto.Recipient,
                emailDto.Subject,
                null,
                emailDto.HtmlContent);
        }

        public async Task<bool> RegisterConfirmationSuccessAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded;
        }

        public async Task SignInAsync(string userId, bool isPersistent = false)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _signInManager.SignInAsync(user, isPersistent);
        }

        public async Task<bool> PasswordSignInAsync(string email, string password, 
            bool isPersistent, bool lockoutOnFailure = true)
        {
            var result = await _signInManager.PasswordSignInAsync(email,
                    password, isPersistent, lockoutOnFailure);

            if (result.RequiresTwoFactor)
            {

            }
            if (result.IsLockedOut)
            {
                //UserAccount Lock...display Page
            }

            return result.Succeeded;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResultDTO> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var applicationUser = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(applicationUser, currentPassword, newPassword);
            return new IdentityResultDTO
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select( e => e.Description).ToList()
            };
        }

        public async Task<ForgotPasswordDTO> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return new ForgotPasswordDTO(false);
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            return new ForgotPasswordDTO(true, user.Id, user.Email, user.FirstName, code);
        }

        public async Task<IdentityResultDTO> ResetPasswordAsync(string userId, string code, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ResetPasswordAsync(user, code, password);

            return new IdentityResultDTO
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<ApplicationUserDTO> GetUser(string userId)
        {
            var applicationUser = await _userManager.FindByIdAsync(userId);
            return _mapper.Map<ApplicationUserDTO>(applicationUser);
        }

        public async Task<ApplicationUserDTO> UpdateUserAsync(ApplicationUserDTO applicationUserDto)
        {
            var applicationUser = await _userManager.FindByIdAsync(applicationUserDto.Id);
            applicationUser.FirstName = applicationUserDto.FirstName;
            applicationUser.LastName = applicationUserDto.LastName;
            applicationUser.Age = applicationUserDto.Age;
            var result = await _userManager.UpdateAsync(applicationUser);

            return _mapper.Map<ApplicationUserDTO>(applicationUser);
        }

        public bool IsSignedIn(ClaimsPrincipal user)
        {
            return _signInManager.IsSignedIn(user);
        }
    }
}
