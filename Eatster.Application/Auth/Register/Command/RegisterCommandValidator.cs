using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eatster.Application.Auth.Register.Command
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(rv => rv.FirstName).NotEmpty().NotNull();
            RuleFor(rv => rv.LastName).NotEmpty().NotNull();
            RuleFor(rv => rv.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(rv => rv.Password).NotEmpty().NotNull();
            RuleFor(rv => rv.UserName).NotEmpty().NotNull().MinimumLength(6);
        }
    }
}
