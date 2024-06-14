using Dreamer.DataAccess.Constants;

namespace Dreamer.DataAccess.Repository
{
    public interface ISqlErrorUnpacker
    {
        SqlConstrainViolations GetSqlErrorType(Microsoft.EntityFrameworkCore.DbUpdateException ex);
    }
}
