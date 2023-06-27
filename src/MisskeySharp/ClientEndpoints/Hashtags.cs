using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MisskeySharp.Entities;

namespace MisskeySharp.ClientEndpoints
{
    public class Hashtags : EndpointBase
    {
        internal Hashtags(MisskeyService parent)
            : base(parent)
        { }

        
        public async Task<TrendCollection> Trend()
        {
            return await this.Parent.PostAsync<VoidParameter, TrendCollection>("hashtags/trend", new VoidParameter());
        }
    }
}
