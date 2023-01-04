using System;
using NServiceBus.Persistence.Sql;

namespace OutboxPatternDemo.Subscriber.DuplicateCheckers;

public interface IDuplicateChecker
{
    bool IsDuplicate(Guid stateDetailsId);
    bool IsDuplicateTransactional(Guid stateDetailsId, ISqlStorageSession sqlStorageSession);
}
