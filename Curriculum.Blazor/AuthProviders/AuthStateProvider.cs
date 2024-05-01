using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Curriculum.EF.Models;
using System.Net.Http.Json;

namespace Curriculum.Blazor
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsStringAsync(Helper.AUTH_TOKEN_KEY);
            var tokenHandler = new JwtSecurityTokenHandler();
            var identity = new ClaimsIdentity();

            if (!string.IsNullOrEmpty(token) && tokenHandler.CanReadToken(token))
            {
                var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
                if (IsExpiredToken(jwtSecurityToken)) {
                    await _localStorage.RemoveItemAsync(Helper.AUTH_TOKEN_KEY);
                } else {
                    identity = new ClaimsIdentity(
                        jwtSecurityToken.Claims,
                        "jwtAuthType",
                        "unique_name",
                        ClaimTypes.Role
                    );
                }
            }

            var principal = new ClaimsPrincipal(identity);
            var authenticationState = new AuthenticationState(principal);

            return authenticationState;
        }

        private bool IsExpiredToken(JwtSecurityToken jwtSecurityToken) {
            var now = DateTime.Now.ToUniversalTime();
            // Console.WriteLine($"from={jwtSecurityToken.ValidFrom}, to={jwtSecurityToken.ValidTo}, now={now}");
            return (now < jwtSecurityToken.ValidFrom || now > jwtSecurityToken.ValidTo);
        }

        public async Task<ActionResult<UserDTO>> LoginAsync(CreateSignInRequest request)
        {
            var result = await _httpClient.GetFromJsonAsync<UserDTO>("data/sign-in.json");
            if (result != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
                await _localStorage.SetItemAsStringAsync(Helper.AUTH_TOKEN_KEY, result.Token);

                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                return new ActionResult<UserDTO>(true, result, string.Empty);
            }

            return new ActionResult<UserDTO>(true, result, "Invalid response");
        }

        public async Task LogoutAsync()
        {
            var result = await _httpClient.PostAction<UserDTO, string>("data/sign-out.json", string.Empty);
            if (result.Succeeded)
            {
                await _localStorage.RemoveItemAsync(Helper.AUTH_TOKEN_KEY);
                _httpClient.DefaultRequestHeaders.Authorization = null;

                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }

        public async Task<bool> IsAuthenticatedAsync() {
            var token = await _localStorage.GetItemAsStringAsync(Helper.AUTH_TOKEN_KEY);
            if (string.IsNullOrEmpty(token)) return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            if (!tokenHandler.CanReadToken(token)) return false;
            
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            return !IsExpiredToken(jwtSecurityToken);
        }
        
        public async Task<Guid> GetCurrentUserIdAsync() {
            var id = Guid.Empty;
            var token = await _localStorage.GetItemAsStringAsync(Helper.AUTH_TOKEN_KEY);
            if (string.IsNullOrEmpty(token)) return id;

            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(token)) return id;
            
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            var strId = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            if(!Guid.TryParse(strId, out id)) return Guid.Empty;
            
            return id;
        }
    }
}

public static class Extensions
{
    private const string KS_ROLE = "role";

    public static bool IsAdmin(this IEnumerable<Claim> claims)
    {
        return claims.Where(c => c.Type == KS_ROLE)
            .SelectMany(c => c.Value.Split(';'))
            .Any(v => v.Split('=').First().Equals("Admins"));
    }

    public static bool IsViewer(this IEnumerable<Claim> claims)
    {
        return claims.Where(c => c.Type == KS_ROLE)
            .SelectMany(c => c.Value.Split(';'))
            .Any(v => v.Split('=').First().Equals("Viewers"));
    }

    public static bool Can(this IEnumerable<Claim> claims, string permission)
    {
        return claims.Where(c => c.Type == KS_ROLE)
            .SelectMany(c => c.Value.Split(';'))
            .Any(v => v.Split('=').Last().Split(',').Contains(permission));
    }
}
