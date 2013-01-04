using System.Collections.Generic;
using System.ComponentModel;

namespace CallWall
{
    public sealed class NotifyEventComparer : IEqualityComparer<PropertyChangedEventArgs>
    {
        public static readonly NotifyEventComparer Instance = new NotifyEventComparer();

        bool IEqualityComparer<PropertyChangedEventArgs>.Equals(PropertyChangedEventArgs x, PropertyChangedEventArgs y)
        {
            return x.PropertyName == y.PropertyName;
        }

        int IEqualityComparer<PropertyChangedEventArgs>.GetHashCode(PropertyChangedEventArgs obj)
        {
            return obj.PropertyName.GetHashCode();
        }
    }
}