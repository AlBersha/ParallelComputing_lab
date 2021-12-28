using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ParallelComputing_lab.Tests
{
    public class CompareAndSetMutexTests
    {
        private const int MaxCapasity = 3;
        private readonly MutexCAS _mutexCas;
        private readonly Queue<int> _nums;
        private readonly Random _random;

        public CompareAndSetMutexTests()
        {
            _mutexCas = new MutexCAS();
            _nums = new Queue<int>();
            _random = new Random();
        }
        
        private void Enqueue(object? obj)
        {
            _mutexCas.Lock();

            while (_nums.Count >= MaxCapasity)
            {
                _mutexCas.Wait();
            }
            
            _nums.Enqueue((int)(obj ?? throw new ArgumentNullException(nameof(obj))));
            _mutexCas.NotifyAll();
            
            Thread.Sleep(500);

            _mutexCas.Unlock();
        }

        private void Dequeue(object? obj)
        {
            _mutexCas.Lock();

            while (_nums.Count == 0)
            {
                _mutexCas.Wait();
            }

            _nums.Dequeue();
            _mutexCas.NotifyAll();
            
            Thread.Sleep(1500);
            
            _mutexCas.Unlock();
        }
        
        [Fact]
        public void CompareAndSetMutexTestPerformance()
        {
            var threads = new List<Thread>
            {
                new(Enqueue),
                new(Enqueue),
                new(Dequeue),
                new(Enqueue),
                new(Dequeue),
                new(Enqueue),
                new(Dequeue),
                new(Dequeue),
                new(Dequeue),
                new(Enqueue)
            };

            Parallel.ForEach(threads, t =>
            {
                t.Start(_random.Next(0, 10));
            });

            foreach (var thread in threads)
            {
                thread.Join();
            }
            
            Assert.Empty(_nums);
        }
    }
}

