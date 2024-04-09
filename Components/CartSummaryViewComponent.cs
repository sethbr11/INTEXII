using Microsoft.AspNetCore.Mvc;
using INTEXII.Models;

namespace INTEXII.Components {
    public class CartSummaryViewComponent : ViewComponent {
        private Cart cart;

        public CartSummaryViewComponent(Cart cartService) {
            cart = cartService;
        }

        public IViewComponentResult Invoke() {
            return View(cart);
        }
    }
}
