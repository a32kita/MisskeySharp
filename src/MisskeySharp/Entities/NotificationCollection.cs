using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Entities
{
    public class NotificationCollection : MisskeyApiEntitiesBase, IList<Notification>
    {
        private List<Notification> _notificationss;

        public Notification this[int index]
        {
            get => this._notificationss[index];
            set => this._notificationss[index] = value;
        }

        public int Count
        {
            get => this._notificationss.Count;
        }

        public bool IsReadOnly
        {
            get => ((IList<Notification>)this._notificationss).IsReadOnly;
        }


        public NotificationCollection()
        {
            this._notificationss = new List<Notification>();
        }


        public void Add(Notification item)
        {
            this._notificationss.Add(item);
        }

        public void Clear()
        {
            this._notificationss.Clear();
        }

        public bool Contains(Notification item)
        {
            return this._notificationss.Contains(item);
        }

        public void CopyTo(Notification[] array, int arrayIndex)
        {
            this._notificationss.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Notification> GetEnumerator()
        {
            return this._notificationss.GetEnumerator();
        }

        public int IndexOf(Notification item)
        {
            return this._notificationss.IndexOf(item);
        }

        public void Insert(int index, Notification item)
        {
            this._notificationss.Insert(index, item);
        }

        public bool Remove(Notification item)
        {
            return this._notificationss.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this._notificationss.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_notificationss).GetEnumerator();
        }
    }
}
