namespace ParallelComputing_lab.Helper
{
    public class MarkedReference<T>
    {
        private readonly AtomicReference<ReferenceBase<T>> _atomicReference;
        public T Value => _atomicReference.Value.Value;
        private AtomicBool Flag => _atomicReference.Value.Flag;

        public MarkedReference(T value, bool flag)
        {
            _atomicReference = new AtomicReference<ReferenceBase<T>>(new ReferenceBase<T>(value, flag));
        }

        public T Get(ref bool flag)
        {
            flag = Flag.Value;
            return Value;
        }

        public bool TryMark(T previousValue, bool flag)
        {
            return ReferenceEquals(_atomicReference.Value.Value, previousValue) && _atomicReference.Value.Flag.SetValue(flag);
        }

        public bool CompareAndSet(T init, bool initFlag, T previous, bool prevFlag)
        {
            var previousReference = _atomicReference.Value;

            if (!ReferenceEquals(previousReference.Value, previous))
            {
                return false;
            }
            return previousReference.Flag.Value == prevFlag && _atomicReference.CompareAndSet(new ReferenceBase<T>(init, initFlag), previousReference);
        }
    }
}