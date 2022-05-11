using Microsoft.AspNetCore.Identity;

namespace Siniauski.WhatIWant.Data.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Avatar { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Status { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Wish>? Wishes { get; set; }

        public virtual ICollection<WishInfo>? WishesInfo { get; set; }

        public virtual ICollection<Friend>? Friends { get; set; }
    }
}