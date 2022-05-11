using Siniauski.WhatIWant.Logic.ModelsDto;

namespace Siniauski.WhatIWant.Logic.Interfaces
{
    public interface IWishManager
    {
        Task CreateAsync(WishDto model);

        Task DeleteAsync(int wishId);

        Task<IEnumerable<WishDto>> GetUserWishesAsync(string userId);

        Task<IEnumerable<WishDto>> GetFriendsWishesAsync(IEnumerable<string> friendIds);

        Task<IEnumerable<WishDto>> GetUnfulfilledFriendsWishesAsync(string userId);

        string? GetWhoBlockedUserId(int wishId);

        Task BlockAsync(string userId, int wishId);

        Task UnblockAsync(string userId, int wishId);

        Task SetAsDoneAsync(string userId, int wishId);
    }
}