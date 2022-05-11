using Siniauski.WhatIWant.WebData.Contracts.Requests;
using Siniauski.WhatIWant.WebData.Contracts.Responses;
using Siniauski.WhatIWant.WebData.Models;

namespace Siniauski.WhatIWant.Logic.Interfaces
{
    public interface IFriendManager
    {
        Task<Response> CreateAsync(FriendRequest model);

        Task<Response> DeleteAsync(FriendRequest model);

        Task<IEnumerable<FriendModel>> GetAllUsersAsync(string userId);

        Task<IEnumerable<FriendModel>> GetFriendsByUserIdAsync(string userId);

        Task<IEnumerable<FriendModel>> GetOutgoingInvitationsByUserIdAsync(string userId);

        Task<IEnumerable<FriendModel>> GetIncomingInvitationsByUserIdAsync(string userId);
    }
}