using Microsoft.EntityFrameworkCore;
using Siniauski.WhatIWant.Data.Models;
using Siniauski.WhatIWant.Logic.Interfaces;
using Siniauski.WhatIWant.Logic.ModelsDto;

namespace Siniauski.WhatIWant.Logic.Managers
{
    ///<inheritdoc cref="IWishManager"/>
    public class WishManager : IWishManager
    {
        private readonly IRepositoryManager<Wish> _wishRepository;
        private readonly IRepositoryManager<WishInfo> _wishInfoRepository;
        private readonly IRepositoryManager<Friend> _friendRepository;

        public WishManager(IRepositoryManager<Wish> wishRepository, IRepositoryManager<WishInfo> wishInfoRepository, IRepositoryManager<Friend> friendRepository)
        {
            _wishRepository = wishRepository ?? throw new ArgumentNullException(nameof(wishRepository));
            _wishInfoRepository = wishInfoRepository ?? throw new ArgumentNullException(nameof(wishInfoRepository));
            _friendRepository = friendRepository ?? throw new ArgumentNullException(nameof(friendRepository));
        }

        public async Task CreateAsync(WishDto model)
        {
            model = model ?? throw new Exception($"Входные данные не могут быть null.");
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new Exception($"Значение '{nameof(model.Name)}' не может быть пустым или null.");
            }
            if (string.IsNullOrEmpty(model.UserId))
            {
                throw new Exception($"Значение '{nameof(model.UserId)}' не может быть пустым или null.");
            }
            var wish = new Wish
            {
                UserId = model.UserId,
                Name = model.Name,
                Description = model.Description,
                IsDone = model.IsDone,
            };
            await _wishRepository.CreateAsync(wish);
            await _wishRepository.SaveChangesAsync();
            var incInvUserIds = await _friendRepository.GetAll()
                                              .Where(f => f.SecondUserId == model.UserId)
                                              .Select(f => f.FirstUserId)
                                              .ToListAsync();
            foreach (var friendId in incInvUserIds)
            {
                if (!_wishInfoRepository.GetAll().Any(wi => wi.UserId == friendId && wi.WishId == wish.Id))
                {
                    await _wishInfoRepository.CreateAsync(new WishInfo() { UserId = friendId, WishId = wish.Id, IsBlocked = false, IsRead = false });
                }
            }
            await _wishInfoRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int wishId)
        {
            var wish = await _wishRepository.GetEntityAsync(w => w.Id == wishId);

            if (wish is null)
            {
                throw new Exception($"Желание не найдено.");
            }
            var wishInfos = _wishInfoRepository.GetAll().Where(wi => wi.WishId == wishId);
            _wishInfoRepository.DeleteRange(wishInfos);
            await _wishInfoRepository.SaveChangesAsync();
            _wishRepository.Delete(wish);
            await _wishRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<WishDto>> GetUserWishesAsync(string userId)
        {
            var wishes = await _wishRepository.GetAll()
                                              .Where(w => w.UserId == userId)
                                              .ToListAsync();

            if (!wishes.Any())
            {
                return new List<WishDto>();
            }

            var wishDtos = new List<WishDto>();
            foreach (var wish in wishes)
            {
                wishDtos.Add(new WishDto
                {
                    Id = wish.Id,
                    UserId = wish.UserId,
                    Name = wish.Name,
                    Description = wish.Description,
                    IsDone = wish.IsDone
                });
            }
            return wishDtos;
        }

        public async Task<IEnumerable<WishDto>> GetFriendsWishesAsync(IEnumerable<string> friendIds)
        {
            var wishes = await _wishRepository.GetAll()
                                              .Where(w => friendIds.Contains(w.UserId))
                                              .ToListAsync();
            if (!wishes.Any())
            {
                return new List<WishDto>();
            }

            var wishDtos = new List<WishDto>();
            foreach (var wish in wishes)
            {
                wishDtos.Add(new WishDto
                {
                    Id = wish.Id,
                    UserId = wish.UserId,
                    Name = wish.Name,
                    Description = wish.Description,
                    IsDone = wish.IsDone
                });
            }
            return wishDtos;
        }

        public async Task<IEnumerable<WishDto>> GetUnfulfilledFriendsWishesAsync(string userId)
        {
            var wishIds = await _wishInfoRepository.GetAll()
                                              .Where(wi => wi.UserId == userId && wi.IsBlocked)
                                              .Select(wi => wi.WishId)
                                              .ToListAsync();
            if (!wishIds.Any())
            {
                return new List<WishDto>();
            }
            var wishes = await _wishRepository.GetAll()
                                              .Where(w => wishIds.Contains(w.Id))
                                              .ToListAsync();
            if (!wishes.Any())
            {
                return new List<WishDto>();
            }

            var wishDtos = new List<WishDto>();
            foreach (var wish in wishes)
            {
                wishDtos.Add(new WishDto
                {
                    Id = wish.Id,
                    UserId = wish.UserId,
                    Name = wish.Name,
                    Description = wish.Description,
                    IsDone = wish.IsDone
                });
            }
            return wishDtos;
        }

        public string? GetWhoBlockedUserId(int wishId)
        {
            return _wishInfoRepository.GetAll().Where(wi => wi.WishId == wishId && wi.IsBlocked).Select(wi => wi.UserId).FirstOrDefault();
        }

        public async Task BlockAsync(string userId, int wishId)
        {
            if (_wishInfoRepository.GetAll().Any(wi => wi.WishId == wishId && wi.IsBlocked))
            {
                throw new Exception("Желание уже заблокировано!");
            }
            WishInfo wishInfo = _wishInfoRepository.GetAll().Where(wi => wi.WishId == wishId && wi.UserId == userId).FirstOrDefault();
            if (wishInfo == null)
            {
                throw new Exception("Информация о желании для пользователя не найдена!");
            }
            wishInfo.IsBlocked = true;
            await _wishInfoRepository.SaveChangesAsync();
        }

        public async Task UnblockAsync(string userId, int wishId)
        {
            WishInfo wishInfo = _wishInfoRepository.GetAll().Where(wi => wi.WishId == wishId && wi.UserId == userId).FirstOrDefault();
            if (wishInfo == null)
            {
                throw new Exception("Информация о желании для пользователя не найдена!");
            }
            wishInfo.IsBlocked = false;
            await _wishInfoRepository.SaveChangesAsync();
        }

        public async Task SetAsDoneAsync(string userId, int wishId)
        {
            Wish wish = _wishRepository.GetAll().Where(w => w.Id == wishId && w.UserId == userId).FirstOrDefault();
            if (wish == null)
            {
                throw new Exception("Желание не найдено!");
            }
            wish.IsDone = true;
            await _wishRepository.SaveChangesAsync();
            var wishInfos = _wishInfoRepository.GetAll().Where(wi => wi.WishId == wishId && wi.IsBlocked);
            foreach (var wishInfo in wishInfos)
            {
                wishInfo.IsBlocked = false;
            }
            await _wishInfoRepository.SaveChangesAsync();
        }
    }
}