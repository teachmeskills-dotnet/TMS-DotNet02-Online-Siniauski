using Newtonsoft.Json;
using Siniauski.WhatIWant.WebApp.Interfaces;
using Siniauski.WhatIWant.WebData.Contracts.Requests;
using Siniauski.WhatIWant.WebData.Contracts.Responses;
using Siniauski.WhatIWant.WebData.Enums;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Siniauski.WhatIWant.WebApp.Services
{
    public class FriendService : IFriendService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FriendService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpContextAccessor = httpContextAccessor;
            var token = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<UsersInfoResponse> MyAsync(string userId)
        {
            UserInfoRequest userInfoRequest = new() { UserId = userId };
            var request = new HttpRequestMessage(HttpMethod.Get, "api/friend/my")
            {
                Content = new StringContent(JsonConvert.SerializeObject(userInfoRequest), Encoding.UTF8, "application/json")
            };
            using var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<UsersInfoResponse>();
        }

        public async Task<UsersInfoResponse> SearchAsync(string userId)
        {
            UserInfoRequest userInfoRequest = new() { UserId = userId };
            var request = new HttpRequestMessage(HttpMethod.Get, "api/friend/search")
            {
                Content = new StringContent(JsonConvert.SerializeObject(userInfoRequest), Encoding.UTF8, "application/json")
            };
            using var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<UsersInfoResponse>();
        }

        public async Task<UsersInfoResponse> IncomingAsync(string userId)
        {
            UserInfoRequest userInfoRequest = new() { UserId = userId };
            var request = new HttpRequestMessage(HttpMethod.Get, "api/friend/incoming")
            {
                Content = new StringContent(JsonConvert.SerializeObject(userInfoRequest), Encoding.UTF8, "application/json")
            };
            using var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<UsersInfoResponse>();
        }

        public async Task<UsersInfoResponse> OutgoingAsync(string userId)
        {
            UserInfoRequest userInfoRequest = new() { UserId = userId };
            var request = new HttpRequestMessage(HttpMethod.Get, "api/friend/outgoing")
            {
                Content = new StringContent(JsonConvert.SerializeObject(userInfoRequest), Encoding.UTF8, "application/json")
            };
            using var response = await _httpClient.SendAsync(request);
            return await response.Content.ReadFromJsonAsync<UsersInfoResponse>();
        }

        public async Task<Response> CreateAsync(FriendRequest request)
        {
            var apiRequest = new HttpRequestMessage(HttpMethod.Post, "api/friend/create")
            {
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            };
            var apiResponse = await _httpClient.SendAsync(apiRequest);
            return await apiResponse.Content.ReadFromJsonAsync<Response>();
        }

        public async Task<Response> DeleteAsync(FriendRequest request)
        {
            var apiRequest = new HttpRequestMessage(HttpMethod.Post, "api/friend/delete")
            {
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            };
            var apiResponse = await _httpClient.SendAsync(apiRequest);
            return await apiResponse.Content.ReadFromJsonAsync<Response>();
        }
    }
}