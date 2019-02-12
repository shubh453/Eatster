using Eatster.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eatster.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string IdentityId { get; private set; }
        public string PasswordHash { get; private set; }

        private readonly List<RefreshToken> refreshTokens = new List<RefreshToken>();

        public IReadOnlyCollection<RefreshToken> RefreshTokens => refreshTokens.AsReadOnly();

        public User()
        {
        }

        public User(string firstName, string lastName, string userName, string identityId)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            IdentityId = identityId;
        }

        public bool HasValidRefreshToken(string refreshToken)
        {
            return refreshTokens.Any(rt => rt.Token == refreshToken && rt.Active);
        }

        public void AddRefreshToken(string token, int userId, string remoteIpAddress, double daysToExpires = 5)
        {
            refreshTokens.Add(new RefreshToken(token,
                                    DateTime.UtcNow.AddDays(daysToExpires),
                                    userId,
                                    remoteIpAddress));
        }

        public void RemoveRefreshToken(string token)
        {
            refreshTokens.Remove(refreshTokens.First(rt => rt.Token == token));
        }
    }
}