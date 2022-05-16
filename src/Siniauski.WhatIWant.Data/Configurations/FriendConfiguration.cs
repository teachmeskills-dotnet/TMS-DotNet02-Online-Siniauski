using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Siniauski.WhatIWant.Data.Constants;
using Siniauski.WhatIWant.Data.Models;

namespace Siniauski.WhatIWant.Data.Configurations
{
    public class FriendConfiguration : IEntityTypeConfiguration<Friend>
    {
        public void Configure(EntityTypeBuilder<Friend> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(Table.Friends)
                .HasKey(friends => friends.Id);

            builder.Property(friends => friends.Id)
                .IsRequired();

            builder.Property(friends => friends.FirstUserId)
                .IsRequired();

            builder.Property(friends => friends.SecondUserId)
               .IsRequired();

            builder.HasOne(friends => friends.FirstUser)
                    .WithMany(user => user.Friends)
                    .HasForeignKey(friends => friends.FirstUserId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(friends => friends.SecondUser)
                    .WithMany()
                    .HasForeignKey(friends => friends.SecondUserId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}