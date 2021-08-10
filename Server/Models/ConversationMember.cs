using System;

namespace messanger.Server.Models
{
    public class ConversationMember
    {
        public string IdUser { get; set; }
        public int IdConversation { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User IdUserNavigation { get; set; }
        public virtual Conversation IdConversationNavigation { get; set; }
    }
}
