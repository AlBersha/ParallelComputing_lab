namespace ParallelComputing_lab.Nodes
{
    public class QueueNode<T>
    {
        public T Value { get; }
        public QueueNode<T> Next;

        public QueueNode(T value, QueueNode<T> next)
        {
            Value = value;
            Next = next;
        }
    }
}