using System.Collections.Generic;

namespace Wish.Common
{
    public class UniqueList<T> : List<T>
    {
        public new void Add(T item)
        {
// ReSharper disable CompareNonConstrainedGenericWithNull
            if (null == item) return;
// ReSharper restore CompareNonConstrainedGenericWithNull
            if (Contains(item)) return;
            Insert(0, item);
        }
    }
}
