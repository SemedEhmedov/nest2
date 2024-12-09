using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nest1.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin")]
    public class ManageController : Controller
    {
    
    }
}
