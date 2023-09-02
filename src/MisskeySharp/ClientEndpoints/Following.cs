using MisskeySharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MisskeySharp.ClientEndpoints
{
    public class Following : EndpointBase
    {
        internal Following(MisskeyService parent)
            : base(parent)
        { }

        public async Task<User> Create(FollowRequestParameter parameter)
        {
            return await this.Parent.PostAsync<FollowRequestParameter, User>("following/create", parameter);
        }

        public async Task<User> Delete(FollowRequestParameter parameter)
        {
            return await this.Parent.PostAsync<FollowRequestParameter, User>("following/delete", parameter);
        }
    }
}
