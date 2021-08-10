using messanger.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace messanger.Server.EfConfigurations
{
    public class FriendshipRequestEfConfiguration : IEntityTypeConfiguration<FriendshipRequest>
    {
        public void Configure(EntityTypeBuilder<FriendshipRequest> builder)
        {
            builder.ToTable("FriendshipRequest");

            builder.HasKey(f => new { f.IdSender, f.IdReceiver })
                .HasName("FriendshipRequest_pk");

            builder.Property(f => f.CreatedAt).IsRequired();

            builder.HasOne(f => f.IdSenderNavigation)
                .WithMany(s => s.SentFriendshipRequests)
                .HasForeignKey(f => f.IdSender)
                .HasConstraintName("FriendshipRequest_Sender")
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(f => f.IdReceiverNavigation)
                .WithMany(r => r.ReceivedFriendshipRequests)
                .HasForeignKey(f => f.IdReceiver)
                .HasConstraintName("FriendshipRequest_Receiver")
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
