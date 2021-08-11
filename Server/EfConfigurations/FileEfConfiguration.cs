using messanger.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace messanger.Server.EfConfigurations
{
    public class FileEfConfiguration : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.ToTable("File");

            builder.HasKey(f => f.IdFile)
                .HasName("File_pk");

            builder.Property(f => f.IdFile)
                .UseIdentityColumn();

            builder.Property(f => f.IdMessage).IsRequired(false);

            builder.Property(f => f.Name).IsRequired().HasMaxLength(256);

            builder.HasOne(f => f.IdUserNavigation)
                .WithOne(u => u.IdAvatarNavigation)
                .HasForeignKey<User>(u => u.IdAvatar)
                .IsRequired(false)
                .HasConstraintName("User_AvatarFile");

            builder.HasOne(f => f.IdConversationNavigation)
                .WithOne(c => c.IdAvatarNavigation)
                .HasForeignKey<Conversation>(c => c.IdAvatar)
                .IsRequired(false)
                .HasConstraintName("Conversation_AvatarFile");

            builder.HasOne(f => f.IdMessageNavigation)
                .WithMany(m => m.AttachedFiles)
                .HasForeignKey(f => f.IdMessage)
                .IsRequired(false)
                .HasConstraintName("Message_AttachedFiles");
        }
    }
}
