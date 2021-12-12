using System;
using System.Threading;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OutboxPatternDemo.Subscriber.DuplicateCheckers;
using Xunit;

namespace OutboxPatternDemo.Subscriber.Tests.DuplicateCheckers;

public class DistributedCacheDuplicateCheckerShould
{
    [Fact]
    public void MarkDuplicateAsDuplicate()
    {
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var duplicateChecker = new DistributedCacheDuplicateChecker(distributedCache, TimeSpan.FromMinutes(10));

        duplicateChecker.IsDuplicate(12345);
        var isDuplicate = duplicateChecker.IsDuplicate(12345);

        Assert.True(isDuplicate);
    }

    [Fact]
    public void NotMarkNewAsDuplicate()
    {
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var duplicateChecker = new DistributedCacheDuplicateChecker(distributedCache, TimeSpan.FromMinutes(10));

        duplicateChecker.IsDuplicate(1);
        duplicateChecker.IsDuplicate(2);
        duplicateChecker.IsDuplicate(3);
        duplicateChecker.IsDuplicate(4);
        var isDuplicate = duplicateChecker.IsDuplicate(5);

        Assert.False(isDuplicate);
    }

    [Fact]
    public void NotMarkDuplicateAsDuplicateAfterExpiration()
    {
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var duplicateChecker = new DistributedCacheDuplicateChecker(distributedCache, TimeSpan.FromMilliseconds(10));

        duplicateChecker.IsDuplicate(12345);
        Thread.Sleep(11);
        var isDuplicate = duplicateChecker.IsDuplicate(12345);

        Assert.False(isDuplicate);
    }
}
