using System.ComponentModel.DataAnnotations;

namespace Siniauski.WhatIWant.WebData.Contracts.Requests
{
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "Введите логин!")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        public string? Password { get; set; }
    }
}