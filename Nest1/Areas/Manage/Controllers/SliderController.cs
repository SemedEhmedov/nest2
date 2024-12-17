using Microsoft.AspNetCore.Mvc;

namespace Nest1.Areas.Manage.Controllers
{
    public class SliderController : ManageController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
