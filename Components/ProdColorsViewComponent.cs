using Microsoft.AspNetCore.Mvc;
using INTEXII.Models;

namespace INTEXII.Components {
    public class ProdColorsViewComponent : ViewComponent {
        private IIntexW24datasetRepository _repo;

        // Constructor
        public ProdColorsViewComponent(IIntexW24datasetRepository temp) => _repo = temp;

        public IViewComponentResult Invoke() {
            ViewBag.CurrentProdCategory = RouteData?.Values["prodCategory"];
            ViewBag.CurrentProdColor = RouteData?.Values["prodColor"];

            var prodColors = _repo.Products
                .Select(x => x.PrimaryColor)
                .Distinct()
                .OrderBy(x => x);

            return View(prodColors);
        }
    }
}