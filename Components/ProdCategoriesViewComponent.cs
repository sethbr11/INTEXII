using Microsoft.AspNetCore.Mvc;
using INTEXII.Models;

namespace INTEXII.Components {
    public class ProdCategoriesViewComponent : ViewComponent {
        private IIntexW24datasetRepository _repo;

        // Constructor
        public ProdCategoriesViewComponent(IIntexW24datasetRepository temp) => _repo = temp;

        public IViewComponentResult Invoke() {
            ViewBag.CurrentProdCategory = Request.Query["prodCategory"];
            ViewBag.CurrentProdColor = Request.Query["prodColor"];

            var prodCategories = _repo.Products
                .Select(x => x.PublicCategory)
                .Distinct()
                .OrderBy(x => x);

            return View(prodCategories);
        }
    }
}