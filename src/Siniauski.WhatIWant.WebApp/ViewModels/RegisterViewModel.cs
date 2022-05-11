using System.ComponentModel.DataAnnotations;

namespace Siniauski.WhatIWant.WebApp.ViewModels
{
    public class RegisterViewModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Login { get; set; }

        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Avatar { get; set; }

        public string? AvatarServerName { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}