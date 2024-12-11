using Microsoft.AspNetCore.Mvc;

namespace Nest1.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
