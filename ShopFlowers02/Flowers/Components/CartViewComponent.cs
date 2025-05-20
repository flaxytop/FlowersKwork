
using Flowers.Domain.Cart;
using Flowers.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Flowers.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<Cart>("cart");
            return View(cart);
        }
    }
}


