using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repo;
        private Cart cart;

        public OrderController(IOrderRepository repoServices, Cart cartServices)
        {
            cart = cartServices;
            repo = repoServices;
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout (Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty");
            }

            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repo.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            } else
            {
                return View(order);
            }
        }
        
        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }

    }
}
