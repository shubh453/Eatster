using Eatster.Domain.Abstract;
using Eatster.Domain.DTO;
using System.Collections.Generic;

namespace Eatster.Application.Login.Response
{
    public class LoginResponse : BaseResponse
    {
        public AccessToken AccessToken { get; }

        public string RefreshToken { get; }

        public IEnumerable<Error> Errors { get; }

        public LoginResponse(AccessToken accessToken,
                              string refreshToken,
                              bool success = true,
                              string message = null) : base(success, message)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public LoginResponse(IEnumerable<Error> errors,
                              bool success = false,
                              string message = null) : base(success, message) => Errors = errors;
    }
}