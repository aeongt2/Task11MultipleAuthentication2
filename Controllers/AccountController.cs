using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Task11MultipleAuthentication2.Models.VMs;
using Task11MultipleAuthentication2.Services.IServices;

namespace Task11MultipleAuthentication2.Controllers
{
    public class AccountController : Controller
    {
        IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.Login(loginVM);
                if (result)
                    return RedirectToAction("Index", "Home");
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt!");
                    TempData["error"] = "Login Failed";
                }

            }
            return View(loginVM);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                string result = await _accountService.RegisterNewUser(registerVM);
                if (result != null)
                {
                    ModelState.AddModelError(string.Empty, result.ToString());
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            return View(registerVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
