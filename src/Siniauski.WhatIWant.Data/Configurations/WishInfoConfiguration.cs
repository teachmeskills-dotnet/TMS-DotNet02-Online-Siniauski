using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Siniauski.WhatIWant.Data.Constants;
using Siniauski.WhatIWant.Data.Models;

namespace Siniauski.WhatIWant.Data.Configurations
{
    public class WishInfoConfiguration : IEntityTypeConfiguration<WishInfo>
    {
        public void Configure(EntityTypeBuilder<WishInfo> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(Table.WishesInfo)
                .HasKey(wishInfo => wishInfo.Id);

            builder.Property(wishInfo => wishInfo.Id)
                .IsRequired();

            builder.Property(wishInfo => wishInfo.WishId)
                .IsRequired();

            builder.Property(wishInfo => wishInfo.UserId)
               .IsRequired();

            builder.Property(wishInfo => wishInfo.IsRead)
               .HasDefaultValue(false);

            builder.Property(wishInfo => wishInfo.IsBlocked)
               .HasDefaultValue(false);

            builder.HasOne(wishInfo => wishInfo.User)
                    .WithMany(user => user.WishesInfo)
                    .HasForeignKey(wishInfo => wishInfo.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(wishInfo => wishInfo.Wish)
                    .WithMany(wish => wish.WishesInfo)
                    .HasForeignKey(wishInfo => wishInfo.WishId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}