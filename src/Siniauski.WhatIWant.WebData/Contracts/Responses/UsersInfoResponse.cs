using Siniauski.WhatIWant.WebData.Models;

namespace Siniauski.WhatIWant.WebData.Contracts.Responses
{
    public class UsersInfoResponse : Response
    {
        public List<FriendModel>? Users { get; set; }
    }
}