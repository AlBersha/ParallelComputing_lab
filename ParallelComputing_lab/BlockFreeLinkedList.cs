using System;
using System.IO;
using ParallelComputing_lab.Helper;
using ParallelComputing_lab.Nodes;

namespace ParallelComputing_lab
{
    public class BlockFreeLinkedList<TValue>
        // where TKey : new()
        where TValue : IComparable<TValue>, new()
    {
        private LinkedListNode<TValue> Top { get; }
        private LinkedListNode<TValue> Tail { get; }

        public BlockFreeLinkedList()
        {
            Top = new LinkedListNode<TValue>(new TValue());
            Tail = new LinkedListNode<TValue>(new TValue());
                
            while (!Top.Next.CompareAndSet(Tail, false, default, false))
            {
            }
        }

        public bool Add(TValue value)
        {
            var node = new LinkedListNode<TValue>(value);

            while (true)
            {
                var left = (LinkedListNode<TValue>)null;
                var right = Search(value, ref left);
                if (right != Tail && right.Value.CompareTo(value) == 0)
                {
                    return false;
                }

                node.Next = new MarkedReference<LinkedListNode<TValue>>(right, false);
                if (left.Next.CompareAndSet(node, false, right, false))
                {
                    return true;
                }
            }
        }

        public bool Remove(TValue value)
        {
            while (true)
            {
                var left = (LinkedListNode<TValue>)null;
                var right = Search(value, ref left);

                if (right.Value.CompareTo(value) != 0)
                {
                    return false;
                }

                var nextRight = right.Next.Value;
                var s = right.Next.TryMark(nextRight, true);
                if (s)
                {
                    continue;
                }

                left.Next.CompareAndSet(nextRight, false, right, false);
                return true;
            }
        }

        private LinkedListNode<TValue> Search(TValue value, ref LinkedListNode<TValue> left)
        {
            var isRetryNeed = false;
            var flag = false;

            while (true)
            {
                var top = Top;
                var nextTop = top.Next.Value;

                while (true)
                {
                    var success = nextTop.Next.Get(ref flag);
                    
                    
                    while (flag)
                    {
                        var s = top.Next.CompareAndSet(success, false, nextTop, false);
                        if (!s)
                        {
                            isRetryNeed = true;
                            break;
                        }

                        nextTop = top.Next.Value;
                        success = nextTop.Next.Get(ref flag);
                    }

                    if (isRetryNeed)
                    {
                        isRetryNeed = false;
                        continue;
                    }

                    if (nextTop.Value.CompareTo(value) < 0)
                    {
                        top = nextTop;
                        nextTop = success;
                    }
                    else
                    {
                        left = top;
                        return nextTop;
                    }
                }
            }
        }
    }
}