using AKQA.DemoApp.Models;
using System.Configuration;
using System.Web.Mvc;

namespace AKQA.DemoApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "AKQA Demo";

            var model = new ConvertAmountToWordsViewModel
            {
                DemoAppServiceBaseUrl = ConfigurationManager.AppSettings["DemoAppServiceBaseUrl"]
            };

            return View(model);
        }
    }
}
