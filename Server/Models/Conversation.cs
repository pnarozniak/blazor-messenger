using System.Collections.Generic;

namespace messanger.Server.Models
{
    public class Conversation
    {
        public Conversation()
        {
            ConversationMembers = new HashSet<ConversationMember>();
            Messages = new HashSet<Message>();
        }

        public int IdConversation { get; set; }
        public int? IdAvatar { get; set; }
        public string Name { get; set; }

        public virtual File IdAvatarNavigation { get; set; }
        public virtual ICollection<ConversationMember> ConversationMembers { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
