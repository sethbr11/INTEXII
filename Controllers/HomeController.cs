using INTEXII.Models;
using INTEXII.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.ProjectModel;
using System.Diagnostics;

namespace INTEXII.Controllers {
    public class HomeController : Controller {
        private IIntexW24datasetRepository _repo;

        // Constructor
        public HomeController(IIntexW24datasetRepository temp) => _repo = temp;

        [AllowAnonymous]
        public IActionResult Index() {
            // Top 5 products
            var data = _repo.Products.OrderByDescending(p => p.PopularityRank).Take(5).ToList();

			return View(data);
        }

        [AllowAnonymous]
        public IActionResult Shop(int pageNum = 1, string prodCategory = null, string prodColor = null) {

            int pageSize;

            // Try to get the page size from TempData
            if (TempData["PageSize"] != null)
            {
                pageSize = (int)TempData["PageSize"];
                // Store the page size in session for persistence
                HttpContext.Session.SetInt32("PageSize", pageSize);
            }
            else
            {
                // If TempData is empty, check session storage
                pageSize = HttpContext.Session.GetInt32("PageSize") ?? 5;
            }


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

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SetResultsPerPage(int resultsPerPage)
        {
            // Store the selected results per page in the session
            HttpContext.Session.SetInt32("PageSize", resultsPerPage);

            // Redirect to the Shop action or the desired action
            return RedirectToAction("Shop");
        }
        [AllowAnonymous]
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

        [AllowAnonymous]
        public IActionResult AboutUs() => View();

        [AllowAnonymous]
        public IActionResult Privacy() => View();

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [AllowAnonymous]
        public IActionResult AdminReviewProducts()
        {
            var data = _repo.Products.ToList();
            return View(data);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AdminAddProduct(int? id)
        {
            if (id.HasValue)
            {
                var product = _repo.Products.SingleOrDefault(x => x.ProductId == id.Value);
                if (product == null)
                {
                    return NotFound(); // Or handle the case when the task is not found
                }
                else
                {
                    return View(product);
                }
            }
            else
            {
                return View(new Product());
            }
        }
        [AllowAnonymous]
        [HttpPost]
        // controller for admin to add/edit a product
        public IActionResult AdminAddProduct(Product r) 
        {
            //_repo.AddProduct(response);
            // return View("AddProductConfirmation");
            if (r.ProductId == null)
            {
                // Add new task
                _repo.AddProduct(r);
            }
            else
            {
                // Update existing task

                _repo.UpdateProduct(r);
            }
            return View("AddProductConfirmation");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Delete(int id)
        {

            var recordToDelete = _repo.Products.SingleOrDefault(x => x.ProductId == id);

            return View(recordToDelete);
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Delete(Product p)
        {


            _repo.DeleteProduct(p);
            return RedirectToAction("AdminReviewProducts");

        }

        [AllowAnonymous]
        public IActionResult AdminReviewUsers()
        {
            var data = _repo.Customers.ToList();
            return View(data);
        }

		[AllowAnonymous]
        public IActionResult Checkout()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult DeleteUser(int id)
        {

            //var recordToDelete = _repo.Products.SingleOrDefault(x => x.C == id);

            return View(/*recordToDelete*/);
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult DeleteUser(Customer p)
        {


            //_repo.DeleteProduct(/*p*/);
            return RedirectToAction("AdminReviewProducts");

        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AdminEditUser(int? id)
        {
            if (id.HasValue)
            {
                var product = _repo.Customers.SingleOrDefault(x => x.CustomerId == id.Value);
                if (product == null)
                {
                    return NotFound(); // Or handle the case when the task is not found
                }
                else
                {
                    return View(product);
                }
            }
            else
            {
                return View(new Product());
            }
        }
        [AllowAnonymous]
        [HttpPost]
        // controller for admin to add/edit a product
        public IActionResult AdminEditUser(Customer r)
        {
            //_repo.AddProduct(response);
            // return View("AddProductConfirmation");

            // unblock this code 
            //if (r.CustomerId == null)
            //{
            //    _repo.AddProduct(r);
            //}
            //else
            //{
            //    _repo.UpdateProduct(r);
            //}
            return View("AddEditUser");
        }
    }
}
