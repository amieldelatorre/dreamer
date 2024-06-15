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
        ILogger logger)
        : IUserService
    {

        public async Task<Result<UserView>> Create(UserCreate userCreateObj)
        {
            logger.Debug("{featureName} starting", FeatureName.UserCreate);
            var result = new Result<UserView>();
            var newUser = NewUserFromUserCreate(userCreateObj);

            if (!(await IsEmailUnique(newUser.Email)))
            {
                result.AddError(nameof(userCreateObj.Email), "Email already exists");
                result.RequestResultStatus = RequestResultStatusTypes.UserError;
                return result;
            }

            await userCache.Create(newUser);

            logger.Debug("{featureName}: processed with new Id '{userId}'.",
                FeatureName.UserCreate,
                newUser.Id);
            result.RequestResultStatus = RequestResultStatusTypes.Created;
            result.Item = UserViewFromUser(newUser);
            return result;
        }

        public async Task<bool> IsEmailUnique(string email)
        {
            var user = await userCache.GetUserByEmail(email);
            return user == null;
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
