using Eatster.Domain.DTO;
using System.Threading.Tasks;

namespace Eatster.Application.Interfaces
{
    public interface IJwtFactory
    {
        Task<AccessToken> GenerateEncodedToken(string id, string userName);
    }
}