using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelComputing_lab.Tests
{
    public class Setup
    {
        private readonly object _obj = new();
        private readonly Random _random = new();

        public void Run(Action<object> action, int count)
        {
            var threads = new List<Thread>();

            for (var i = 0; i < count; i++)
            {
                threads.Add(new Thread(new ParameterizedThreadStart(action!)));
            }

            Parallel.ForEach(threads, t =>
            {
                int value;
                lock (_obj)
                {
                    value = _random.Next(0, 10000);
                }

                t.Start(value);
            });

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }
}