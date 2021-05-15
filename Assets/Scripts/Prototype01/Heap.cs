using System;
using UnityEngine;

namespace Prototype01
{
    public class Heap<T> where T : IHeapItem<T>
    {
        private int _currentItemIndex;
        private T[] _elements;

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

        public void AddItem(T item)
        {
            Debug.Assert(_elements.Length > _currentItemIndex + 1);
            item.HeapIndex = _currentItemIndex;
            _elements[_currentItemIndex] = item;
            SortUp(item);
            _currentItemIndex++;
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