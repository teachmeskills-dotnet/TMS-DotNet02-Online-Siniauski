using System.ComponentModel.DataAnnotations;

namespace Siniauski.WhatIWant.WebData.Contracts.Requests
{
    public class FriendRequest
    {
        [Required]
        public string? FirstUserId { get; set; }

        [Required]
        public string? SecondUserId { get; set; }
    }
}