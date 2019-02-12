using Eatster.Application.Auth.Register.Response;
using MediatR;

namespace Eatster.Application.Auth.Register.Command
{
    public class RegisterCommand : IRequest<RegisterResponse>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string UserName { get; set; }
    }
}