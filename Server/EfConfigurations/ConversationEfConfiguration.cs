using messanger.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace messanger.Server.EfConfigurations
{
    public class ConversationEfConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.ToTable("Conversation");

            builder.HasKey(c => c.IdConversation)
                .HasName("Conversation_pk");

            builder.Property(c => c.IdConversation)
                .UseIdentityColumn();

            builder.Property(c => c.Name).IsRequired(false).HasMaxLength(64);

            builder.HasMany(c => c.Messages)
                .WithOne(m => m.IdConversationNavigation)
                .HasForeignKey(m => m.IdConversation)
                .HasConstraintName("Conversation_Message");
        }
    }
}
