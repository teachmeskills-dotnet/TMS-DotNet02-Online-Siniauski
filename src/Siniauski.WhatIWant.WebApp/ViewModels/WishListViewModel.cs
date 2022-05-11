using Siniauski.WhatIWant.WebData.Contracts.Responses;
using System.ComponentModel.DataAnnotations;

namespace Siniauski.WhatIWant.WebApp.ViewModels
{
    public class WishListViewModel
    {
        public string? MyId { get; set; }

        public string? WishesInfoType { get; set; }

        public WishesInfoResponse? WishesInfoResponse { get; set; }
    }
}