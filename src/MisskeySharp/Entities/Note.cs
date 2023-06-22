using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Entities
{
    public class Note
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public object Cw { get; set; }
        public string Visibility { get; set; }
        public bool LocalOnly { get; set; }
        public object ReactionAcceptance { get; set; }
        public int RenoteCount { get; set; }
        public int RepliesCount { get; set; }
        public Reactions Reactions { get; set; }
        public Emojis ReactionEmojis { get; set; }
        public List<object> FileIds { get; set; }
        public List<object> Files { get; set; }
        public object ReplyId { get; set; }
        public object RenoteId { get; set; }
    }
}
