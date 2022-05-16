using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Siniauski.WhatIWant.Logic.Interfaces;
using Siniauski.WhatIWant.WebData.Contracts.Requests;
using Siniauski.WhatIWant.WebData.Contracts.Responses;
using Siniauski.WhatIWant.WebData.Enums;
using Siniauski.WhatIWant.WebData.Models;

namespace Siniauski.WhatIWant.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendController : ControllerBase
    {
        private readonly IFriendManager _friendManager;

        public FriendController(IFriendManager friendManager)
        {
            _friendManager = friendManager;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] FriendRequest request)
        {
            try
            {
                Response response = await _friendManager.CreateAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response() { Status = ResponseStatus.Failure, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromBody] FriendRequest request)
        {
            try
            {
                Response response = await _friendManager.DeleteAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response() { Status = ResponseStatus.Failure, Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("my")]
        public async Task<IActionResult> GetFriends([FromBody] UserInfoRequest request)
        {
            try
            {
                var users = await _friendManager.GetFriendsByUserIdAsync(request.UserId);
                UsersInfoResponse response = new() { Status = ResponseStatus.Success, Message = "Запрос выполнен успешно!", Users = users.ToList() };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new UsersInfoResponse() { Status = ResponseStatus.Failure, Message = ex.Message, Users = new List<FriendModel>() });
            }
        }

        [HttpGet]
        [Route("search")]
        public IActionResult GetAllUsers([FromBody] UserInfoRequest request)
        {
            try
            {
                var users = _friendManager.GetAllUsersAsync(request.UserId).Result;
                UsersInfoResponse response = new() { Status = ResponseStatus.Success, Message = "Запрос выполнен успешно!", Users = users.ToList() };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new UsersInfoResponse() { Status = ResponseStatus.Failure, Message = ex.Message, Users = new List<FriendModel>() });
            }
        }

        [HttpGet]
        [Route("outgoing")]
        public async Task<IActionResult> GetOutgoingInvitations([FromBody] UserInfoRequest request)
        {
            try
            {
                var users = await _friendManager.GetOutgoingInvitationsByUserIdAsync(request.UserId);
                UsersInfoResponse response = new() { Status = ResponseStatus.Success, Message = "Запрос выполнен успешно!", Users = users.ToList() };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new UsersInfoResponse() { Status = ResponseStatus.Failure, Message = ex.Message, Users = new List<FriendModel>() });
            }
        }

        [HttpGet]
        [Route("incoming")]
        public async Task<IActionResult> GetIncomingInvitations([FromBody] UserInfoRequest request)
        {
            try
            {
                var users = await _friendManager.GetIncomingInvitationsByUserIdAsync(request.UserId);
                UsersInfoResponse response = new() { Status = ResponseStatus.Success, Message = "Запрос выполнен успешно!", Users = users.ToList() };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new UsersInfoResponse() { Status = ResponseStatus.Failure, Message = ex.Message, Users = new List<FriendModel>() });
            }
        }
    }
}