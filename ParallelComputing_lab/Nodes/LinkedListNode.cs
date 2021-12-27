using ParallelComputing_lab.Helper;

namespace ParallelComputing_lab.Nodes
{
    public class LinkedListNode<T>
    {
        internal T Value { get; }
        public MarkedReference<LinkedListNode<T>> Next { get; set; }

        public LinkedListNode(T value, MarkedReference<LinkedListNode<T>> next = null)
        {
            Value = value;
            Next = next;
        }
    }
}