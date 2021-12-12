using NServiceBus.Persistence.Sql;

namespace OutboxPatternDemo.Subscriber.DuplicateCheckers;

public interface IDuplicateChecker
{
    bool IsDuplicate(int stateDetailsId);
    bool IsDuplicateTransactional(int stateDetailsId, ISqlStorageSession sqlStorageSession);
}
