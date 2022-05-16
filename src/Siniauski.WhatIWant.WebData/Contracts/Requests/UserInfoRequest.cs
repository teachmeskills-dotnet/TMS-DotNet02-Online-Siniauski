using System.ComponentModel.DataAnnotations;

namespace Siniauski.WhatIWant.WebData.Contracts.Requests
{
    public class UserInfoRequest
    {
        [Required]
        public string? UserId { get; set; }
    }
}