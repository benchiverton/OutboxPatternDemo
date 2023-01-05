using System;
using Microsoft.EntityFrameworkCore;
using NServiceBus.Persistence.Sql;
using OutboxPatternDemo.Bookings.DuplicateCheckers.Sql.Data;

namespace OutboxPatternDemo.Bookings.DuplicateCheckers.Sql;

public class SqlDuplicateChecker : IDuplicateChecker, ITransactionalDuplicateChecker
{
    private readonly DuplicateKeyContext _context;

    /// <summary>
    /// Returns true when record with the same Id exists in the database
    /// </summary>
    /// <param name="context"></param>
    public SqlDuplicateChecker(DuplicateKeyContext context) => _context = context;

    public bool IsDuplicate(Guid id)
    {
        try
        {
            _context.DuplicateKeys.Add(new DuplicateKey(id.ToString()));
            _context.SaveChanges();
            return false;
        }
        // TODO: only catch duplicate key exceptions
        catch (DbUpdateException ex)
        {
            return true;
        }
    }

    public bool IsDuplicateTransactional(Guid id, ISqlStorageSession sqlStorageSession)
    {
        try
        {
            _context.Database.SetDbConnection(sqlStorageSession.Connection);
            _context.Database.UseTransaction(sqlStorageSession.Transaction);

            _context.DuplicateKeys.Add(new DuplicateKey(id.ToString()));
            _context.SaveChanges();

            return false;
        }
        // TODO: only catch duplicate key exceptions
        catch (DbUpdateException ex)
        {
            return true;
        }
    }
}
