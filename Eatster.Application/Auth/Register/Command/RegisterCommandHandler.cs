using Eatster.Application.Auth.Register.Response;
using Eatster.Domain.Entities;
using Eatster.Persistence.Data;
using Eatster.Persistence.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eatster.Application.Auth.Register.Command
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly AppDbContext context;
        private readonly UserManager<AppUser> manager;

        public RegisterCommandHandler(AppDbContext context, UserManager<AppUser> manager)
        {
            this.context = context;
            this.manager = manager;
        }

        async Task<RegisterResponse> IRequestHandler<RegisterCommand, RegisterResponse>.Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var appUser = new AppUser { Email = request.Email, UserName = request.UserName };

            var identityResult = await manager.CreateAsync(appUser, request.Password);

            if (!identityResult.Succeeded)
            {
                return new RegisterResponse(appUser.Id,
                                identityResult.Errors
                                .Select(e => new Domain.DTO.Error(e.Code, e.Description)));
            }

            var user = new User(request.FirstName, request.LastName, request.UserName, appUser.Id);

            this.context.Users.Add(user);
            await this.context.SaveChangesAsync(cancellationToken);

            return new RegisterResponse(appUser.Id);
        }
    }
}