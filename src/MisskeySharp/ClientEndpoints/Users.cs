using MisskeySharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MisskeySharp.ClientEndpoints
{
    public class Users : EndpointBase
    {
        internal Users(MisskeyService parent)
            : base(parent) { }

        public async Task<FolloweeFollowerCollection> Following(UsersFollowingFollowersQuery query)
        {
            return await this.Parent.PostAsync<UsersFollowingFollowersQuery, FolloweeFollowerCollection>("users/following", query);
        }

        public async Task<FolloweeFollowerCollection> Follower(UsersFollowingFollowersQuery query)
        {
            return await this.Parent.PostAsync<UsersFollowingFollowersQuery, FolloweeFollowerCollection>("users/followers", query);
        }

        public async Task<NoteCollection> Notes(UsersNoteQuery query)
        {
            return await this.Parent.PostAsync<UsersNoteQuery, NoteCollection>("users/notes", query);
        }

        public async Task<UserCollection> Search(UsersSearchQuery query)
        {
            return await this.Parent.PostAsync<UsersSearchQuery, UserCollection>("users/search", query);
        }
    }
}
