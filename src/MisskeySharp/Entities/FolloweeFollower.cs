using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace MisskeySharp.Entities
{
    public class FolloweeFollower
    {
        public string Id
        {
            get;
            set;
        }

        public DateTime CreatedAt
        {
            get;
            set;
        }

        public string FolloweeId
        {
            get;
            set;
        }

        public string FollowerId
        {
            get;
            set;
        }

        public User Followee
        {
            get;
            set;
        }

        public User Follower
        {
            get;
            set;
        }
    }
}
