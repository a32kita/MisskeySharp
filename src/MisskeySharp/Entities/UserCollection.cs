using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Entities
{
    public class UserCollection : MisskeyApiEntitiesBase, IList<User>
    {
        private List<User> _users;

        public User this[int index]
        {
            get => this._users[index];
            set => this._users[index] = value;
        }

        public int Count
        {
            get => this._users.Count;
        }

        public bool IsReadOnly
        {
            get => ((IList<User>)this._users).IsReadOnly;
        }


        public UserCollection()
        {
            this._users = new List<User>();
        }


        public void Add(User item)
        {
            this._users.Add(item);
        }

        public void Clear()
        {
            this._users.Clear();
        }

        public bool Contains(User item)
        {
            return this._users.Contains(item);
        }

        public void CopyTo(User[] array, int arrayIndex)
        {
            this._users.CopyTo(array, arrayIndex);
        }

        public IEnumerator<User> GetEnumerator()
        {
            return this._users.GetEnumerator();
        }

        public int IndexOf(User item)
        {
            return this._users.IndexOf(item);
        }

        public void Insert(int index, User item)
        {
            this._users.Insert(index, item);
        }

        public bool Remove(User item)
        {
            return this._users.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this._users.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_users).GetEnumerator();
        }
    }
}
