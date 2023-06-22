using System;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Entities
{
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public object Color { get; set; }
        public object IconUrl { get; set; }
        public string Description { get; set; }
        public bool IsModerator { get; set; }
        public bool IsAdministrator { get; set; }
        public int DisplayOrder { get; set; }
    }
}
