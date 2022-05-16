using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Siniauski.WhatIWant.Data.Models;

namespace Siniauski.WhatIWant.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));

            builder.Property(user => user.UserName)
                .IsRequired();
            builder.Property(user => user.FirstName)
                .IsRequired();
            builder.Property(user => user.LastName)
                .IsRequired();
            builder.Property(user => user.Email)
                .IsRequired();
            builder.Property(user => user.IsActive)
                .HasDefaultValue(true);
        }
    }
}