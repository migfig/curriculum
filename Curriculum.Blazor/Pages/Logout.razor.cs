using Microsoft.AspNetCore.Components;

namespace Curriculum.Blazor.Pages
{
    public partial class Logout
    {
        protected override async Task OnInitializedAsync()
        {
            await AuthenticationService.Logout();
            NavigationManager.NavigateTo("/login");
        }
    }
}
