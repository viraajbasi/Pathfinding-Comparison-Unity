using System;
using System.Collections.Generic;
using System.Linq;

namespace Maze
{
    public class PriorityQueue<T>
    {
        private class Node
        {
            public int Priority { get; set; }
            public T Object { get; }

            public Node(int priority, T obj)
            {
                Priority = priority;
                Object = obj;
            }
        }

        public int Count => _queue.Count;

        private List<Node> _queue = new();
        private int _heapSize = -1;
        private bool _isMinPriorityQueue;

        public PriorityQueue(bool isMinPriorityQueue = false)
        {
            _isMinPriorityQueue = isMinPriorityQueue;
        }

        public void Enqueue(T obj, int priority)
        {
            var node = new Node(priority, obj);

            _queue.Add(node);

            _heapSize++;

            if (_isMinPriorityQueue)
            {
                BuildHeapMin(_heapSize);
            }
            else
            {
                BuildHeapMax(_heapSize);
            }
        }

        public T Dequeue()
        {
            if (_heapSize > -1)
            {
                var returnVal = _queue[0].Object;
                _queue[0] = _queue[_heapSize];
                _queue.RemoveAt(_heapSize);
                _heapSize--;

                if (_isMinPriorityQueue)
                {
                    MinHeapify(0);
                }
                else
                {
                    MaxHeapify(0);
                }

                return returnVal;
            }
            else
            {
                throw new Exception("Queue is empty");
            }
        }

        public void UpdatePriority(T obj, int priority)
        {
            for (int i = 0; i <= _heapSize; i++)
            {
                var node = _queue[i];

                if (ReferenceEquals(node.Object, obj))
                {
                    node.Priority = priority;

                    if (_isMinPriorityQueue)
                    {
                        BuildHeapMin(i);
                        MinHeapify(i);
                    }
                    else
                    {
                        BuildHeapMax(i);
                        MaxHeapify(i);
                    }
                }
            }
        }

        public bool IsInQueue(T obj)
        {
            return _queue.Any(node => ReferenceEquals(node.Object, obj));
        }

        private void BuildHeapMax(int i)
        {
            while (i >= 0 && _queue[(i - 1) / 2].Priority < _queue[i].Priority)
            {
                Swap(i, (i - 1) / 2);
                i = (i - 1) / 2;
            }
        }

        private void BuildHeapMin(int i)
        {
            while (i < 0 && _queue[(i - 1) / 2].Priority > _queue[i].Priority)
            {
                Swap(i, (i - 1) / 2);
                i = (i - 1) / 2;
            }
        }

        private void MaxHeapify(int i)
        {
            var left = ChildL(i);
            var right = ChildR(i);
            var highest = i;

            if (left <= _heapSize && _queue[highest].Priority < _queue[left].Priority)
            {
                highest = left;
            }

            if (right <= _heapSize && _queue[highest].Priority < _queue[right].Priority)
            {
                highest = right;
            }

            if (highest != i)
            {
                Swap(highest, i);
                MaxHeapify(highest);
            }
        }

        private void MinHeapify(int i)
        {
            var left = ChildL(i);
            var right = ChildR(i);
            var lowest = i;

            if (left <= _heapSize && _queue[lowest].Priority > _queue[left].Priority)
            {
                lowest = left;
            }

            if (right <= _heapSize && _queue[lowest].Priority > _queue[right].Priority)
            {
                lowest = right;
            }

            if (lowest != i)
            {
                Swap(lowest, i);
                MinHeapify(lowest);
            }
        }

        private void Swap(int i, int j)
        {
            (_queue[i], _queue[j]) = (_queue[j], _queue[i]);
        }

        private int ChildL(int i)
        {
            return i * 2 + 1;
        }

        private int ChildR(int i)
        {
            return i * 2 + 2;
        }
    }
}
