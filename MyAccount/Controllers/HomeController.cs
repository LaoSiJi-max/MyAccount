using MyAccount.Services;
using System.Web.Mvc;

namespace MyAccount.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (OwnersService.IsLogined)
            {
                ViewBag.userName = "当前登录主人:"+OwnersService.GetLoginOwner().Name;
            }
            else
            {
                ViewBag.userName = "当前未登录任何主人";
            }

            return View();
        }
    }
}