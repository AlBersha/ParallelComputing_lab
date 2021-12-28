using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ParallelComputing_lab.Tests
{
    public class MichaelScottQueueTests
    {
        private readonly MichaelScottQueue<int> _michaelScottQueue;
        private readonly SynchronizedCollection<int> _addedValues;
        private readonly SynchronizedCollection<int> _removedValues;
        private readonly Setup _setup;

        public MichaelScottQueueTests()
        {
            _michaelScottQueue = new MichaelScottQueue<int>();
            _addedValues = new SynchronizedCollection<int>();
            _removedValues = new SynchronizedCollection<int>();
            _setup = new Setup();
        }

        private void AddToCollection(object? obj)
        {
            _michaelScottQueue.Push((int)obj!);
            _addedValues.Add((int)obj);
        }

        private void RemoveFromCollection(object? obj)
        {
            _removedValues.Add(_michaelScottQueue.Pop());
        }

        [Fact]
        public void QueuePerformance()
        {
            _setup.Run(AddToCollection, 10);
            _setup.Run(RemoveFromCollection, 10);
            
            Assert.Equal(_addedValues.OrderBy(x => x), _removedValues.OrderBy(x => x));
        }
    }
}