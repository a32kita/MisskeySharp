using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MisskeySharp.Entities
{
    public class FileCollection : MisskeyApiEntitiesBase, IList<File>
    {
        private List<File> _fs;

        public File this[int index]
        {
            get => this._fs[index];
            set => this._fs[index] = value;
        }

        public int Count
        {
            get => this._fs.Count;
        }

        public bool IsReadOnly
        {
            get => ((IList<File>)this._fs).IsReadOnly;
        }


        public FileCollection()
        {
            this._fs = new List<File>();
        }


        public void Add(File item)
        {
            this._fs.Add(item);
        }

        public void Clear()
        {
            this._fs.Clear();
        }

        public bool Contains(File item)
        {
            return this._fs.Contains(item);
        }

        public void CopyTo(File[] array, int arrayIndex)
        {
            this._fs.CopyTo(array, arrayIndex);
        }

        public IEnumerator<File> GetEnumerator()
        {
            return this._fs.GetEnumerator();
        }

        public int IndexOf(File item)
        {
            return this._fs.IndexOf(item);
        }

        public void Insert(int index, File item)
        {
            this._fs.Insert(index, item);
        }

        public bool Remove(File item)
        {
            return this._fs.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this._fs.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_fs).GetEnumerator();
        }
    }
}
