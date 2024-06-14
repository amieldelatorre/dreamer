using Dreamer.Domain.DTOs;
using Dreamer.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Dreamer.Domain.Validators
{
    public class UserCreateValidator
    {
        private const int _passwordMinimmumLength = 8;

        public static Result<UserView> Check(UserCreate userCreate)
        {
            var result = new Result<UserView>();
            string nullOrEmptyMessage = "Cannot be null or empty";
            string invalidEmailFormat = "Email format invalid";
            string passwordMinimumLengthMessage = $"Must be a string with a minimum length of {_passwordMinimmumLength}";

            if (String.IsNullOrWhiteSpace(userCreate.FirstName))
                result.AddError(nameof(userCreate.FirstName), nullOrEmptyMessage);

            if (String.IsNullOrWhiteSpace(userCreate.LastName))
                result.AddError(nameof(userCreate.LastName), nullOrEmptyMessage);

            if (String.IsNullOrWhiteSpace(userCreate.Email))
                result.AddError(nameof(userCreate.Email), nullOrEmptyMessage);
            
            try
            {
                _ = new MailAddress(userCreate.Email.Trim());
            }
            catch 
            {
                result.AddError(nameof(userCreate.Email), invalidEmailFormat);
            }

            if (String.IsNullOrWhiteSpace(userCreate.Password) || userCreate.Password.Trim().Length < _passwordMinimmumLength)
                result.AddError(nameof(userCreate.Password), passwordMinimumLengthMessage);

            return result;
        }
    }
}
