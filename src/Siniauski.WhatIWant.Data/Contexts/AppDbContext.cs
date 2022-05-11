using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Siniauski.WhatIWant.Data.Configurations;
using Siniauski.WhatIWant.Data.Models;

namespace Siniauski.WhatIWant.Data.Contexts
{
    public class AppDbContext : IdentityDbContext<User>
    {
        /// <summary>
        /// DbSet for Wish.
        /// </summary>
        public DbSet<Wish>? Wishes { get; set; }

        /// <summary>
        /// DbSet for Friends.
        /// </summary>
        public DbSet<Friend>? Friends { get; set; }

        /// <summary>
        /// DbSet for WishInfo.
        /// </summary>
        public DbSet<WishInfo>? WishesInfo { get; set; }

        public AppDbContext() : base()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new WishConfiguration());
            builder.ApplyConfiguration(new WishInfoConfiguration());
            builder.ApplyConfiguration(new FriendConfiguration());
            base.OnModelCreating(builder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WhatIWantDB;Trusted_Connection=True;");
        //}
    }
}