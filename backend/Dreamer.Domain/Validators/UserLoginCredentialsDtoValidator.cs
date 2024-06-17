using Dreamer.Domain.DTOs;
using FluentValidation;

namespace Dreamer.Domain.Validators;

public class UserLoginCredentialsDtoValidator : AbstractValidator<UserLoginCredentialsDto>
{
    public UserLoginCredentialsDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty();
    }
}
