using Microsoft.AspNetCore.Mvc;
using ChartDirector;

namespace NetCoreMvcCharts.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Indextop()
        {
            return View();
        }

        public ActionResult Indexleft()
        {
            return View();
        }

        public ActionResult Indexright()
        {
            return View();
        }
    }
}