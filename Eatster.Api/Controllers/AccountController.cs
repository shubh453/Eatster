using Eatster.Api.Infrastructure;
using Eatster.Application.Login.Command;
using Eatster.Application.Login.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Eatster.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        [HttpPost("[action]")]
        [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}