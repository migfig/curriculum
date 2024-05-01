using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using MudBlazor;
using Curriculum.EF.Models;

namespace Curriculum.Blazor.Pages
{
    public partial class Registration
    {
        private CreateSignUpRequest signUpRequest = new CreateSignUpRequest();

        private ActionResult<UserDTO> actionResult = new ActionResult<UserDTO>(false, null, null);

        bool ShowPassword { get; set; } = false;
        InputType PasswordInputType => ShowPassword ? InputType.Text : InputType.Password;
        string PasswordInputIcon => ShowPassword ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;

        MudTextField<string> passwordField;

        public async Task Register()
        {

            // actionResult = await AuthenticationService.RegisterUser(signUpRequest);
            // if (actionResult.Succeeded)
            // {
                NavigationManager.NavigateTo("/login");
            // }
        }

        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Password is required!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "Password must be at least of length 8";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Password must contain at least one digit";
        }

        private string PasswordMatch(string arg)
        {
            if (passwordField.Value != arg)
                return "Passwords don't match";
            return null;
        }

        protected IEnumerable<string> MaxCharacters(string ch, int maxLen)
        {
            if (!string.IsNullOrEmpty(ch) && maxLen < ch?.Length)
                yield return $"Max {maxLen} characters";
        }

        protected IEnumerable<string> MaxCharactersAndPwdStrength(string ch, int maxLen)
        {
            if (!string.IsNullOrEmpty(ch) && maxLen < ch?.Length)
                yield return $"Max {maxLen} characters";

            foreach (var item in PasswordStrength(ch))
            {
                yield return item;
            }
        }
    }
}
