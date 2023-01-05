using System;

namespace OutboxPatternDemo.Bookings.DuplicateCheckers;

public interface IDuplicateChecker
{
    bool IsDuplicate(Guid id);
}
