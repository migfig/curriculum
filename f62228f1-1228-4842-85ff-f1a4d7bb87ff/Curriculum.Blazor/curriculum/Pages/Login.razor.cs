using Microsoft.AspNetCore.Components;
using MudBlazor;
using Curriculum.EF.Models;

namespace Curriculum.Blazor.Pages
{
    public partial class Login
    {
        private CreateSignInRequest signInRequest = new CreateSignInRequest
        {
            Email = "viewer.user@domain.com",
            Password = "tvJ3R4sP2feQ5by!",
            RememberMe = true
        };

        private ActionResult<UserDTO> loginResult = new ActionResult<UserDTO>(false, null, null);

        bool ShowPassword { get; set; } = false;
        InputType PasswordInputType => ShowPassword ? InputType.Text : InputType.Password;
        string PasswordInputIcon => ShowPassword ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;

        public async Task ExecuteLogin()
        {
            loginResult = await AuthenticationService.Login(signInRequest);
            if (loginResult.Succeeded)
            {
                NavigationManager.NavigateTo("/home");
            }
        }

        protected IEnumerable<string> MaxCharacters(string ch, int maxLen)
        {
            if (!string.IsNullOrEmpty(ch) && maxLen < ch?.Length)
                yield return $"Max {maxLen} characters";
        }
    }
}
