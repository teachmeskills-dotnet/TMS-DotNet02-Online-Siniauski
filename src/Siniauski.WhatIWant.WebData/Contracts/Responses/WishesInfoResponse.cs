using Siniauski.WhatIWant.WebData.Models;

namespace Siniauski.WhatIWant.WebData.Contracts.Responses
{
    public class WishesInfoResponse : Response
    {
        public List<WishModel>? Wishes { get; set; }
    }
}