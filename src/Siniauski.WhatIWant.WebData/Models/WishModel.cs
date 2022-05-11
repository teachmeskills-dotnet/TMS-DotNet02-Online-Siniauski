namespace Siniauski.WhatIWant.WebData.Models
{
    public class WishModel
    {
        public int Id { get; set; }
        public FriendModel? WhoCreate { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsDone { get; set; }
        public FriendModel? WhoBlocked { get; set; }
    }
}