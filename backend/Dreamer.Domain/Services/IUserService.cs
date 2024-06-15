using Dreamer.DataAccess.Models;
using Dreamer.Domain.DTOs;

namespace Dreamer.Domain.Services
{
    public interface IUserService
    {
        Task<Result<UserView>> Create(UserCreate userCreateObj);
        Task<bool> IsEmailUnique(string email);
    }
}
