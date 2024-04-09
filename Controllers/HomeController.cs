using INTEXII.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace INTEXII.Controllers {
    public class HomeController : Controller {
        private IIntexW24datasetRepository _repo;

        // Constructor
        public HomeController(IIntexW24datasetRepository temp) => _repo = temp;

        public IActionResult Index() {
            var data = Tuple.Create(_repo.Customers.ToList(), 
                                    _repo.LineItems.ToList(),
                                    _repo.Orders.ToList(),
                                    _repo.Products.ToList());
            return View(data);
        }

        public IActionResult Shop() {
            var data = _repo.Products.ToList();
            return View(data); 
        }

        public IActionResult AboutUs() => View();

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
