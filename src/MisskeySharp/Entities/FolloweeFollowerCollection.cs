using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Entities
{
    public class FolloweeFollowerCollection : MisskeyApiEntitiesBase, IList<FolloweeFollower>
    {
        private List<FolloweeFollower> _ff;

        public FolloweeFollower this[int index]
        {
            get => this._ff[index];
            set => this._ff[index] = value;
        }

        public int Count
        {
            get => this._ff.Count;
        }

        public bool IsReadOnly
        {
            get => ((IList<FolloweeFollower>)this._ff).IsReadOnly;
        }


        public FolloweeFollowerCollection()
        {
            this._ff = new List<FolloweeFollower>();
        }


        public void Add(FolloweeFollower item)
        {
            this._ff.Add(item);
        }

        public void Clear()
        {
            this._ff.Clear();
        }

        public bool Contains(FolloweeFollower item)
        {
            return this._ff.Contains(item);
        }

        public void CopyTo(FolloweeFollower[] array, int arrayIndex)
        {
            this._ff.CopyTo(array, arrayIndex);
        }

        public IEnumerator<FolloweeFollower> GetEnumerator()
        {
            return this._ff.GetEnumerator();
        }

        public int IndexOf(FolloweeFollower item)
        {
            return this._ff.IndexOf(item);
        }

        public void Insert(int index, FolloweeFollower item)
        {
            this._ff.Insert(index, item);
        }

        public bool Remove(FolloweeFollower item)
        {
            return this._ff.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this._ff.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_ff).GetEnumerator();
        }
    }
}
