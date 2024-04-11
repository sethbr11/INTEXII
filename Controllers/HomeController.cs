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
                                    _repo.Products.ToList(),
                                    _repo.Recommendations.ToList());
            return View(data);
        }

        public IActionResult Shop(int pageNum = 1, string prodCategory = null, string prodColor = null) {

            int pageSize = TempData["PageSize"] != null ? (int)TempData["PageSize"] : 5;


            var data = new ProductsListViewModel {
                Products = _repo.Products
                    .Where(x => x.PublicCategory == prodCategory || prodCategory == null)
                    .Where(x => x.PrimaryColor == prodColor || prodColor == null)
                    .OrderByDescending(x => x.PopularityRank) // Sort by popularity descending
                    .ThenBy(x => x.Price) // Then sort by price ascending (if applicable)
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize),
                PaginationInfo = new PaginationInfo {
                    CurrentPage = pageNum,
                    ItemsPerPage = pageSize,
                    TotalItems = prodCategory == null ?
                        _repo.Products.Count() :
                        _repo.Products.Where(x => x.Category == prodCategory).Count()
                },
                CurrentProdCategory = prodCategory,
                CurrentProdColor = prodColor
            };

            return View(data);
        }

        [HttpPost]
        public IActionResult SetResultsPerPage(int resultsPerPage)
        {
            TempData["PageSize"] = resultsPerPage;
            return RedirectToAction("Shop");
        } 

        public IActionResult ProductDetail(int productId, string returnUrl) {
            try { // Find the product and go to that product's page
                var data = _repo.Products.First(x => x.ProductId == productId);
                return View(Tuple.Create(data, returnUrl));
            }
            // If there was no productId passed or the productId is out of range, go to the Shop IActionResult
            catch (Exception ex) { // We won't do anything with the exception
                return RedirectToAction("Shop");
            }
        }

        public IActionResult AboutUs() => View();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
