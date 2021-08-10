using System;

namespace messanger.Server.Models
{
    public class Friendship
    {
        public string IdUser1 { get; set; }
        public string IdUser2 { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User IdUser1Navigation { get; set; }
        public virtual User IdUser2Navigation { get; set; }
    }
}
