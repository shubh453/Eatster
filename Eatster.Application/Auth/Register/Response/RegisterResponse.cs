using Eatster.Domain.Abstract;
using Eatster.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eatster.Application.Auth.Register.Response
{
    public class RegisterResponse : BaseResponse
    {
        public string Id { get; }

        public IEnumerable<Error> Errors { get; }

        public RegisterResponse(string id, IEnumerable<Error> errors, string message = null, bool success = false)
            :base( success, message )
        {
            Id = id;
            Errors = errors;
        }

        public RegisterResponse(string id, string message = null, bool success = true) : base(success, message)
        {
            Id = id;
        }
    }
}
