using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Siniauski.WhatIWant.Data.Constants;
using Siniauski.WhatIWant.Data.Models;

namespace Siniauski.WhatIWant.Data.Configurations
{
    public class WishConfiguration : IEntityTypeConfiguration<Wish>
    {
        public void Configure(EntityTypeBuilder<Wish> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.ToTable(Table.Wishes)
                .HasKey(wish => wish.Id);

            builder.Property(wish => wish.Id)
                .IsRequired();

            builder.Property(wish => wish.UserId)
                .IsRequired();

            builder.Property(wish => wish.Name)
                .IsRequired();

            builder.Property(wish => wish.IsDone)
               .HasDefaultValue(false);

            builder.HasOne(wish => wish.User)
                    .WithMany(user => user.Wishes)
                    .HasForeignKey(wish => wish.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}