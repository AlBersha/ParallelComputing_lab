using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ParallelComputing_lab.Tests
{
    public class BlockFreeLinkedListTests
    {
        private readonly BlockFreeLinkedList<int> _linkedList;
        private readonly SynchronizedCollection<int> _pushedValue;
        private readonly SynchronizedCollection<int> _popedValue;
        private readonly ConcurrentStack<int> _pending;
        private readonly Setup _setup;

        public BlockFreeLinkedListTests()
        {
            _linkedList = new BlockFreeLinkedList<int>();
            _pushedValue = new SynchronizedCollection<int>();
            _popedValue = new SynchronizedCollection<int>();
            _pending = new ConcurrentStack<int>();
            _setup = new Setup();
        }

        private void AddToCollection(object? obj)
        {
            _linkedList.Add((int)obj!);
            _pending.Push((int)obj);
            _pushedValue.Add((int)obj);
        }

        private void RemoveFromCollection(object? obj)
        {
            if (!_pending.TryPop(out var item)) return;
            if (_linkedList.Remove(item))
            {
                _popedValue.Add(item);
            }
        }

        [Fact]
        public void BlockFreeLinkedListPerformance()
        {
            _setup.Run(AddToCollection, 1);
            _setup.Run(RemoveFromCollection, 1);

            Assert.Equal(_pushedValue.OrderBy(x => x), _popedValue.OrderBy(x => x));
        }
    }
}