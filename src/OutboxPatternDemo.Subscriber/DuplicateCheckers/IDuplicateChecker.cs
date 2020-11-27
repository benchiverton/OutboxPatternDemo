using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Subscriber.DuplicateCheckers
{
    public interface IDuplicateChecker
    {
        bool IsDuplicate(int stateDetailsId);
    }
}
