using System;
using NServiceBus.Persistence.Sql;

namespace OutboxPatternDemo.Subscriber.DuplicateCheckers;

public interface ITransactionalDuplicateChecker
{
    bool IsDuplicateTransactional(Guid id, ISqlStorageSession sqlStorageSession);
}
