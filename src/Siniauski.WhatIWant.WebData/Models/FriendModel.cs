namespace Siniauski.WhatIWant.WebData.Models
{
    public class FriendModel
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool HasOutgoingInvite { get; set; }
        public bool HasIncomingInvite { get; set; }
    }
}