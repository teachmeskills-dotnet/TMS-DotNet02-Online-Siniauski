namespace Siniauski.WhatIWant.Data.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public string? FirstUserId { get; set; }
        public User? FirstUser { get; set; }
        public string? SecondUserId { get; set; }
        public User? SecondUser { get; set; }
    }
}