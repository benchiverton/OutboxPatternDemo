using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OutboxPatternDemo.Subscriber.DuplicateCheckers.Sql;
using OutboxPatternDemo.Subscriber.DuplicateCheckers.Sql.Data;
using Xunit;

namespace OutboxPatternDemo.Subscriber.Tests.DuplicateCheckers;

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
        var duplicateId = Guid.NewGuid();

        duplicateChecker.IsDuplicate(duplicateId);
        var isDuplicate = duplicateChecker.IsDuplicate(duplicateId);

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

        duplicateChecker.IsDuplicate(Guid.NewGuid());
        duplicateChecker.IsDuplicate(Guid.NewGuid());
        duplicateChecker.IsDuplicate(Guid.NewGuid());
        duplicateChecker.IsDuplicate(Guid.NewGuid());
        var isDuplicate = duplicateChecker.IsDuplicate(Guid.NewGuid());

        Assert.False(isDuplicate);
    }
}
