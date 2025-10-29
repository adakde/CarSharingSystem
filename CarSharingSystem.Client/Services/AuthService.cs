using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CarSharingSystem.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly LocalStorage _localStorage;
        public AuthService(HttpClient http, LocalStorage localStorage)
        {
            _http = http;
            _localStorage = localStorage;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var res = await _http.PostAsJsonAsync("users/login", new { email, password });
            if (!res.IsSuccessStatusCode)
                return false;

            var json = await res.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var token = doc.RootElement.GetProperty("token").GetString();

            if (string.IsNullOrEmpty(token)) return false;

            await _localStorage.SetItemAsync("token", token);
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return true;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("token");
            _http.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync("token");
        }
    }
}
