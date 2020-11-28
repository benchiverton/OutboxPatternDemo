using System;
using System.Threading;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using OutboxPatternDemo.Subscriber.DuplicateCheckers;
using Xunit;

namespace OutboxPatternDemo.Subscriber.Tests.DuplicateCheckers
{
    public class CircularBufferDuplicateCheckerShould
    {
        [Fact]
        public void MarkDuplicateAsDuplicate()
        {
            var circularBuffer = new ConcurrentCurcularBuffer<int>(10);
            var duplicateChecker = new CircularBufferDuplicateChecker(circularBuffer);

            duplicateChecker.IsDuplicate(12345);
            var isDuplicate = duplicateChecker.IsDuplicate(12345);

            Assert.True(isDuplicate);
        }

        [Fact]
        public void NotMarkNewAsDuplicate()
        {
            var circularBuffer = new ConcurrentCurcularBuffer<int>(10);
            var duplicateChecker = new CircularBufferDuplicateChecker(circularBuffer);

            duplicateChecker.IsDuplicate(1);
            duplicateChecker.IsDuplicate(2);
            duplicateChecker.IsDuplicate(3);
            duplicateChecker.IsDuplicate(4);
            var isDuplicate = duplicateChecker.IsDuplicate(5);

            Assert.False(isDuplicate);
        }

        [Fact]
        public void NotMarkDuplicateAsDuplicateAfterRecordPurged()
        {
            var circularBuffer = new ConcurrentCurcularBuffer<int>(5);
            var duplicateChecker = new CircularBufferDuplicateChecker(circularBuffer);

            duplicateChecker.IsDuplicate(1);
            duplicateChecker.IsDuplicate(2);
            duplicateChecker.IsDuplicate(3);
            duplicateChecker.IsDuplicate(4);
            duplicateChecker.IsDuplicate(5);
            duplicateChecker.IsDuplicate(6);
            var isDuplicate = duplicateChecker.IsDuplicate(1);

            Assert.False(isDuplicate);
        }
    }
}
