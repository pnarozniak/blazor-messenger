using messanger.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace messanger.Server.EfConfigurations
{
    public class UserEfConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(64);

            builder.Property(u => u.LastName).IsRequired().HasMaxLength(64);

            builder.Property(u => u.Birthdate).IsRequired();

            builder.Property(u => u.CreatedAt).IsRequired();
        }
    }
}
