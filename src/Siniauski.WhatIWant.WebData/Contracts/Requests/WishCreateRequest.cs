using System.ComponentModel.DataAnnotations;

namespace Siniauski.WhatIWant.WebData.Contracts.Requests
{
    public class WishCreateRequest
    {
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Введите название!")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Введите описание!")]
        public string? Description { get; set; }
        public bool IsDone { get; set; }
    }
}