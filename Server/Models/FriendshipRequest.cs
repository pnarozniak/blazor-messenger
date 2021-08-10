using System;

namespace messanger.Server.Models
{
    public class FriendshipRequest
    {
        public string IdSender { get; set; }
        public string IdReceiver { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User IdSenderNavigation { get; set; }
        public virtual User IdReceiverNavigation { get; set; }
    }
}
