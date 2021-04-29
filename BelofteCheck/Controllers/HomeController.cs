using System.Web.Mvc;
using BelofteCheck.ViewModels;

namespace BelofteCheck.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HomeVM homeVM = new HomeVM();
            string msg = "";
            string level = homeVM.MessageSection.Info;
            string title = "";
            homeVM.MessageSection.SetMessage(title, level, msg);
            return View(homeVM);
        }

        public ActionResult About()
        {
            HomeVM homeVM = new HomeVM();
            string msg = "Wie wij zijn en wat we willen";
            string level = homeVM.MessageSection.Info;
            string title = "Over ons";
            homeVM.MessageSection.SetMessage(title, level, msg);
            return View(homeVM);
        }

        public ActionResult Contact()
        {
            HomeVM homeVM = new HomeVM();
            string msg = "Hoe je ons kan bereiken";
            string level = homeVM.MessageSection.Info;
            string title = "Contact";
            homeVM.MessageSection.SetMessage(title, level, msg);
            return View(homeVM);
        }
    }
}