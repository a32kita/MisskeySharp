using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Entities
{
    public class DriveCapacityInfo : MisskeyApiEntitiesBase
    {
        public long Capacity
        {
            get;
            set;
        }

        public long Usage
        {
            get;
            set;
        }
    }
}
