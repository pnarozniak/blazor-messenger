using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace messanger.Server.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            SentFriendshipRequests = new HashSet<FriendshipRequest>();
            ReceivedFriendshipRequests = new HashSet<FriendshipRequest>();
            FriendshipsWhereIsUser1 = new HashSet<Friendship>();
            FriendshipsWhereIsUser2 = new HashSet<Friendship>();
            ConversationsParticipation = new HashSet<ConversationMember>();
            SentMessages = new HashSet<Message>();
        }


        public int IdAvatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime CreatedAt { get; set; }


        public virtual File IdAvatarNavigation { get; set; }
        public virtual ICollection<FriendshipRequest> SentFriendshipRequests { get; set; }
        public virtual ICollection<FriendshipRequest> ReceivedFriendshipRequests { get; set; }
        public virtual ICollection<Friendship> FriendshipsWhereIsUser1 { get; set; }
        public virtual ICollection<Friendship> FriendshipsWhereIsUser2 { get; set; }
        public virtual ICollection<ConversationMember> ConversationsParticipation { get; set; }
        public virtual ICollection<Message> SentMessages { get; set; }
    }
}
