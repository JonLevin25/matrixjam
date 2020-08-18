using System.Collections.Generic;

namespace MatrixJam.Team19.DataStructures
{
    public class FixedSizedQueue<T>: Queue<T>
    {

        public int MaxSize { get; private set; }

        public FixedSizedQueue(int size)
        {
            MaxSize = size;
        }

        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);

            while (Count > MaxSize)
            {
                base.Dequeue();
            }
        }
    }
}