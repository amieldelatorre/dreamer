using Dreamer.Domain.DTOs;
using Dreamer.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Dreamer.Domain.Validators;

public class UserCreateValidator : AbstractValidator<UserCreate>
{
    private const int PasswordMinimumLength = 8;

    public UserCreateValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.LastName)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .NotNull()
            .NotEmpty()
            .MinimumLength(PasswordMinimumLength);
    }
}

