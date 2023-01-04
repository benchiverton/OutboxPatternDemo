using System;
using OutboxPatternDemo.Subscriber.DuplicateCheckers.CircularBuffer;
using OutboxPatternDemo.Subscriber.DuplicateCheckers.Sql;
using Xunit;

namespace OutboxPatternDemo.Subscriber.Tests.DuplicateCheckers;

public class CircularBufferDuplicateCheckerShould
{
    [Fact]
    public void MarkDuplicateAsDuplicate()
    {
        var circularBuffer = new ConcurrentCircularBuffer<Guid>(10);
        var duplicateChecker = new CircularBufferDuplicateChecker(circularBuffer);
        var duplicateId = Guid.NewGuid();

        duplicateChecker.IsDuplicate(duplicateId);
        var isDuplicate = duplicateChecker.IsDuplicate(duplicateId);

        Assert.True(isDuplicate);
    }

    [Fact]
    public void NotMarkNewAsDuplicate()
    {
        var circularBuffer = new ConcurrentCircularBuffer<Guid>(10);
        var duplicateChecker = new CircularBufferDuplicateChecker(circularBuffer);

        duplicateChecker.IsDuplicate(Guid.NewGuid());
        duplicateChecker.IsDuplicate(Guid.NewGuid());
        duplicateChecker.IsDuplicate(Guid.NewGuid());
        duplicateChecker.IsDuplicate(Guid.NewGuid());
        var isDuplicate = duplicateChecker.IsDuplicate(Guid.NewGuid());

        Assert.False(isDuplicate);
    }

    [Fact]
    public void NotMarkDuplicateAsDuplicateAfterRecordPurged()
    {
        var circularBuffer = new ConcurrentCircularBuffer<Guid>(5);
        var duplicateChecker = new CircularBufferDuplicateChecker(circularBuffer);
        var duplicateId = Guid.NewGuid();

        duplicateChecker.IsDuplicate(duplicateId);
        duplicateChecker.IsDuplicate(Guid.NewGuid());
        duplicateChecker.IsDuplicate(Guid.NewGuid());
        duplicateChecker.IsDuplicate(Guid.NewGuid());
        duplicateChecker.IsDuplicate(Guid.NewGuid());
        duplicateChecker.IsDuplicate(Guid.NewGuid()); // record purged after this duplicate checked
        var isDuplicate = duplicateChecker.IsDuplicate(duplicateId);

        Assert.False(isDuplicate);
    }
}
