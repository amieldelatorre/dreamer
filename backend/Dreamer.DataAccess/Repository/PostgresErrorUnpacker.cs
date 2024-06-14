using Dreamer.DataAccess.Constants;
using Npgsql;
using System.Diagnostics;

namespace Dreamer.DataAccess.Repository
{
    public class PostgresErrorUnpacker : ISqlErrorUnpacker
    {
        public SqlConstrainViolations GetSqlErrorType(Microsoft.EntityFrameworkCore.DbUpdateException ex)
        {
            if (ex.InnerException == null || ex.InnerException is not PostgresException)
                return SqlConstrainViolations.Unknown;

            var pgEx = ex.InnerException as PostgresException;
            Debug.Assert(pgEx != null);

            if (!(pgEx.SqlState == Npgsql.PostgresErrorCodes.UniqueViolation))
                return SqlConstrainViolations.Unknown;

            switch (pgEx.ConstraintName)
            {
                case IndexNames.UserEmailUnique:
                    return SqlConstrainViolations.UserEmailUnique;
                default:
                    return SqlConstrainViolations.Unknown;
            }
        }
    }
}
