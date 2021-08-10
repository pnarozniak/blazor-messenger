using messanger.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace messanger.Server.EfConfigurations
{
    public class ConversationMemberEfConfiguration : IEntityTypeConfiguration<ConversationMember>
    {
        public void Configure(EntityTypeBuilder<ConversationMember> builder)
        {
            builder.ToTable("ConversationMember");

            builder.HasKey(p => new { p.IdUser, p.IdConversation })
                .HasName("ConversationMember_pk");

            builder.Property(p => p.CreatedAt).IsRequired();

            builder.HasOne(p => p.IdConversationNavigation)
                .WithMany(c => c.ConversationMembers)
                .HasForeignKey(p => p.IdConversation)
                .HasConstraintName("Conversation_ConversationMembers")
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(p => p.IdUserNavigation)
                .WithMany(u => u.ConversationsParticipation)
                .HasForeignKey(p => p.IdUser)
                .HasConstraintName("User_ConversationMembers");
        }
    }
}
