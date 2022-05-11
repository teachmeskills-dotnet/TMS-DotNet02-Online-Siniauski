namespace Siniauski.WhatIWant.Data.Models
{
    public class WishInfo
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public virtual User? User { get; set; }
        public int? WishId { get; set; }
        public virtual Wish? Wish { get; set; }
        public bool IsRead { get; set; }
        public bool IsBlocked { get; set; }
    }
}