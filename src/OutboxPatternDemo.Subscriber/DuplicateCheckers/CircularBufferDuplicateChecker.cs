using System;
using System.Collections.Concurrent;
using System.Linq;
using NServiceBus.Persistence.Sql;

namespace OutboxPatternDemo.Subscriber.DuplicateCheckers;

public class CircularBufferDuplicateChecker : IDuplicateChecker
{
    private readonly ConcurrentCircularBuffer<Guid> _circularBuffer;

    /// <summary>
    /// Returns true when a duplicate is received within the last X records.
    /// </summary>
    /// <param name="circularBuffer"></param>
    public CircularBufferDuplicateChecker(ConcurrentCircularBuffer<Guid> circularBuffer) => _circularBuffer = circularBuffer;

    public bool IsDuplicate(Guid stateDetailsId)
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

    // not implemented
    public bool IsDuplicateTransactional(Guid stateDetailsId, ISqlStorageSession sqlStorageSession) => false;
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
