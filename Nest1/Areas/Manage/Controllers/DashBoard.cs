using Microsoft.AspNetCore.Mvc;

namespace Nest1.Areas.Manage.Controllers
{

    public class DashBoard : ManageController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
