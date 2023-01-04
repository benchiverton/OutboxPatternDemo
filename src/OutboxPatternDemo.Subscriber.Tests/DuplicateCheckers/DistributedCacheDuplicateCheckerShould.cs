using System;
using System.Threading;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OutboxPatternDemo.Subscriber.DuplicateCheckers.DistributedCache;
using OutboxPatternDemo.Subscriber.DuplicateCheckers.Sql;
using Xunit;

namespace OutboxPatternDemo.Subscriber.Tests.DuplicateCheckers;

public class DistributedCacheDuplicateCheckerShould
{
    [Fact]
    public void MarkDuplicateAsDuplicate()
    {
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var duplicateChecker = new DistributedCacheDuplicateChecker(distributedCache, TimeSpan.FromMinutes(10));
        var duplicateId = Guid.NewGuid();

        duplicateChecker.IsDuplicate(duplicateId);
        var isDuplicate = duplicateChecker.IsDuplicate(duplicateId);

        Assert.True(isDuplicate);
    }

    [Fact]
    public void NotMarkNewAsDuplicate()
    {
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var duplicateChecker = new DistributedCacheDuplicateChecker(distributedCache, TimeSpan.FromMinutes(10));

        duplicateChecker.IsDuplicate(Guid.NewGuid());
        duplicateChecker.IsDuplicate(Guid.NewGuid());
        duplicateChecker.IsDuplicate(Guid.NewGuid());
        duplicateChecker.IsDuplicate(Guid.NewGuid());
        var isDuplicate = duplicateChecker.IsDuplicate(Guid.NewGuid());

        Assert.False(isDuplicate);
    }

    [Fact]
    public void NotMarkDuplicateAsDuplicateAfterExpiration()
    {
        var distributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        var duplicateChecker = new DistributedCacheDuplicateChecker(distributedCache, TimeSpan.FromMilliseconds(10));
        var duplicateId = Guid.NewGuid();

        duplicateChecker.IsDuplicate(duplicateId);
        Thread.Sleep(11);
        var isDuplicate = duplicateChecker.IsDuplicate(duplicateId);

        Assert.False(isDuplicate);
    }
}
