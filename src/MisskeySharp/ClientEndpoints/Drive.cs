using MisskeySharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MisskeySharp.ClientEndpoints
{
    public class Drive : EndpointBase
    {
        public Drive_Files Files
        {
            get;
            private set;
        }

        internal Drive(MisskeyService parent)
            : base(parent)
        {
            this.Files = new Drive_Files(parent);
        }

        public async Task<DriveCapacityInfo> Get()
        {
            return await this.Parent.PostAsync<VoidParameter, DriveCapacityInfo>("drive", new VoidParameter());
        }


    }
}
