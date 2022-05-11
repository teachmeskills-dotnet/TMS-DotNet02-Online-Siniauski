using System.ComponentModel.DataAnnotations;

namespace Siniauski.WhatIWant.WebData.Contracts.Requests
{
    public class WishActionRequest
    {
        [Required]
        public string? UserId { get; set; }

        [Required]
        public int WishId { get; set; }
    }
}