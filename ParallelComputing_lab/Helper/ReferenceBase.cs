namespace ParallelComputing_lab.Helper
{
    public class ReferenceBase<T>
    {
        public readonly T Value;
        public readonly AtomicBool Flag;

        public ReferenceBase(T value, bool mark)
        {
            Value = value;
            Flag = new AtomicBool(mark);
        }
    }
}