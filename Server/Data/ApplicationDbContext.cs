using IdentityServer4.EntityFramework.Options;
using messanger.Server.EfConfigurations;
using messanger.Server.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace messanger.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<User>
    {
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Conversation> Conversations { get; set; }
        public virtual DbSet<ConversationMember> ConversationMembers { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Friendship> Friendships { get; set; }
        public virtual DbSet<FriendshipRequest> FriendshipRequests { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserEfConfiguration());
            builder.ApplyConfiguration(new FileEfConfiguration());
            builder.ApplyConfiguration(new ConversationEfConfiguration());
            builder.ApplyConfiguration(new ConversationMemberEfConfiguration());
            builder.ApplyConfiguration(new MessageEfConfiguration());
            builder.ApplyConfiguration(new FriendshipEfConfiguration());
            builder.ApplyConfiguration(new FriendshipRequestEfConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
