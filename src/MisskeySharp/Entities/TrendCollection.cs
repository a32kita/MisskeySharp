using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Entities
{
    public class TrendCollection : MisskeyApiEntitiesBase, IList<Trend>
    {
        private List<Trend> _trends;

        public Trend this[int index]
        {
            get => this._trends[index];
            set => this._trends[index] = value;
        }

        public int Count
        {
            get => this._trends.Count;
        }

        public bool IsReadOnly
        {
            get => ((IList<Trend>)this._trends).IsReadOnly;
        }


        public TrendCollection()
        {
            this._trends = new List<Trend>();
        }


        public void Add(Trend item)
        {
            this._trends.Add(item);
        }

        public void Clear()
        {
            this._trends.Clear();
        }

        public bool Contains(Trend item)
        {
            return this._trends.Contains(item);
        }

        public void CopyTo(Trend[] array, int arrayIndex)
        {
            this._trends.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Trend> GetEnumerator()
        {
            return this._trends.GetEnumerator();
        }

        public int IndexOf(Trend item)
        {
            return this._trends.IndexOf(item);
        }

        public void Insert(int index, Trend item)
        {
            this._trends.Insert(index, item);
        }

        public bool Remove(Trend item)
        {
            return this._trends.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this._trends.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_trends).GetEnumerator();
        }
    }
}
