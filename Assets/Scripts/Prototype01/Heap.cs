using System;

namespace Prototype01
{
    public class Heap<T>
    {
        private int _maxElementCount;

        public Heap(int maxElementCount)
        {
            _maxElementCount = maxElementCount;
        }

        public void AddItem()
        {
            
        }
    }

    public interface IHeapItem : IComparable
    {
        
    }
}