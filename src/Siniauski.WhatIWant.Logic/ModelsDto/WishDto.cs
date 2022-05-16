namespace Siniauski.WhatIWant.Logic.ModelsDto
{
    public class WishDto
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsDone { get; set; }
    }
}