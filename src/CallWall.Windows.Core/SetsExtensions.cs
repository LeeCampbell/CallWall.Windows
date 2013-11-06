using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CallWall.Windows
{
    public static class SetsExtensions
    {
        public static void AddRange<T>(this ISet<T> target, IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                target.Add(item);
            }
        }
    }

    public sealed class EmptySet<T> : ISet<T>
    {
        public static readonly EmptySet<T> Instance = new EmptySet<T>();

        bool ISet<T>.Add(T item)
        {
            throw new System.NotSupportedException();
        }

        void ISet<T>.ExceptWith(IEnumerable<T> other)
        {
            throw new System.NotSupportedException();
        }

        void ISet<T>.IntersectWith(IEnumerable<T> other)
        {
            throw new System.NotSupportedException();
        }

        bool ISet<T>.IsProperSubsetOf(IEnumerable<T> other)
        {
            return true;
        }

        bool ISet<T>.IsProperSupersetOf(IEnumerable<T> other)
        {
            return false;
        }

        bool ISet<T>.IsSubsetOf(IEnumerable<T> other)
        {
            return true;
        }

        bool ISet<T>.IsSupersetOf(IEnumerable<T> other)
        {
            return false;
        }

        bool ISet<T>.Overlaps(IEnumerable<T> other)
        {
            return true;
        }

        bool ISet<T>.SetEquals(IEnumerable<T> other)
        {
            return !other.Any();
        }

        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new System.NotSupportedException();
        }

        void ISet<T>.UnionWith(IEnumerable<T> other)
        {
            throw new System.NotSupportedException();
        }

        void ICollection<T>.Add(T item)
        {
            throw new System.NotSupportedException();
        }

        void ICollection<T>.Clear()
        {
            throw new System.NotSupportedException();
        }

        bool ICollection<T>.Contains(T item)
        {
            return false;
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
        }

        int ICollection<T>.Count
        {
            get { return 0; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return true; }
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new System.NotSupportedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return Enumerable.Empty<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Enumerable.Empty<T>().GetEnumerator();
        }
    }
}