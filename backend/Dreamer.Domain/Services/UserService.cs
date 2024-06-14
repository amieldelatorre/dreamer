using Dreamer.Domain.DTOs;
using Dreamer.DataAccess.Repository;
using Dreamer.DataAccess.Models;
using Dreamer.Domain.Validators;
using Dreamer.Shared.Constants;
using Dreamer.Cache;
using Serilog;

namespace Dreamer.Domain.Services
{
    public class UserService(
        IUserCache userCache,
        ISqlErrorUnpacker sqlErrorUnpacker,
        ILogger logger)
        : IUserService
    {

        public async Task<Result<UserView>> Create(UserCreate userCreateObj)
        {
            logger.Debug("{featureName} starting", FeatureName.UserCreate);
            var result = UserCreateValidator.Check(userCreateObj);
            if (result.Errors.Count > 0)
            {
                result.RequestResultStatus = RequestResultStatusTypes.UserError;
                return result;
            }

            var newUser = NewUserFromUserCreate(userCreateObj);

            try
            {
                await userCache.Create(newUser);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                var sqlConstraintViolation = sqlErrorUnpacker.GetSqlErrorType(ex);
                if (sqlConstraintViolation == DataAccess.Constants.SqlConstrainViolations.UserEmailUnique)
                {
                    result.RequestResultStatus = RequestResultStatusTypes.UserError;
                    result.AddError(nameof(userCreateObj.Email), "Email already exists");
                    return result;
                }

                logger.Error("{featureName}: unhandled {exception}", FeatureName.UserCreate, ex.Message);
                return ServerErrors<UserView>.GetInternalServerErrorResult(result);
            }
            catch (Exception ex)
            {
                ServerErrors<UserView>.LogException(logger, FeatureName.UserGetByEmail, ex);
                return ServerErrors<UserView>.GetInternalServerErrorResult(result);
            }

            logger.Debug("{featureName}: processed with new Id '{userId}'.",
                FeatureName.UserCreate,
                newUser.Id);
            result.RequestResultStatus = RequestResultStatusTypes.Created;
            result.Item = UserViewFromUser(newUser);
            return result;
        }

        public async Task<bool> EmailExists(string email)
        {
            try
            {
                var user = await userCache.GetUserByEmail(email);
                return user != null;
            }
            catch (Exception ex)
            {
                ServerErrors<UserView>.LogException(logger, FeatureName.UserGetByEmail, ex);
                return false;
            }

        }

        private static User NewUserFromUserCreate(UserCreate userCreateObj)
        {
            return new User()
            {
                Id = Guid.NewGuid(),
                FirstName = userCreateObj.FirstName.Trim(),
                LastName = userCreateObj.LastName.Trim(),
                Email = userCreateObj.Email.Trim(),
                Password = BCrypt.Net.BCrypt.HashPassword(userCreateObj.Password),
                DateCreated = DateTime.Now.ToUniversalTime(),
                DateModified = DateTime.Now.ToUniversalTime(),
            };
        }

        private static UserView UserViewFromUser(User user)
        {
            return new UserView() 
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateCreated = user.DateCreated,
                DateModified = user.DateModified
            };
        }
    }
}
