using System.Web.Mvc;

namespace Intelligent.Community.Web.Controllers
{
    public class HomeController : CustomController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Default()
        {
            return View();
        }
    }
}