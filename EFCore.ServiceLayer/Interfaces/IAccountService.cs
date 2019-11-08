using EFCore.DTO;
using System.Net;
using System.Threading.Tasks;

namespace EFCore.ServiceLayer
{
    public interface IAccountService
    {
        Task<RegisterUserDTO> RegisterAsync(RegisterUserDTO registerDto);

        Task<HttpStatusCode> SendEmailAsync(EmailDTO emailDto);

        Task<bool> RegisterConfirmationSuccessAsync(string userId, string code);

        Task SignInAsync(string Id, bool isPersistent = false);

        Task<bool> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure = true);

        Task SignOutAsync();

        Task<IdentityResultDTO> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

        Task<ForgotPasswordDTO> ForgotPasswordAsync(string email);

        Task<IdentityResultDTO> ResetPasswordAsync(string userId, string code, string password);

        Task<ApplicationUserDTO> GetUser(string userId);

        Task<ApplicationUserDTO> UpdateUserAsync(ApplicationUserDTO applicationUserDto);
        bool IsSignedIn(System.Security.Claims.ClaimsPrincipal user);
    }
}
