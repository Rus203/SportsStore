using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public PagingInfo PagingInfo { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
