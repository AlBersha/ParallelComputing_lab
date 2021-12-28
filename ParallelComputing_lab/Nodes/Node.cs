using System;
using ParallelComputing_lab.Helper;

namespace ParallelComputing_lab.Nodes
{
    public class Node<T>
    {
        private static uint _seed;
        public T Value { get; }
        public int Key { get; }
        public MarkedReference<Node<T>>[] Next { get; }
        public int Top { get; }

        public Node(T value, int key)
        {
            Value = value;
            Key = key;
            var height = Random();
            Next = new MarkedReference<Node<T>>[height + 1];
            for (var i = 0; i < Next.Length; i++)
            {
                Next[i] = new MarkedReference<Node<T>>(null, false);
            }

            Top = height;
        }
        
        private static int Random()
        {
            var x = _seed;
            x ^= x << 13;
            x ^= x >> 17;
            _seed = x ^= x << 5;
            if ((x & 0x80000001) != 0)
            {
                return 0;
            }

            var level = 1;
            while (((x >>= 1) & 1) != 0)
            {
                level++;
            }

            return Math.Min(level, Settings.LevelMax);
        }
        
        static Node()
        {
            _seed = (uint)DateTime.Now.Millisecond;
        }

        public Node(int key)
        {
            Key = key;

            Next = new MarkedReference<Node<T>>[Settings.LevelMax + 1];
            for (var i = 0; i < Next.Length; ++i)
            {
                Next[i] = new MarkedReference<Node<T>>(null, false);
            }

            Top = Settings.LevelMax;
        }
    }
}