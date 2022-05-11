using System.ComponentModel.DataAnnotations;

namespace Siniauski.WhatIWant.WebData.Contracts.Requests
{
    public class WishDeleteRequest
    {
        [Required]
        public int WishId { get; set; }
    }
}