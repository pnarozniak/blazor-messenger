namespace messanger.Server.Models
{
    public class File
    {
        public int IdFile { get; set; }
        public int? IdMessage { get; set; }
        public string Name { get; set; }

        public virtual User IdUserNavigation { get; set; }
        public virtual Conversation IdConversationNavigation { get; set; }
        public virtual Message IdMessageNavigation { get; set; }
    }
}
