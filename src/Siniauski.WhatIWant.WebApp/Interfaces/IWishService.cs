using Siniauski.WhatIWant.WebData.Contracts.Requests;
using Siniauski.WhatIWant.WebData.Contracts.Responses;

namespace Siniauski.WhatIWant.WebApp.Interfaces
{
    public interface IWishService
    {
        Task<WishesInfoResponse> MyAsync(string userId);

        Task<Response> CreateAsync(WishCreateRequest wishCreateRequest);

        Task<WishesInfoResponse> FriendsAsync(string userId);

        Task<WishesInfoResponse> UnfulfilledAsync(string userId);

        Task<Response> DeleteAsync(WishDeleteRequest wishDeleteRequest);

        Task<Response> BlockAsync(WishActionRequest wishBlockRequest);

        Task<Response> UnblockAsync(WishActionRequest wishBlockRequest);

        Task<Response> SetAsDoneAsync(WishActionRequest wishBlockRequest);
    }
}