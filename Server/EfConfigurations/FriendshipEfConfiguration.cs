using messanger.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace messanger.Server.EfConfigurations
{
    public class FriendshipEfConfiguration : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.ToTable("Friendship");

            builder.HasKey(f => new { f.IdUser1, f.IdUser2 })
                .HasName("Friendship_pk");

            builder.Property(f => f.CreatedAt).IsRequired();

            builder.HasOne(f => f.IdUser1Navigation)
                .WithMany(u1 => u1.FriendshipsWhereIsUser1)
                .HasForeignKey(f => f.IdUser1)
                .HasConstraintName("Friendship_User1")
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(f => f.IdUser2Navigation)
                .WithMany(u2 => u2.FriendshipsWhereIsUser2)
                .HasForeignKey(f => f.IdUser2)
                .HasConstraintName("Friendship_User2")
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
