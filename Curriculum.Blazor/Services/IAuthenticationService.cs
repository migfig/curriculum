using Curriculum.EF.Models;

namespace Curriculum.Blazor
{
    public interface IAuthenticationService
    {
        Task<ActionResult<UserDTO>> RegisterUser(CreateSignUpRequest request);
        Task<ActionResult<UserDTO>> ResetPassword(CreatePasswordResetRequest request);
        Task<ActionResult<UserDTO>> Login(CreateSignInRequest request);
        Task Logout();
    }
}
