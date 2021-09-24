using System.Collections.Generic;
using System.Linq;

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
        public bool IsPrivate { get; set; }

        public virtual File IdAvatarNavigation { get; set; }
        public virtual ICollection<ConversationMember> ConversationMembers { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public string ConstructNameForUser(string idUser)
        {
            if (Name is not null)
                return Name;

            if (IsPrivate)
                return ConversationMembers
                    .Where(cm => cm.IdUser != idUser)
                    .Select(cm => cm.IdUserNavigation)
                    .Select(u => $"{u.FirstName} {u.LastName}")
                    .FirstOrDefault();

            return string.Join(",", ConversationMembers
                .Where(cm => cm.IdUser != idUser)
                .Take(5)
                .Select(cm => cm.IdUserNavigation)
                .Select(u => $"{u.FirstName} {u.LastName}"));
        }
    }
}
