using AutoMapper;
using Eatster.Application.Interfaces;
using Eatster.Application.Login.Response;
using Eatster.Domain.DTO;
using Eatster.Persistence.Data;
using Eatster.Persistence.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eatster.Application.Login.Command
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;

        public LoginCommandHandler
            (IMapper mapper,
            UserManager<AppUser> userManager,
            AppDbContext context,
            IJwtFactory jwtFactory,
            ITokenFactory tokenFactory)
        {
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
        }

        async Task<LoginResponse> IRequestHandler<LoginCommand, LoginResponse>.Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Email) && !string.IsNullOrEmpty(request.Password))
            {
                var appUser = await _userManager.FindByEmailAsync(request.Email);

                if (appUser != null)
                {
                    var user = await _context.Users.Where(u => u.IdentityId == appUser.Id)
                                               .Include(u => u.RefreshTokens)
                                               .FirstOrDefaultAsync(cancellationToken);
                    user = _mapper.Map(appUser, user);

                    if (await _userManager.CheckPasswordAsync(appUser, request.Password))
                    {
                        var refreshToken = _tokenFactory.GenerateToken();

                        user.AddRefreshToken(refreshToken, user.Id, request.RemoteIpAddress);

                        _context.Entry(user).State = EntityState.Modified;

                        await _context.SaveChangesAsync(cancellationToken);

                        return new LoginResponse(await _jwtFactory.GenerateEncodedToken(user.IdentityId, user.UserName), refreshToken, true);
                    }
                    else
                        return new LoginResponse(new[] { new Error("login_failure", "Password Incorrect.") });
                }
                else
                    return new LoginResponse(new[] { new Error("login_failure", "Invalid email.") });
            }
            else
                return new LoginResponse(new[] { new Error("login_failure", "Invalid email or password.") });
        }
    }
}