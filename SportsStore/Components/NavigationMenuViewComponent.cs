using Microsoft.AspNetCore.Mvc;
using System.Linq;
using SportsStore.Models;
using System.Threading.Tasks;

namespace SportsStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IProductRepository repository;
        public NavigationMenuViewComponent (IProductRepository repo)
        {
            repository = repo;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(x => x));
        }
    }
}
