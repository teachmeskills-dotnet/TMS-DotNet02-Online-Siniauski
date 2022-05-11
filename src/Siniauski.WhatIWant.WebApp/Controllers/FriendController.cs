using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Siniauski.WhatIWant.WebApp.Attributes;
using Siniauski.WhatIWant.WebApp.Interfaces;
using Siniauski.WhatIWant.WebApp.ViewModels;
using Siniauski.WhatIWant.WebData.Contracts.Requests;
using Siniauski.WhatIWant.WebData.Contracts.Responses;
using Siniauski.WhatIWant.WebData.Enums;
using Siniauski.WhatIWant.WebData.Models;
using System.Security.Claims;

namespace Siniauski.WhatIWant.WebApp.Controllers
{
    [Authorize]
    [NoDirectAccess]
    public class FriendController : Controller
    {
        private readonly IFriendService _friendService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FriendController(IFriendService friendService, IHttpContextAccessor httpContextAccessor)
        {
            _friendService = friendService ?? throw new ArgumentNullException(nameof(friendService));
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("Friend/")]
        public async Task<IActionResult> MyAsync()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated || userId == null)
                {
                    throw new Exception("Ошибка аутентификации!");
                }
                UsersInfoResponse response = await _friendService.MyAsync(userId);
                return PartialView("UserList", new UserListViewModel() { UsersInfoResponse = response });
            }
            catch (Exception ex)
            {
                return PartialView("UserList", new UserListViewModel()
                {
                    UsersInfoResponse = new() { Status = ResponseStatus.Failure, Message = ex.Message, Users = new List<FriendModel>() }
                });
            }
        }

        [HttpGet("Friend/Search")]
        public async Task<IActionResult> SearchAsync()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated || userId == null)
                {
                    throw new Exception("Ошибка аутентификации!");
                }
                UsersInfoResponse response = await _friendService.SearchAsync(userId);
                return PartialView("UserList", new UserListViewModel() { UsersInfoResponse = response });
            }
            catch (Exception ex)
            {
                return PartialView("UserList", new UserListViewModel()
                {
                    UsersInfoResponse = new() { Status = ResponseStatus.Failure, Message = ex.Message, Users = new List<FriendModel>() }
                });
            }
        }

        [HttpGet("Friend/Incoming")]
        public async Task<object> IncomingAsync()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated || userId == null)
                {
                    throw new Exception("Ошибка аутентификации!");
                }
                UsersInfoResponse response = await _friendService.IncomingAsync(userId);
                return PartialView("UserList", new UserListViewModel() { UsersInfoResponse = response });
            }
            catch (Exception ex)
            {
                return PartialView("UserList", new UserListViewModel()
                {
                    UsersInfoResponse = new() { Status = ResponseStatus.Failure, Message = ex.Message, Users = new List<FriendModel>() }
                });
            }
        }

        [HttpGet("Friend/Outgoing")]
        public async Task<object> OutgoingAsync()
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated || userId == null)
                {
                    throw new Exception("Ошибка аутентификации!");
                }
                UsersInfoResponse response = await _friendService.OutgoingAsync(userId);
                return PartialView("UserList", new UserListViewModel() { UsersInfoResponse = response });
            }
            catch (Exception ex)
            {
                return PartialView("UserList", new UserListViewModel()
                {
                    UsersInfoResponse = new() { Status = ResponseStatus.Failure, Message = ex.Message, Users = new List<FriendModel>() }
                });
            }
        }

        [HttpPost("Friend/Create")]
        public async Task<object> CreateAsync([FromBody] FriendRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated || userId == null)
                    {
                        throw new Exception("Ошибка аутентификации!");
                    }
                    request.FirstUserId = userId;
                    return await _friendService.CreateAsync(request);
                }
                else
                {
                    throw new Exception("Запрос не прошел валидацию!");
                }
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    Status = ResponseStatus.Failure,
                    Message = ex.Message
                };
            }
        }

        [HttpPost("Friend/Delete")]
        public async Task<object> DeleteAsync([FromBody] FriendRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated || userId == null)
                    {
                        throw new Exception("Ошибка аутентификации!");
                    }
                    request.FirstUserId = userId;
                    return await _friendService.DeleteAsync(request);
                }
                else
                {
                    throw new Exception("Запрос не прошел валидацию!");
                }
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    Status = ResponseStatus.Failure,
                    Message = ex.Message
                };
            }
        }
    }
}