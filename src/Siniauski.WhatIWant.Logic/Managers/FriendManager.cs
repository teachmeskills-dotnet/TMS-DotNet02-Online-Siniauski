using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Siniauski.WhatIWant.Data.Models;
using Siniauski.WhatIWant.Logic.Interfaces;
using Siniauski.WhatIWant.WebData.Contracts.Requests;
using Siniauski.WhatIWant.WebData.Contracts.Responses;
using Siniauski.WhatIWant.WebData.Enums;
using Siniauski.WhatIWant.WebData.Models;

namespace Siniauski.WhatIWant.Logic.Managers
{
    public class FriendManager : IFriendManager
    {
        private readonly IRepositoryManager<Friend> _friendRepository;
        private readonly UserManager<User> _userRepository;
        private readonly IRepositoryManager<Wish> _wishRepository;
        private readonly IRepositoryManager<WishInfo> _wishInfoRepository;

        public FriendManager(IRepositoryManager<Friend> friendRepository, UserManager<User> userRepository, IRepositoryManager<Wish> wishRepository, IRepositoryManager<WishInfo> wishInfoRepository)
        {
            _friendRepository = friendRepository ?? throw new ArgumentNullException(nameof(friendRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _wishRepository = wishRepository ?? throw new ArgumentNullException(nameof(wishRepository));
            _wishInfoRepository = wishInfoRepository ?? throw new ArgumentNullException(nameof(wishInfoRepository));
        }

        public async Task<Response> CreateAsync(FriendRequest request)
        {
            request = request ?? throw new Exception($"Входные данные не могут быть null.");
            if (string.IsNullOrEmpty(request.FirstUserId))
            {
                throw new Exception($"Значение '{nameof(request.FirstUserId)}' не может быть пустым или null.");
            }
            if (string.IsNullOrEmpty(request.SecondUserId))
            {
                throw new Exception($"Значение '{nameof(request.SecondUserId)}' не может быть пустым или null.");
            }
            var friend = new Friend
            {
                FirstUserId = request.FirstUserId,
                SecondUserId = request.SecondUserId
            };
            await _friendRepository.CreateAsync(friend);
            await _friendRepository.SaveChangesAsync();
            var wishIds = await _wishRepository.GetAll().Where(w => w.UserId == request.SecondUserId).Select(w => w.Id).ToListAsync();
            foreach (var wishId in wishIds)
            {
                if (!_wishInfoRepository.GetAll().Any(wi => wi.UserId == request.FirstUserId && wi.WishId == wishId))
                {
                    await _wishInfoRepository.CreateAsync(new WishInfo() { UserId = request.FirstUserId, WishId = wishId, IsBlocked = false, IsRead = false });
                }
            }
            await _wishInfoRepository.SaveChangesAsync();
            return new Response() { Status = ResponseStatus.Success, Message = "Запрос успешно выполнен!" };
        }

        public async Task<Response> DeleteAsync(FriendRequest request)
        {
            request = request ?? throw new Exception($"Входные данные не могут быть null.");
            if (string.IsNullOrEmpty(request.FirstUserId))
            {
                throw new Exception($"Значение '{nameof(request.FirstUserId)}' не может быть пустым или null.");
            }
            if (string.IsNullOrEmpty(request.SecondUserId))
            {
                throw new Exception($"Значение '{nameof(request.SecondUserId)}' не может быть пустым или null.");
            }
            var friend = await _friendRepository.GetEntityAsync(f => f.FirstUserId == request.FirstUserId && f.SecondUserId == request.SecondUserId);
            _friendRepository.Delete(friend);
            await _friendRepository.SaveChangesAsync();
            return new Response() { Status = ResponseStatus.Success, Message = "Запрос успешно выполнен!" };
        }

        public async Task<IEnumerable<FriendModel>> GetAllUsersAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception($"Входное значение не может быть пустым или null.");
            }
            var users = _userRepository.Users.Where(u => u.Id != userId);
            var outInvUserIds = await _friendRepository.GetAll()
                                              .Where(f => f.FirstUserId == userId)
                                              .Select(f => f.SecondUserId)
                                              .ToListAsync();
            var incInvUserIds = await _friendRepository.GetAll()
                                              .Where(f => f.SecondUserId == userId)
                                              .Select(f => f.FirstUserId)
                                              .ToListAsync();
            var userModels = new List<FriendModel>();
            foreach (var user in users)
            {
                userModels.Add(new FriendModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    BirthDate = user.BirthDate,
                    Avatar = user.Avatar,
                    HasOutgoingInvite = outInvUserIds.Contains(user.Id),
                    HasIncomingInvite = incInvUserIds.Contains(user.Id),
                });
            }
            return userModels;
        }

