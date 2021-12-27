using System.Collections.Generic;
using System.Threading;
using ParallelComputing_lab.Helper;

namespace ParallelComputing_lab
{
    public class MutexCAS
    {
        private readonly AtomicReference<Thread> _lockedThread = new(null);
        private readonly SynchronizedCollection<Thread> _synchronizedThreads = new();
        
        public void Wait()
        {
            var current = Thread.CurrentThread;

            if (_lockedThread.Value != Thread.CurrentThread)
            {
                throw new ThreadStateException();
            }
            
            _synchronizedThreads.Add(current);
            Unlock();

            while (_synchronizedThreads.Contains(current))
            {
                Thread.Yield();
            }
            
            Lock();
        }

        public void Notify()
        {
            var current = Thread.CurrentThread;
            if (_lockedThread.Value != current)
            {
                throw new ThreadStateException();
            }

            _synchronizedThreads.Remove(current);
        }

        public void NotifyAll()
        {
            if (_lockedThread.Value != Thread.CurrentThread)
            {
                throw new ThreadStateException();
            }
            
            _synchronizedThreads.Clear();
        }
        
        private void Lock()
        {
            while (!_lockedThread.CompareAndSet(null, Thread.CurrentThread))
            {
                Thread.Yield();
            }
        }

        private void Unlock()
        {
            _lockedThread.CompareAndSet(_lockedThread.Value, null);
        }
    }
}