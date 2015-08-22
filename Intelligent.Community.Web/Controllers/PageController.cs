using System.Web.Mvc;

namespace Intelligent.Community.Web.Controllers
{
    [AllowAnonymous]
    public class PageController : Controller
    {
        // GET: Page
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult Unknown()
        {
            return View();
        }
    }
}