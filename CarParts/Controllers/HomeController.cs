using CarParts.Helpers;
using System.Web.Mvc;
using System.Linq;

namespace CarParts.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new CarPartsEntities())
            {
                ViewBag.CustomerList = db.Customers.Where(x => !x.Deleted).Select(x => new SelectListItem() { Text = string.IsNullOrEmpty(x.CompanyName) ? x.Name + " " + x.Surname : x.CompanyName, Value = x.Id.ToString() }).ToList();
            }
            return View();
        }
    }
}
