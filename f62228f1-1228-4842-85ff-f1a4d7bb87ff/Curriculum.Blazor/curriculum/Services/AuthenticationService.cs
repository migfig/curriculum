using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Curriculum.EF.Models;

namespace Curriculum.Blazor
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly IApplicationState _applicationState;

        public AuthenticationService(HttpClient client, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage, IApplicationState applicationState)
        {
            _client = client;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _applicationState = applicationState;
        }

        public Task<ActionResult<UserDTO>> RegisterUser(CreateSignUpRequest request)
        {
            return _client.PostAction<UserDTO, CreateSignUpRequest>("data/sign-up.json", request);
        }

        public Task<ActionResult<UserDTO>> ResetPassword(CreatePasswordResetRequest request)
        {
            return _client.PostAction<UserDTO, CreatePasswordResetRequest>("data/reset-pass.json", request);
        }

        public Task<ActionResult<UserDTO>> Login(CreateSignInRequest request)
        {
            return ((AuthStateProvider)_authStateProvider).LoginAsync(request);
        }

        public async Task Logout()
        {
            await ((AuthStateProvider)_authStateProvider).LogoutAsync();
        }
    }
}
