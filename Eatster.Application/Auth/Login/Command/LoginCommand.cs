using Eatster.Application.Login.Response;
using MediatR;

namespace Eatster.Application.Login.Command
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string RemoteIpAddress { get; set; }
    }
}