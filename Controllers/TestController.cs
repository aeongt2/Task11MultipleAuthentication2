using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Task11MultipleAuthentication2.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        
        public IActionResult Index()
        {
            return Ok("Index Loaded");
        }
    }
}
