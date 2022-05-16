using Siniauski.WhatIWant.WebData.Contracts.Requests;
using Siniauski.WhatIWant.WebData.Contracts.Responses;

namespace Siniauski.WhatIWant.WebApp.Interfaces
{
    public interface IFriendService
    {
        Task<UsersInfoResponse> MyAsync(string userId);

        Task<UsersInfoResponse> SearchAsync(string userId);

        Task<UsersInfoResponse> OutgoingAsync(string userId);

        Task<UsersInfoResponse> IncomingAsync(string userId);

        Task<Response> CreateAsync(FriendRequest friendRequest);

        Task<Response> DeleteAsync(FriendRequest request);
    }
}