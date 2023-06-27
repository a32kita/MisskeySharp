using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Entities
{
    public class Trend : MisskeyApiEntitiesBase
    {
        public string Tag
        {
            get;
            set;
        }

        public int[] Chart
        {
            get;
            set;
        }

        public int UsersCount
        {
            get;
            set;
        }
    }
}
