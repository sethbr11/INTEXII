using Microsoft.AspNetCore.Mvc;
using INTEXII.Models;

namespace INTEXII.Components {
    public class ProjectTypesViewComponent : ViewComponent {
        private IIntexW24datasetRepository _repo;

        // Constructor
        public ProjectTypesViewComponent(IIntexW24datasetRepository temp) {
            _repo = temp;
        }

        public IViewComponentResult Invoke() {
            ViewBag.SelectedProjectType = RouteData?.Values["projectType"];

            var projectTypes = _repo.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);

            return View(projectTypes);
        }
    }
}


