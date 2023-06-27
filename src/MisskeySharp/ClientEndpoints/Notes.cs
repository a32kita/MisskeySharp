using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MisskeySharp.Entities;

namespace MisskeySharp.ClientEndpoints
{
    public class Notes : EndpointBase
    {
        internal Notes(MisskeyService parent)
            : base(parent)
        { }


        public async Task<NoteCreated> Create(Note note)
        {
            return await this.Parent.PostAsync<Note, NoteCreated>("notes/create", note);
        }

        public async Task<NoteCollection> Search(NoteSearchQuery query)
        {
            return await this.Parent.PostAsync<NoteSearchQuery, NoteCollection>("notes/search", query);
        }

        public async Task<NoteCollection> Timeline(NotesTimelineParameter parameter)
        {
            return await this.Parent.PostAsync<NotesTimelineParameter, NoteCollection>("notes/timeline", parameter);
        }
    }
}
