namespace Siniauski.WhatIWant.Data.Models
{
    public class Wish
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public virtual User? User { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsDone { get; set; }

        public virtual ICollection<WishInfo>? WishesInfo { get; set; }
    }
}