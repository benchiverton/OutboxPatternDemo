using System;
using System.Collections.Concurrent;
using System.Linq;

namespace OutboxPatternDemo.Subscriber.DuplicateCheckers.CircularBuffer;

public class CircularBufferDuplicateChecker : IDuplicateChecker
{
    private readonly ConcurrentCircularBuffer<Guid> _circularBuffer;

    /// <summary>
    /// Returns true when a duplicate is received within the last X records.
    /// </summary>
    /// <param name="circularBuffer"></param>
    public CircularBufferDuplicateChecker(ConcurrentCircularBuffer<Guid> circularBuffer) => _circularBuffer = circularBuffer;

    public bool IsDuplicate(Guid id)
    {
        if (_circularBuffer.Contains(id))
        {
            return true;
        }
        else
        {
            _circularBuffer.AddToBuffer(id);
            return false;
        }
    }
}

public class ConcurrentCircularBuffer<T>
{
    private readonly ConcurrentQueue<T> _queue;
    private readonly int _capacity;

    public ConcurrentCircularBuffer(int capacity)
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
