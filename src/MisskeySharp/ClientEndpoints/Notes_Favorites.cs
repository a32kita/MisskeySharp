using MisskeySharp.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MisskeySharp.ClientEndpoints
{
    public class Notes_Favorites : EndpointBase
    {
        internal Notes_Favorites(MisskeyService parent)
            : base(parent) { }


        public async Task Create(NotesFavoriteCreateParameter parameter)
        {
            await this.Parent.PostAsync<NotesFavoriteCreateParameter, VoidResponse>("notes/favorites/create", parameter);
        }
    }
}
