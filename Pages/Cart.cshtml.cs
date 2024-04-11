using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using INTEXII.Infrastructure;
using INTEXII.Models;
using Microsoft.AspNetCore.Authorization;

namespace INTEXII.Pages
{
    [AllowAnonymous]
    public class CartModel : PageModel {
        private IIntexW24datasetRepository _repo;
        public CartModel(IIntexW24datasetRepository temp, Cart cartService) {
            _repo = temp;
            Cart = cartService;
        }

        public Cart? Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";
        public void OnGet(string returnUrl) {
            ReturnUrl = returnUrl ?? "/";

            // After SessionCart is added we don't need this line
            //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        public IActionResult OnPost(int productId, string returnUrl) {
            Product prod = _repo.Products
                .FirstOrDefault(x => x.ProductId == productId);

            if (prod != null) {
                // After SessionCart is added we don't need this line
                //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();

                Cart.AddItem(prod, 1);

                // Or this line
                //HttpContext.Session.SetJson("cart", Cart);
            }

            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove(int productId, string returnUrl) {
            Cart.RemoveLine(Cart.Lines.First(x => x.Product.ProductId == productId).Product);

            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
