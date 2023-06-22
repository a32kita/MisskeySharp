using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Entities
{
    internal class TokenResponse
    {
        public bool Ok { get; set; }

        public string Token { get; set; }

        public User User { get; set; }
    }
}
