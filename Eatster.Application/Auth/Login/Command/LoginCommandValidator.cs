using Eatster.Application.Login.Command;
using FluentValidation;

namespace Eatster.Application.Auth.Login.Command
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(v => v.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(v => v.Password).NotNull().NotEmpty();
            RuleFor(v => v.RemoteIpAddress).NotNull().NotEmpty();
        }
    }
}