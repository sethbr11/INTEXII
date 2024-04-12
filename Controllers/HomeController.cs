using INTEXII.Models;
using INTEXII.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.ProjectModel;
using System.Diagnostics;
using Microsoft.ML;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace INTEXII.Controllers {
    public class HomeController : Controller {
        private IIntexW24datasetRepository _repo;
        private InferenceSession _session;
        private readonly ILogger<HomeController> _logger;

        // Constructor
        public HomeController(IIntexW24datasetRepository temp, ILogger<HomeController> logger) {
            _repo = temp;
            _logger = logger;

            try {
                _session = new InferenceSession("./fraud_model2.onnx");
                _logger.LogInformation("NNX model loaded successfully.");
                Console.WriteLine("Success");
            }
            catch (Exception ex) {
                _logger.LogError($"Error loading the ONNX model: {ex.Message}");
                Console.WriteLine("Error");
            }

        }

        [AllowAnonymous]
        public IActionResult Index() {
            // Top 5 products
            var data = _repo.Products.OrderByDescending(p => p.PopularityRank).Take(3).ToList();

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
                var product = _repo.Products.First(x => x.ProductId == productId);

                var similarProductIds = new List<int?> {
                        product.Recommendation1,
                        product.Recommendation2,
                        product.Recommendation3
                };

                var similarProducts = _repo.Products
                    .Where(x => similarProductIds.Contains(x.ProductId))
                    .OrderByDescending(x => x.PopularityRank)
                    .ToList();

				return View(Tuple.Create(product, returnUrl, similarProducts));
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddOrder() { return View(); }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddOrder(Order o) {
            //_repo.AddOrder(o);
            var prediction = Predict(PrepareModelInput(o));
            if (prediction == 1 || prediction == 0 )
            {
                o.PredFraud = prediction;
                _repo.AddOrder(o);
            }
            else
            {
                
            }
            return RedirectToAction("OrderConfirmation", new { prediction = prediction });
        }

        public IActionResult OrderConfirmation(int prediction)
        {
            return View(prediction);
        }
        // ONNX MODEL PREDICTING
        private List<int> PrepareModelInput(Order order) {
            int time = (int)order.Time;
            int amount = (int)order.Amount;
            int country_UK = (int)(order.CountryOfTransaction == "United Kingdom" ? 1 : 0);

            return new List<int>() { time, amount, country_UK };
        }

        public int Predict(List<int> valuesToPredict) {
            int time = valuesToPredict[0];
            int amount = valuesToPredict[1];
            int country_of_transaction_United_Kingdom = valuesToPredict[2];

            // mapping of class type to readable format
            var class_type_dict = new Dictionary<int, string>
            {
                {0, "Not Fraud" },
                {1, "Fraud" }
            };
            try {
                // prepare data input for the ONNX model
                var input = new List<float> { time, amount, country_of_transaction_United_Kingdom };
                var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });
                var inputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
                };
                // Run the model with the input data
                using (var results = _session.Run(inputs)) {
                    // Retrieve the prediction result
                    var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>().ToArray();
                    if (prediction != null && prediction.Length > 0) {
                        // Map the numerical result to a meaningful string
                        var fraudType = class_type_dict.GetValueOrDefault((int)prediction[0], "Unknown");
                        TempData["Prediction"] = fraudType;
                        Console.WriteLine(TempData["Prediction"] = fraudType);
                        return (int)prediction[0];
                        
                    }
                    else {
                        TempData["Prediction"] = "Error: Unable to make a prediction";
                        return -1;
                    }
                }
                // Return the view with the prediction result 
                //return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) {
                Console.WriteLine("Predication Failed");
                // Handle exceptions and return error message
                //return BadRequest($"Error: {ex.Message}");
                return -1;
            }
        }
    }
}
