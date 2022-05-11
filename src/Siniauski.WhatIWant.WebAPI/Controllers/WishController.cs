using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Siniauski.WhatIWant.Data.Models;
using Siniauski.WhatIWant.Logic.Interfaces;
using Siniauski.WhatIWant.Logic.ModelsDto;
using Siniauski.WhatIWant.WebData.Contracts.Requests;
using Siniauski.WhatIWant.WebData.Contracts.Responses;
using Siniauski.WhatIWant.WebData.Enums;
using Siniauski.WhatIWant.WebData.Models;

namespace Siniauski.WhatIWant.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishController : ControllerBase
    {
        private readonly IWishManager _wishManager;
        private readonly UserManager<User> _userManager;
        private readonly IFriendManager _friendManager;

        public WishController(IWishManager wishManager, IFriendManager friendManager, UserManager<User> userManager)
        {
            _wishManager = wishManager;
            _friendManager = friendManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("my")]
        public async Task<IActionResult> GetUserWishes([FromBody] UserInfoRequest request)
        {
            try
            {
                WishesInfoResponse response = new() { Status = ResponseStatus.Success, Message = "Запрос выполнен успешно!", Wishes = new List<WishModel>() };
                var wishDtos = await _wishManager.GetUserWishesAsync(request.UserId);
                foreach (var wishDto in wishDtos)
                {
                    var whoBlockedId = _wishManager.GetWhoBlockedUserId(wishDto.Id);
                    FriendModel whoBlocked = null;
                    if (whoBlockedId != null)
                    {
                        User whoBlockedUser = await _userManager.FindByIdAsync(whoBlockedId);
                        whoBlocked = new FriendModel()
                        {
                            Id = whoBlockedUser.Id,
                            FirstName = whoBlockedUser.FirstName,
                            LastName = whoBlockedUser.LastName,
                            BirthDate = whoBlockedUser.BirthDate,
                            Email = whoBlockedUser.Email,
                            PhoneNumber = whoBlockedUser.PhoneNumber,
                            UserName = whoBlockedUser.UserName,
                            Avatar = whoBlockedUser.Avatar,
                        };
                    }
                    response.Wishes.Add(new WishModel()
                    {
                        Id = wishDto.Id,
                        Name = wishDto.Name,
                        WhoCreate = null,
                        Description = wishDto.Description,
                        IsDone = wishDto.IsDone,
                        WhoBlocked = null,
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new WishesInfoResponse() { Status = ResponseStatus.Failure, Message = ex.Message, Wishes = new List<WishModel>() });
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAsync([FromBody] WishDto request)
        {
            try
            {
                await _wishManager.CreateAsync(request);
                return Ok(new Response() { Status = ResponseStatus.Success, Message = "Желание успешно создано!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response() { Status = ResponseStatus.Success, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] WishDeleteRequest request)
        {
            try
            {
                await _wishManager.DeleteAsync(request.WishId);
                return Ok(new Response() { Status = ResponseStatus.Success, Message = "Желание успешно удалено!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response() { Status = ResponseStatus.Success, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("block")]
        public async Task<IActionResult> BlockAsync([FromBody] WishActionRequest request)
        {
            try
            {
                await _wishManager.BlockAsync(request.UserId, request.WishId);
                return Ok(new Response() { Status = ResponseStatus.Success, Message = "Желание успешно заблокировано!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response() { Status = ResponseStatus.Failure, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("unblock")]
        public async Task<IActionResult> UnblockAsync([FromBody] WishActionRequest request)
        {
            try
            {
                await _wishManager.UnblockAsync(request.UserId, request.WishId);
                return Ok(new Response() { Status = ResponseStatus.Success, Message = "Желание успешно разблокировано!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response() { Status = ResponseStatus.Failure, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("done")]
        public async Task<IActionResult> SetIsDoneAsync([FromBody] WishActionRequest request)
        {
            try
            {
                await _wishManager.SetAsDoneAsync(request.UserId, request.WishId);
                return Ok(new Response() { Status = ResponseStatus.Success, Message = "Желание помечено как исполненное!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response() { Status = ResponseStatus.Failure, Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("friends")]
        public async Task<IActionResult> GetFriendsWishes([FromBody] UserInfoRequest request)
        {
            try
            {
                WishesInfoResponse response = new() { Status = ResponseStatus.Success, Message = "Запрос выполнен успешно!", Wishes = new List<WishModel>() };
                var friendIds = (await _friendManager.GetFriendsByUserIdAsync(request.UserId)).Select(f => f.Id);
                var wishDtos = await _wishManager.GetFriendsWishesAsync(friendIds);

                foreach (var wishDto in wishDtos)
                {
                    var whoCreateUser = await _userManager.FindByIdAsync(wishDto.UserId);
                    FriendModel whoCreate = new()
                    {
                        Id = whoCreateUser.Id,
                        FirstName = whoCreateUser.FirstName,
                        LastName = whoCreateUser.LastName,
                        BirthDate = whoCreateUser.BirthDate,
                        Email = whoCreateUser.Email,
                        PhoneNumber = whoCreateUser.PhoneNumber,
                        UserName = whoCreateUser.UserName,
                        Avatar = whoCreateUser.Avatar,
                    };
                    var whoBlockedId = _wishManager.GetWhoBlockedUserId(wishDto.Id);
                    FriendModel? whoBlocked = null;
                    if (whoBlockedId != null)
                    {
                        User whoBlockedUser = await _userManager.FindByIdAsync(whoBlockedId);
                        whoBlocked = new FriendModel()
                        {
                            Id = whoBlockedUser.Id,
                            FirstName = whoBlockedUser.FirstName,
                            LastName = whoBlockedUser.LastName,
                            BirthDate = whoBlockedUser.BirthDate,
                            Email = whoBlockedUser.Email,
                            PhoneNumber = whoBlockedUser.PhoneNumber,
                            UserName = whoBlockedUser.UserName,
                            Avatar = whoBlockedUser.Avatar,
                        };
                    }
                    response.Wishes.Add(new WishModel()
                    {
                        Id = wishDto.Id,
                        Name = wishDto.Name,
                        WhoCreate = whoCreate,
                        Description = wishDto.Description,
                        IsDone = wishDto.IsDone,
                        WhoBlocked = whoBlocked,
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new WishesInfoResponse() { Status = ResponseStatus.Failure, Message = ex.Message, Wishes = new List<WishModel>() });
            }
        }

        [HttpGet]
        [Route("unfulfilled")]
        public async Task<IActionResult> GetUnfulfilledWishes([FromBody] UserInfoRequest request)
        {
            try
            {
                WishesInfoResponse response = new() { Status = ResponseStatus.Success, Message = "Запрос выполнен успешно!", Wishes = new List<WishModel>() };
                var wishDtos = await _wishManager.GetUnfulfilledFriendsWishesAsync(request.UserId);
                foreach (var wishDto in wishDtos)
                {
                    var whoCreateUser = await _userManager.FindByIdAsync(wishDto.UserId);
                    FriendModel whoCreate = new()
                    {
                        Id = whoCreateUser.Id,
                        FirstName = whoCreateUser.FirstName,
                        LastName = whoCreateUser.LastName,
                        BirthDate = whoCreateUser.BirthDate,
                        Email = whoCreateUser.Email,
                        PhoneNumber = whoCreateUser.PhoneNumber,
                        UserName = whoCreateUser.UserName,
                        Avatar = whoCreateUser.Avatar,
                    };

                    response.Wishes.Add(new WishModel()
                    {
                        Id = wishDto.Id,
                        Name = wishDto.Name,
                        WhoCreate = whoCreate,
                        Description = wishDto.Description,
                        IsDone = wishDto.IsDone,
                        WhoBlocked = null,
                    });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new WishesInfoResponse() { Status = ResponseStatus.Failure, Message = ex.Message, Wishes = new List<WishModel>() });
            }
        }
    }
}