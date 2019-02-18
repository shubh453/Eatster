using Eatster.Api.Infrastructure;
using Eatster.Application.Auth.Register.Command;
using Eatster.Application.delete;
using Eatster.Application.Login.Command;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eatster.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [Authorize("ApiUser")]
        [HttpGet("[action]")]
        public async Task<IActionResult> Protected()
        {
            await Mediator.Send(new DeleteQuery());
            return NoContent();
        }
    }
}