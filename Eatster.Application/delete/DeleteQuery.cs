using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eatster.Application.delete
{
    public class DeleteQuery : IRequest
    {
        public int Id { get; set; }
    }
}
