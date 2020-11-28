using System;
using System.Collections.Concurrent;
using System.Linq;

namespace OutboxPatternDemo.Subscriber.DuplicateCheckers
{
    public class CircularBufferDuplicateChecker : IDuplicateChecker
    {
        private readonly ConcurrentCurcularBuffer<int> _circularBuffer;

        /// <summary>
        /// Returns true when a duplicate is received within the last X records
        /// </summary>
        /// <param name="curcularBuffer"></param>
        public CircularBufferDuplicateChecker(ConcurrentCurcularBuffer<int> curcularBuffer) => _circularBuffer = curcularBuffer;

        public bool IsDuplicate(int stateDetailsId)
        {
            if (_circularBuffer.Contains(stateDetailsId))
            {
                return true;
            }
            else
            {
                _circularBuffer.AddToBuffer(stateDetailsId);
                return false;
            }
        }
    }

    public class ConcurrentCurcularBuffer<T>
    {
        private readonly ConcurrentQueue<T> _queue;
        private readonly int _capacity;

        public ConcurrentCurcularBuffer(int capacity)
        {
            _queue = new ConcurrentQueue<T>();
            _capacity = capacity;
        }

        public bool Contains(T item) => _queue.Contains(item);

        public void AddToBuffer(T item)
        {
            while (_queue.Count >= _capacity)
            {
                _queue.TryDequeue(out _);
            }
            _queue.Enqueue(item);
        }
    }
}