        public async Task<IEnumerable<FriendModel>> GetFriendsByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception($"Входное значение не может быть пустым или null.");
            }
            var outInvUserIds = await _friendRepository.GetAll()
                                              .Where(f => f.FirstUserId == userId)
                                              .Select(f => f.SecondUserId)
                                              .ToListAsync();
            var incInvUserIds = await _friendRepository.GetAll()
                                              .Where(f => f.SecondUserId == userId)
                                              .Select(f => f.FirstUserId)
                                              .ToListAsync();
            if (!outInvUserIds.Any() || !incInvUserIds.Any())
            {
                return new List<FriendModel>();
            }
            var friendsIds = outInvUserIds.Intersect(incInvUserIds);
            var userModels = new List<FriendModel>();
            foreach (var friendId in friendsIds)
            {
                var user = await _userRepository.FindByIdAsync(friendId);
                if (user != null)
                {
                    userModels.Add(new FriendModel()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        BirthDate = user.BirthDate,
                        Avatar = user.Avatar,
                        HasOutgoingInvite = outInvUserIds.Contains(user.Id),
                        HasIncomingInvite = incInvUserIds.Contains(user.Id),
                    });
                }
            }
            return userModels;
        }

        public async Task<IEnumerable<FriendModel>> GetOutgoingInvitationsByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception($"Входное значение не может быть пустым или null.");
            }
            var outInvUserIds = await _friendRepository.GetAll()
                                               .Where(f => f.FirstUserId == userId)
                                               .Select(f => f.SecondUserId)
                                               .ToListAsync();
            var incInvUserIds = await _friendRepository.GetAll()
                                              .Where(f => f.SecondUserId == userId)
                                              .Select(f => f.FirstUserId)
                                              .ToListAsync();
            if (!outInvUserIds.Any())
            {
                return new List<FriendModel>();
            }
            var userIds = outInvUserIds.Except(incInvUserIds);
            var userModels = new List<FriendModel>();
            foreach (var userOutInvId in userIds)
            {
                var user = await _userRepository.FindByIdAsync(userOutInvId);
                if (user != null)
                {
                    userModels.Add(new FriendModel()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        BirthDate = user.BirthDate,
                        Avatar = user.Avatar,
                        HasOutgoingInvite = outInvUserIds.Contains(user.Id),
                        HasIncomingInvite = incInvUserIds.Contains(user.Id),
                    });
                }
            }
            return userModels;
        }

        public async Task<IEnumerable<FriendModel>> GetIncomingInvitationsByUserIdAsync(string userId)
        {
            var outInvUserIds = await _friendRepository.GetAll()
                                               .Where(f => f.FirstUserId == userId)
                                               .Select(f => f.SecondUserId)
                                               .ToListAsync();
            var incInvUserIds = await _friendRepository.GetAll()
                                              .Where(f => f.SecondUserId == userId)
                                              .Select(f => f.FirstUserId)
                                              .ToListAsync();
            if (!incInvUserIds.Any())
            {
                return new List<FriendModel>();
            }
            var userIds = incInvUserIds.Except(outInvUserIds);
            var userModels = new List<FriendModel>();
            foreach (var userIncInvId in userIds)
            {
                var user = await _userRepository.FindByIdAsync(userIncInvId);
                if (user != null)
                {
                    userModels.Add(new FriendModel()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        BirthDate = user.BirthDate,
                        Avatar = user.Avatar,
                        HasOutgoingInvite = outInvUserIds.Contains(user.Id),
                        HasIncomingInvite = incInvUserIds.Contains(user.Id),
                    });
                }
            }
            return userModels;
        }
    }
}