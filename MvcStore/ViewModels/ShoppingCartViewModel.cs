using System.Collections.Generic;
using MvcStore.Models;

namespace MvcStore.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IList<CartItem> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}
