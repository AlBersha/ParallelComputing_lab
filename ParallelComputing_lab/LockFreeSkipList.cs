using System.Net.Http;
using ParallelComputing_lab.Helper;
using ParallelComputing_lab.Node;

namespace ParallelComputing_lab
{
    public class LockFreeSkipList<T>
    {
        public Node<T> Top { get; set; }
        public Node<T> Tail { get; set; }

        public LockFreeSkipList()
        {
            for (var i = 0; i < Top.Next.Length; i++)
            {
                Top.Next[i] = new MarkedReference<Node<T>>(Tail, false);
            }
        }

        public bool Add(Node<T> node)
        {
            var pred = new Node<T>[Settings.LevelMax + 1];
            var success = new Node<T>[Settings.LevelMax + 1];

            while (true)
            {
                if (Find(node, ref pred, ref success))
                {
                    return false;
                }

                var top = node.Top;

                for (var i = Settings.LevelMin; i <= top; i++)
                {
                    var tmp = success[i];
                    node.Next[Settings.LevelMin] = new MarkedReference<Node<T>>(tmp, false);
                }

                var predNode = pred[Settings.LevelMin];
                var succNode = success[Settings.LevelMin];

                node.Next[Settings.LevelMin] = new MarkedReference<Node<T>>(succNode, false);

                if (!predNode.Next[Settings.LevelMin].CompareAndSet(node, false, succNode, false))
                {
                    continue;
                }

                for (var i = 1; i < top; i++)
                {
                    while (true)
                    {
                        predNode = pred[i];
                        succNode = success[i];

                        if (predNode.Next[i].CompareAndSet(node, false, succNode, false))
                        {
                            break;
                        }

                        Find(node, ref pred, ref success);
                    }
                }
            }
        }

        private bool Find(Node<T> node, ref Node<T>[] preds, ref Node<T>[] success)
        {
            var flag = false;
            var isRetryNeeded = false;
            Node<T> current = null;

            while (true)
            {
                var previous = Top;
                for (var i = Settings.LevelMax; i >= Settings.LevelMin; i--)
                {
                    current = previous.Next[i].Value;
                    while (true)
                    {
                        var s = current.Next[i].Get(ref flag);
                        while (flag)
                        {
                            var sn = previous.Next[i].CompareAndSet(s, false, current, false);
                            if (!sn)
                            {
                                isRetryNeeded = true;
                                break;
                            }

                            current = previous.Next[i].Value;
                            s = current.Next[i].Get(ref flag);
                        }

                        if (isRetryNeeded)
                        {
                            break;
                        }

                        if (current.Key < node.Key)
                        {
                            previous = current;
                            current = s;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (isRetryNeeded)
                    {
                        isRetryNeeded = false;
                        continue;
                    }

                    preds[i] = previous;
                    success[i] = current;
                }

                return current != null && current.Key == node.Key;
            }
        }

        public bool Remove(Node<T> node)
        {
            var previous = new Node<T>[Settings.LevelMax + 1];
            var successes = new Node<T>[Settings.LevelMax + 1];

            while (true)
            {
                var found = Find(node, ref previous, ref successes);
                if (!found)
                {
                    return false;
                }

                Node<T> success;

                for (var i = node.Top; i > Settings.LevelMin; i--)
                {
                    var flag = false;
                    success = node.Next[i].Get(ref flag);

                    while (!flag)
                    {
                        node.Next[i].CompareAndSet(success, true, success, false);
                        success = node.Next[i].Get(ref flag);
                    }
                }

                var mark = false;
                success = node.Next[Settings.LevelMin].Get(ref mark);

                while (true)
                {
                    var isMarked = node.Next[Settings.LevelMin].CompareAndSet(success, true, success, false);
                    success = successes[Settings.LevelMin].Next[Settings.LevelMin].Get(ref mark);

                    if (isMarked)
                    {
                        Find(node, ref previous, ref successes);
                        return true;
                    }

                    if (mark)
                    {
                        return false;
                    }
                }
            }
        }
    }
}