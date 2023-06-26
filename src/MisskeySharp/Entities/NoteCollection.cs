using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MisskeySharp.Entities
{
    public class NoteCollection : MisskeyApiEntitiesBase, IList<Note>
    {
        private List<Note> _notes;

        public Note this[int index]
        {
            get => this._notes[index];
            set => this._notes[index] = value;
        }

        public int Count
        {
            get => this._notes.Count;
        }

        public bool IsReadOnly
        {
            get => ((IList<Note>)this._notes).IsReadOnly;
        }


        public NoteCollection()
        {
            this._notes = new List<Note>();
        }


        public void Add(Note item)
        {
            this._notes.Add(item);
        }

        public void Clear()
        {
            this._notes.Clear();
        }

        public bool Contains(Note item)
        {
            return this._notes.Contains(item);
        }

        public void CopyTo(Note[] array, int arrayIndex)
        {
            this._notes.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Note> GetEnumerator()
        {
            return this._notes.GetEnumerator();
        }

        public int IndexOf(Note item)
        {
            return this._notes.IndexOf(item);
        }

        public void Insert(int index, Note item)
        {
            this._notes.Insert(index, item);
        }

        public bool Remove(Note item)
        {
            return this._notes.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this._notes.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_notes).GetEnumerator();
        }
    }
}
