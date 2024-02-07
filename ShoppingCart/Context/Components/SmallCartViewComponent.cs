using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Models;
using PhoneStore.Models.ViewModels;
using System.Security.Claims;

namespace PhoneStore.Context.Components
{
    public class SmallCartViewComponent : ViewComponent
    {
        private string GetUserId()
        {
            return (User as ClaimsPrincipal)?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public IViewComponentResult Invoke()
        {
            string userId = GetUserId();
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>($"Cart_{userId}");
            SmallCartViewModel smallCartVM;

            if (cart == null || cart.Count == 0)
            {
                smallCartVM = null;
            } else
            {
                smallCartVM = new()
                {
                    NumberOfItems = cart.Sum(x => x.Quantity),
                    TotalAmount = cart.Sum(x => x.Quantity * x.Price)
                };
            }

            return View(smallCartVM);
        }
    }
}
