using Microsoft.EntityFrameworkCore;
using OutboxPatternDemo.Subscriber.Data;

namespace OutboxPatternDemo.Subscriber.DuplicateCheckers
{
    public class SqlDuplicateChecker : IDuplicateChecker
    {
        private readonly DuplicateKeyContext _context;

        /// <summary>
        /// Returns true when record with the same Id exists in the database
        /// </summary>
        /// <param name="context"></param>
        public SqlDuplicateChecker(DuplicateKeyContext context) => _context = context;

        public bool IsDuplicate(int stateDetailsId)
        {
            try
            {
                _context.DuplicateKeys.Add(new DuplicateKey(stateDetailsId.ToString()));
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
}
