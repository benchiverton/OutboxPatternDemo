using System;
using System.Collections.Concurrent;
using System.Linq;

namespace OutboxPatternDemo.Subscriber.DuplicateCheckers
{
    public class CircularBufferDuplicateChecker : IDuplicateChecker
    {
        private readonly ConcurrentCurcularBuffer<string> _circularBuffer;

        public CircularBufferDuplicateChecker(ConcurrentCurcularBuffer<string> curcularBuffer) => _circularBuffer = curcularBuffer;

        public bool IsDuplicate(int stateDetailsId)
        {
            if (_circularBuffer.Contains(stateDetailsId.ToString()))
            {
                return true;
            }
            else
            {
                _circularBuffer.AddToBuffer(stateDetailsId.ToString());
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
