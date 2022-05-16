using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Siniauski.WhatIWant.WebApp.Attributes;
using Siniauski.WhatIWant.WebApp.Interfaces;
using Siniauski.WhatIWant.WebApp.ViewModels;
using Siniauski.WhatIWant.WebData.Contracts.Requests;
using Siniauski.WhatIWant.WebData.Contracts.Responses;
using Siniauski.WhatIWant.WebData.Enums;
using System.Security.Claims;

namespace Siniauski.WhatIWant.WebApp.Controllers
{
    [Authorize]
    [NoDirectAccess]
    public class WishController : Controller
    {
        private readonly IWishService _wishService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WishController(IWishService wishService, IHttpContextAccessor httpContextAccessor)
        {
            _wishService = wishService ?? throw new ArgumentNullException(nameof(wishService));
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("Wishes/My")]
        public async Task<IActionResult> MyAsync()
        {
            string userId = string.Empty;
            WishesInfoResponse response = new();
            if (ModelState.IsValid)
            {
                userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && userId != null)
                {
                    response = await _wishService.MyAsync(userId);
                }
            }
            return PartialView("WishList", new WishListViewModel() { MyId = userId, WishesInfoType = "My", WishesInfoResponse = response });
        }

        [HttpGet("Wishes/Friends")]
        public async Task<IActionResult> FriendsAsync()
        {
            string userId = string.Empty;
            WishesInfoResponse response = new();
            if (ModelState.IsValid)
            {
                userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && userId != null)
                {
                    response = await _wishService.FriendsAsync(userId);
                }
            }
            return PartialView("WishList", new WishListViewModel() { MyId = userId, WishesInfoType = "Friends", WishesInfoResponse = response });
        }

        [HttpGet("Wishes/Unfulfilled")]
        public async Task<IActionResult> UnfulfilledAsync()
        {
            string userId = string.Empty;
            WishesInfoResponse response = new();
            if (ModelState.IsValid)
            {
                userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && userId != null)
                {
                    response = await _wishService.UnfulfilledAsync(userId);
                }
            }
            return PartialView("WishList", new WishListViewModel() { MyId = userId, WishesInfoType = "Unfulfilled", WishesInfoResponse = response });
        }

        [HttpGet("Wishes/Create")]
        public IActionResult Create()
        {
            return PartialView("WishCreate");
        }

        [HttpPost("Wishes/Create")]
        public async Task<object> CreateAsync([FromBody] WishCreateRequest wishCreateRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && userId != null)
                    {
                        wishCreateRequest.UserId = userId;
                        return await _wishService.CreateAsync(wishCreateRequest);
                    }
                    else
                    {
                        throw new Exception("Ошибка аутентификации!");
                    }
                }
                else
                {
                    string message = "";
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            message += error.ErrorMessage + "\n";
                        }
                    }
                    message = message.TrimEnd('\n');
                    throw new Exception(message);
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

        [HttpPost("Wishes/Block")]
        public async Task<object> BlockAsync([FromBody] WishActionRequest wishBlockRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && userId != null)
                    {
                        wishBlockRequest.UserId = userId;
                        return await _wishService.BlockAsync(wishBlockRequest);
                    }
                    else
                    {
                        throw new Exception("Ошибка аутентификации!");
                    }
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

        [HttpPost("Wishes/Unblock")]
        public async Task<object> UnblockAsync([FromBody] WishActionRequest wishBlockRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && userId != null)
                    {
                        wishBlockRequest.UserId = userId;
                        return await _wishService.UnblockAsync(wishBlockRequest);
                    }
                    else
                    {
                        throw new Exception("Ошибка аутентификации!");
                    }
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

        [HttpPost("Wishes/Done")]
        public async Task<object> SetAsDoneAsync([FromBody] WishActionRequest wishBlockRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && userId != null)
                    {
                        wishBlockRequest.UserId = userId;
                        return await _wishService.SetAsDoneAsync(wishBlockRequest);
                    }
                    else
                    {
                        throw new Exception("Ошибка аутентификации!");
                    }
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

        [HttpPost("Wishes/Delete")]
        public async Task<object> DeleteAsync([FromBody] WishDeleteRequest wishDeleteRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && userId != null)
                    {
                        return await _wishService.DeleteAsync(wishDeleteRequest);
                    }
                    else
                    {
                        throw new Exception("Ошибка аутентификации!");
                    }
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