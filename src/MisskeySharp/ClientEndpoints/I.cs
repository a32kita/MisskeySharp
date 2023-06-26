using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MisskeySharp.Entities;

namespace MisskeySharp.ClientEndpoints
{
    public class I : EndpointBase
    {
        internal I(MisskeyService parent)
            : base(parent) { }


        public async Task<User> Get()
        {
            return await this.Parent.PostAsync<VoidParameter, User>("i", new VoidParameter());
        }
    }
}
