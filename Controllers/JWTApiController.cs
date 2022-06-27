using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Task11MultipleAuthentication2.Models.VMs;
using Task11MultipleAuthentication2.Services.IServices;

namespace Task11MultipleAuthentication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JWTApiController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public JWTApiController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        [Route("LoginJWT")]
        public async Task<IActionResult> LoginJWT([FromQuery]LoginVM loginVM)
        {
            var result = await _accountService.LoginWithJWT(loginVM);
            if(result!=null)
                return Ok(result);
            return BadRequest("Invalid Login Attempt");
        }
    }
}
