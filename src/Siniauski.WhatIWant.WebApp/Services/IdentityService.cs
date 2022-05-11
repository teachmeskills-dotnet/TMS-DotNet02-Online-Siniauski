using Newtonsoft.Json;
using Siniauski.WhatIWant.WebApp.Interfaces;
using Siniauski.WhatIWant.WebData.Models;
using System.Text;

namespace Siniauski.WhatIWant.WebApp.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;

        public IdentityService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<UserAuthModel> LoginAsync(object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/account/login")
            {
                Content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json")
            };
            using var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception("Вход не выполнен! Неверный логин и/или пароль!");
                else
                    throw new Exception("Вход не выполнен! Неизвестная ошибка!");
            }
            return await response.Content.ReadFromJsonAsync<UserAuthModel>();
        }

        public async Task<UserAuthModel> RegisterAsync(object value)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/account/register")
            {
                Content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json")
            };
            using var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Ошибка при регистрации пользователя!");
            }
            return await response.Content.ReadFromJsonAsync<UserAuthModel>();
        }
    }
}