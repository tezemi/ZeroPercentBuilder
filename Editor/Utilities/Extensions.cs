using System.Collections.Generic;

namespace ZeroPercentBuilder.Utilities
{
    public static class Extensions
    {
        public static void MoveToFirst<T>(this List<T> list, T item)
        {
            int index = list.IndexOf(item);

            if (index <= 0) 
                return;

            list.RemoveAt(index);
            list.Insert(0, item);
        }

        public static void MoveToLast<T>(this List<T> list, T item)
        {
int index = list.IndexOf(item);

            if (index >= list.Count - 1) 
                return;

            list.RemoveAt(index);
            list.Add(item);
        }

        public static void MoveBack<T>(this List<T> list, T item)
        {
            int index = list.IndexOf(item);

            if (index <= 0) 
                return;

            (list[index], list[index - 1]) = (list[index - 1], list[index]);
        }

        public static void MoveForward<T>(this List<T> list, T item)
        {
            int index = list.IndexOf(item);

            if (index >= list.Count - 1) 
                return;

            (list[index], list[index + 1]) = (list[index + 1], list[index]);
        }
    }    
}
