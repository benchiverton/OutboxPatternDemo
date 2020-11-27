using System;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;

namespace OutboxPatternDemo.Subscriber.DuplicateCheckers
{
    public class TtlDistributedCacheDuplicateChecker : IDuplicateChecker
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _cacheOptions;

        /// <summary>
        /// Duplicate checker that verifies IsDuplicate has not been called with the given Id within the configured timespan
        /// </summary>
        public TtlDistributedCacheDuplicateChecker(IDistributedCache cache, TimeSpan slidingExpiration)
        {
            _cache = cache;
            _cacheOptions = new DistributedCacheEntryOptions
            {
                SlidingExpiration = slidingExpiration
            };
        }

        public bool IsDuplicate(int stateDetailsId)
        {
            if (_cache.Get(stateDetailsId.ToString()) == null)
            {
                var currentTimeUTC = DateTime.UtcNow;
                _cache.Set(stateDetailsId.ToString(), Encoding.UTF8.GetBytes(currentTimeUTC.ToString()), _cacheOptions);
                return false;
            }

            return true;
        }
    }
}
