using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curriculum.Blazor.Pages
{
    public partial class CustomNotFound
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public void NavigateToHome()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
