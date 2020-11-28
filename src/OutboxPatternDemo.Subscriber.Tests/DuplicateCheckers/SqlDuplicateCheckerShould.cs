using System;
using System.Threading;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using OutboxPatternDemo.Subscriber.Data;
using OutboxPatternDemo.Subscriber.DuplicateCheckers;
using Xunit;

namespace OutboxPatternDemo.Subscriber.Tests.DuplicateCheckers
{
    public class SqlDuplicateCheckerShould
    {
        [Fact]
        public void MarkDuplicateAsDuplicate()
        {
            using var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var optionsBuilder = new DbContextOptionsBuilder<DuplicateKeyContext>();
            optionsBuilder.UseSqlite(connection);
            var context = new DuplicateKeyContext(optionsBuilder.Options);
            var duplicateChecker = new SqlDuplicateChecker(context);

            duplicateChecker.IsDuplicate(12345);
            var isDuplicate = duplicateChecker.IsDuplicate(12345);

            Assert.True(isDuplicate);
        }

        [Fact]
        public void NotMarkNewAsDuplicate()
        {
            using var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var optionsBuilder = new DbContextOptionsBuilder<DuplicateKeyContext>();
            optionsBuilder.UseSqlite(connection);
            var context = new DuplicateKeyContext(optionsBuilder.Options);
            var duplicateChecker = new SqlDuplicateChecker(context);

            duplicateChecker.IsDuplicate(1);
            duplicateChecker.IsDuplicate(2);
            duplicateChecker.IsDuplicate(3);
            duplicateChecker.IsDuplicate(4);
            var isDuplicate = duplicateChecker.IsDuplicate(5);

            Assert.False(isDuplicate);
        }
    }
}
