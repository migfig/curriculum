using Microsoft.AspNetCore.Components;
using MudBlazor;
using Curriculum.EF.Models;

namespace Curriculum.Blazor.Pages
{
    public partial class ResetPassword
    {
        private CreatePasswordResetRequest resetPwdRequest = new CreatePasswordResetRequest
        {
            Email = "jane.doe@domain.com",
            Password = "tvJ3R4sP2feQ5by!",
            ConfirmPassword = "tvJ3R4sP2feQ5by!",
        };

        private ActionResult<UserDTO> resetResult = new ActionResult<UserDTO>(false, null, null);

        bool ShowPassword { get; set; } = false;
        InputType PasswordInputType => ShowPassword ? InputType.Text : InputType.Password;
        string PasswordInputIcon => ShowPassword ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;

        public async Task ExecuteReset()
        {
            // resetResult = await AuthenticationService.ResetPassword(resetPwdRequest);
            // if (resetResult.Succeeded)
            // {
                NavigationManager.NavigateTo("/login");
            // }
        }

        protected IEnumerable<string> MaxCharacters(string ch, int maxLen)
        {
            if (!string.IsNullOrEmpty(ch) && maxLen < ch?.Length)
                yield return $"Max {maxLen} characters";
        }
    }
}
