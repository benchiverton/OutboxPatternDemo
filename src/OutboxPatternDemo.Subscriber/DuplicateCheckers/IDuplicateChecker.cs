namespace OutboxPatternDemo.Subscriber.DuplicateCheckers
{
    public interface IDuplicateChecker
    {
        bool IsDuplicate(int stateDetailsId);
    }
}
