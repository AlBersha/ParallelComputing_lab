using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ParallelComputing_lab.Nodes;
using Xunit;

namespace ParallelComputing_lab.Tests
{
    public class SkipListTests
    {
        private LockFreeSkipList<int> _skipList;
        private SynchronizedCollection<int> _pushedValue;
        private SynchronizedCollection<int> _popedValue;
        private ConcurrentStack<Node<int>> _nodes;

        private Setup _setup;
        private Random _random;
        private readonly object randomLock = new();

        public SkipListTests()
        {
            _skipList = new LockFreeSkipList<int>();
            _pushedValue = new SynchronizedCollection<int>();
            _popedValue = new SynchronizedCollection<int>();
            _nodes = new ConcurrentStack<Node<int>>();
            
            _setup = new Setup();
            _random = new Random();
        }

        private void AddToCollection(object? obj)
        {
            int key;
            lock (randomLock)
            {
                key = _random.Next(0, 10000);
            }

            var node = new Node<int>((int)obj!, key);
            var added = _skipList.Add(node);

            if (!added) return;
            _nodes.Push(node);
            _pushedValue.Add(node.Value);
        }

        private void RemoveFromCollection(object? obj)
        {
            if (!_nodes.TryPop(out var result)) return;
            if (_skipList.Remove(result)) _popedValue.Add(result.Value);
        }

        [Fact]
        public void LockFreeSkipListPerformance()
        {
            _setup.Run(AddToCollection, 10);
            _setup.Run(RemoveFromCollection, 10);
            
            Assert.Equal(_pushedValue.OrderBy(x => x), _popedValue.OrderBy(x => x));
        }
    }
}