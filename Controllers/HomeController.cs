using INTEXII.Models;
using INTEXII.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NuGet.ProjectModel;
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

        public IActionResult Shop(int pageNum, string prodCategory) {
            int pageSize = 10;

            var data = new ProductsListViewModel {
                Products = _repo.Products
                .Where(x => x.Category == prodCategory || prodCategory == null)
                    .OrderBy(x => x.Name) // CHANGE TO BE POPULARITY
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),
                PaginationInfo = new PaginationInfo {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = prodCategory == null ?
                                                _repo.Products.Count() :
                                                _repo.Products.Where(x => x.Category == prodCategory).Count()
                },
                CurrentProjectType = prodCategory
            };

            return View(data);
        }

        public IActionResult AboutUs() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
