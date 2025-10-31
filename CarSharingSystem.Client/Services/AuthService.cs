using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace CarSharingSystem.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly LocalStorage _localStorage;
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationManager _nav;

        public AuthService(HttpClient http, LocalStorage localStorage, IServiceProvider serviceProvider, NavigationManager nav)
        {
            _http = http;
            _localStorage = localStorage;
            _serviceProvider = serviceProvider;
            _nav = nav;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var res = await _http.PostAsJsonAsync("users/login", new { email, password });
            if (!res.IsSuccessStatusCode)
                return false;

            var response = await res.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            if (response != null && response.TryGetValue("token", out var token))
            {
                await _localStorage.SetItemAsync("token", token);
            }
            else
            {
                Console.WriteLine("❌ Nie udało się odczytać tokena z odpowiedzi API");
                return false;
            }


            var authProvider = _serviceProvider.GetRequiredService<CustomAuthStateProvider>();
            authProvider.NotifyUserAuthentication();

            _nav.NavigateTo(_nav.Uri, forceLoad: false);

            return true;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("token");

            var authProvider = _serviceProvider.GetRequiredService<CustomAuthStateProvider>();
            authProvider.NotifyUserLogout();

            // ✅ Soft refresh Blazora (bez F5)
            _nav.NavigateTo("/", replace: true);
        }


        public async Task<string?> GetTokenAsync() => await _localStorage.GetItemAsync("token");
    }
}
