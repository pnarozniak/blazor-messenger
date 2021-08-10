using System;
using System.Collections.Generic;

namespace messanger.Server.Models
{
    public class Message
    {
        public Message()
        {
            ChildrenMessages = new HashSet<Message>();
            AttachedFiles = new HashSet<File>();
        }

        public int IdMessage { get; set; }
        public int IdConversation { get; set; }
        public int? IdParentMessage { get; set; }
        public string IdSender { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Conversation IdConversationNavigation { get; set; }
        public virtual Message IdParentMessageNavigation { get; set; }
        public virtual ICollection<Message> ChildrenMessages { get; set; }
        public virtual ICollection<File> AttachedFiles { get; set; }
    }
}
