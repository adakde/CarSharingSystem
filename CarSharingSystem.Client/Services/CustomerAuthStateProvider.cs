using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarSharingSystem.Client.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly AuthService _authService;

        public CustomAuthStateProvider(AuthService authService)
        {
            _authService = authService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _authService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            Console.WriteLine($"[AuthStateProvider] Token: {(string.IsNullOrEmpty(token) ? "BRAK" : token.Substring(0, Math.Min(30, token.Length)) + "...")}");

            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("[AuthStateProvider] Brak tokena, użytkownik niezalogowany.");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }


            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                // 🔹 MAPOWANIE CLAIMÓW – zamień długie URI na krótsze typy
                var mappedClaims = jwt.Claims.Select(c =>
                {
                    if (c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                        return new Claim("role", c.Value); // 👈 Blazor rozpozna to w IsInRole()
                    if (c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                        return new Claim(ClaimTypes.Name, c.Value);
                    return c;
                });

                Console.WriteLine($"[AuthStateProvider] Claims: {string.Join(", ", mappedClaims.Select(c => c.Type + '=' + c.Value))}");

                var identity = new ClaimsIdentity(
                    mappedClaims,
                    authenticationType: "jwtAuth",
                    nameType: ClaimTypes.Name,
                    roleType: "role" // 👈 to mówi Blazorowi, że claim "role" to rola
                );
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch (Exception ex)
            {   
                Console.WriteLine($"[AuthStateProvider] ERROR: {ex.Message}");
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public void NotifyUserAuthentication()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void NotifyUserLogout()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }
    }
}
