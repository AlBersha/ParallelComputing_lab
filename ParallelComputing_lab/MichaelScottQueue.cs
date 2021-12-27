using System;
using System.Threading;
using ParallelComputing_lab.Node;

namespace ParallelComputing_lab
{
    public class MichaelScottQueue<T>
    {
        public QueueNode<T> Top;
        public QueueNode<T> Tail;

        public MichaelScottQueue()
        {
            var node = new QueueNode<T>(default(T), null);
            Top = node;
            Tail = node;
        }

        public bool Push(T value)
        {
            var node = new QueueNode<T>(value, null);
            while (true)
            {
                var tail = Tail;
                if (tail != null && CompareAndSet(ref tail.Next, node, null))
                {
                    CompareAndSet(ref Tail, node, tail);
                    return true;
                }

                CompareAndSet(ref Tail, tail.Next, tail);
            }
        }

        public T Pop()
        {
            while (true)
            {
                var top = Top;
                var tail = Tail;
                var nextTop = top.Next;

                if (top == tail)
                {
                    if (nextTop == null) throw new Exception();
                    CompareAndSet(ref Tail, nextTop, tail);
                }
                else
                {
                    if (CompareAndSet(ref Top, nextTop, top)) return nextTop.Value;
                }
            }
        }
        
        private bool CompareAndSet (ref QueueNode<T> location, QueueNode<T> value, QueueNode<T> comparand)
        {
            return comparand == Interlocked.CompareExchange(ref location, value, comparand);
        }
    }
}