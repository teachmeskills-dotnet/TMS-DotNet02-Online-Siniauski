using Newtonsoft.Json;
using Siniauski.WhatIWant.WebApp.Interfaces;
using Siniauski.WhatIWant.WebData.Contracts.Requests;
using Siniauski.WhatIWant.WebData.Contracts.Responses;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Siniauski.WhatIWant.WebApp.Services
{
    public class WishService : IWishService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WishService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpContextAccessor = httpContextAccessor;
            var token = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<WishesInfoResponse> MyAsync(string userId)
        {
            UserInfoRequest userInfoRequest = new() { UserId = userId };
            var request = new HttpRequestMessage(HttpMethod.Get, "api/wish/my")
            {
                Content = new StringContent(JsonConvert.SerializeObject(userInfoRequest), Encoding.UTF8, "application/json")
            };
            using var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<WishesInfoResponse>();
        }

        public async Task<Response> CreateAsync(WishCreateRequest wishCreateRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/wish/create")
            {
                Content = new StringContent(JsonConvert.SerializeObject(wishCreateRequest), Encoding.UTF8, "application/json")
            };
            using var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<Response>();
        }

        public async Task<WishesInfoResponse> FriendsAsync(string userId)
        {
            UserInfoRequest userInfoRequest = new() { UserId = userId };
            var request = new HttpRequestMessage(HttpMethod.Get, "api/wish/friends")
            {
                Content = new StringContent(JsonConvert.SerializeObject(userInfoRequest), Encoding.UTF8, "application/json")
            };
            using var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<WishesInfoResponse>();
        }

        public async Task<WishesInfoResponse> UnfulfilledAsync(string userId)
        {
            UserInfoRequest userInfoRequest = new() { UserId = userId };
            var request = new HttpRequestMessage(HttpMethod.Get, "api/wish/unfulfilled")
            {
                Content = new StringContent(JsonConvert.SerializeObject(userInfoRequest), Encoding.UTF8, "application/json")
            };
            using var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<WishesInfoResponse>();
        }

        public async Task<Response> DeleteAsync(WishDeleteRequest wishDeleteRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/wish/delete")
            {
                Content = new StringContent(JsonConvert.SerializeObject(wishDeleteRequest), Encoding.UTF8, "application/json")
            };

            using var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<Response>();
        }

        public async Task<Response> BlockAsync(WishActionRequest wishBlockRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/wish/block")
            {
                Content = new StringContent(JsonConvert.SerializeObject(wishBlockRequest), Encoding.UTF8, "application/json")
            };
            using var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<Response>();
        }

        public async Task<Response> UnblockAsync(WishActionRequest wishBlockRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/wish/unblock")
            {
                Content = new StringContent(JsonConvert.SerializeObject(wishBlockRequest), Encoding.UTF8, "application/json")
            };
            using var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<Response>();
        }

        public async Task<Response> SetAsDoneAsync(WishActionRequest wishBlockRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "api/wish/done")
            {
                Content = new StringContent(JsonConvert.SerializeObject(wishBlockRequest), Encoding.UTF8, "application/json")
            };
            using var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<Response>();
        }
    }
}