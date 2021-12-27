using System.Threading;

namespace ParallelComputing_lab.Helper
{
    public class AtomicReference<T> where T : class
    {
        private T _value;
        
        public T Value
        {
            get
            {
                var obj = (object)_value;
                return (T)Thread.VolatileRead(ref obj);
            }
        }
        
        public AtomicReference()
        {
            _value = default(T);
        }

        public AtomicReference(T value)
        {
            _value = value;
        }

        public bool CompareAndSet(T previous, T newVal)
        {
            return ReferenceEquals(Interlocked.CompareExchange(ref _value, newVal, previous), previous);
        }
    }
}