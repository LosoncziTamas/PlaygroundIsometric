using System;
using UnityEngine;

namespace Prototype01
{
    public class Heap<T> where T : IHeapItem<T>
    {
        private readonly T[] _elements;
        private int _currentItemIndex;

        public int Count => _currentItemIndex;

        public Heap(int maxElementCount)
        {
            _elements = new T[maxElementCount];
            _currentItemIndex = 0;
        }

        private static int GetParentIndex(int index)
        {
            return (index - 1) / 2;
        }

        private static int GetLeftChildIndex(int index)
        {
            return (2 * index) + 1;
        }

        private static int GetRightChildIndex(int index)
        {
            return (2 * index) + 2;
        }

        public void UpdateItem(T item)
        {
            // We only increase the priority, so there is no need to call SortDown
            SortUp(item);
        }

        public void AddItem(T item)
        {
            Debug.Assert(_elements.Length > _currentItemIndex + 1);
            item.HeapIndex = _currentItemIndex;
            _elements[_currentItemIndex] = item;
            SortUp(item);
            _currentItemIndex++;
        }

        public T RemoveFirst()
        {
            if (_currentItemIndex > 0)
            {
                var removedFirstItem = _elements[0];
                Debug.Assert(removedFirstItem.HeapIndex == 0);
                _currentItemIndex--;
                
                var lastItem = _elements[_currentItemIndex];
                _elements[0] = lastItem;
                lastItem.HeapIndex = 0;
                SortDown(lastItem);

                return removedFirstItem;
            }

            return default;
        }
        
        public bool Contains(T item)
        {
            return Equals(item, _elements[item.HeapIndex]);
        }

        private void SortDown(T item)
        {
            while (true)
            {
                var leftChildIdx = GetLeftChildIndex(item.HeapIndex);
                var rightChildIdx = GetRightChildIndex(item.HeapIndex);
                
                var hasLeftChild = leftChildIdx < _currentItemIndex;
                if (hasLeftChild)
                {
                    var swapIdx = leftChildIdx;
                    var hasRightChild = rightChildIdx < _currentItemIndex;
                    if (hasRightChild)
                    {
                        if (BetterFit(_elements[rightChildIdx], _elements[leftChildIdx]))
                        {
                            swapIdx = rightChildIdx;
                        }
                    }

                    var swapCandidate = _elements[swapIdx];
                    if (BetterFit(swapCandidate, item))
                    {
                        Swap(swapCandidate, item);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private bool BetterFit(T a, T b)
        {
            return _elements[b.HeapIndex].CompareTo(_elements[a.HeapIndex]) < 0;
        }

        private void SortUp(T item)
        {
            var parentIndex = GetParentIndex(item.HeapIndex);
            var parentItem = _elements[parentIndex];
            
            while (true)
            {
                if (item.CompareTo(parentItem) > 0)
                {
                    Swap(item, parentItem);
                    parentIndex = GetParentIndex(item.HeapIndex);
                    parentItem = _elements[parentIndex];
                }
                else
                {
                    break;
                }
            }
        }

        private void Swap(T itemA, T itemB)
        {
            _elements[itemA.HeapIndex] = itemB;
            _elements[itemB.HeapIndex] = itemA;
            var temp = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = temp;
        }
    }

    public interface IHeapItem<in T> : IComparable<T>
    {
        int HeapIndex { get; set; }
    }
}