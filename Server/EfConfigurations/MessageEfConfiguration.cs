using messanger.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace messanger.Server.EfConfigurations
{
    public class MessageEfConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Message");

            builder.HasKey(m => m.IdMessage)
                .HasName("Message_pk");

            builder.Property(m => m.IdMessage)
                .UseIdentityColumn();

            builder.Property(m => m.IdParentMessage).IsRequired(false);

            builder.Property(m => m.Content).IsRequired().HasMaxLength(256);

            builder.Property(m => m.CreatedAt).IsRequired();

            builder.HasOne(m => m.IdParentMessageNavigation)
                .WithMany(pm => pm.ChildrenMessages)
                .HasForeignKey(m => m.IdParentMessage)
                .HasConstraintName("Message_ParentMessage");

            builder.HasOne(m => m.IdSenderNavigation)
                .WithMany(s => s.SentMessages)
                .HasForeignKey(m => m.IdSender)
                .HasConstraintName("Message_Sender");
        }
    }
}
