using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MisskeySharp.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public object Host { get; set; }
        public string AvatarUrl { get; set; }
        public string AvatarBlurhash { get; set; }
        public bool IsBot { get; set; }
        public bool IsCat { get; set; }
        public Emojis Emojis { get; set; }
        public string OnlineStatus { get; set; }
        public List<object> BadgeRoles { get; set; }
        public object Url { get; set; }
        public object Uri { get; set; }
        public object MovedTo { get; set; }
        public object AlsoKnownAs { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public object LastFetchedAt { get; set; }
        public object BannerUrl { get; set; }
        public object BannerBlurhash { get; set; }
        public bool IsLocked { get; set; }
        public bool IsSilenced { get; set; }
        public bool IsSuspended { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public object Birthday { get; set; }
        public string Lang { get; set; }
        public List<Field> Fields { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public int NotesCount { get; set; }
        public List<string> PinnedNoteIds { get; set; }
        public List<Note> PinnedNotes { get; set; }
        public object PinnedPageId { get; set; }
        public object PinnedPage { get; set; }
        public bool PublicReactions { get; set; }
        public string FfVisibility { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool UsePasswordLessLogin { get; set; }
        public bool SecurityKeys { get; set; }
        public List<Role> Roles { get; set; }
        public object Memo { get; set; }
    }
}
