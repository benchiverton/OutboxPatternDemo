using System;
using NServiceBus.Persistence.Sql;

namespace OutboxPatternDemo.Bookings.DuplicateCheckers;

public interface ITransactionalDuplicateChecker
{
    bool IsDuplicateTransactional(Guid id, ISqlStorageSession sqlStorageSession);
}
