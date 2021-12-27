using System.Threading;

namespace ParallelComputing_lab.Helper
{
    public class AtomicBool
    {
        private const int TrueValue = 1;
        private const int FalseValue = 0;

        private int _current;

        public AtomicBool(bool init)
        {
            _current = init ? TrueValue : FalseValue;
        }

        public bool Value => Interlocked.Add(ref _current, 0) == TrueValue;

        public bool SetValue(bool value)
        {
            return Interlocked.Exchange(ref _current, value ? TrueValue : FalseValue) == TrueValue;
        }

        public bool CompareAndSet(bool previous, bool init)
        {
            return Interlocked.CompareExchange(ref _current,
                init ? TrueValue : FalseValue,
                previous ? TrueValue : FalseValue) == TrueValue;
        }
    }
}