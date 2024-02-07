using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Context;
using PhoneStore.Models;
using PhoneStore.Models.ViewModels;
using System.Security.Claims;

namespace PhoneStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public IActionResult Index()
        {
            string userId = GetUserId();
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>($"Cart_{userId}") ?? new List<CartItem>();

            CartViewModel cartVM = new()
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Quantity * x.Price)
            };

            return View(cartVM);
        }

        public async Task<IActionResult> Add(int id)
        {
            Phone phone = await _context.Phones.FindAsync(id);

            string userId = GetUserId();
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>($"Cart_{userId}") ?? new List<CartItem>();

            CartItem cartItem = cart.FirstOrDefault(c => c.PhoneId == id && c.UserId == userId);

            if (cartItem == null)
            {
                cart.Add(new CartItem(phone, userId));
            }
            else
            {
                cartItem.Quantity += 1;
            }

            HttpContext.Session.SetJson($"Cart_{userId}", cart);

            return Redirect(Request.Headers["Referer"].ToString());
        }



        public async Task<IActionResult> Decrease(int id)
        {
            Phone phone = await _context.Phones.FindAsync(id);

            string userId = GetUserId();
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>($"Cart_{userId}");

            CartItem cartItem = cart.Where(c => c.PhoneId == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.PhoneId == id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove($"Cart_{userId}");
            }
            else
            {
                HttpContext.Session.SetJson($"Cart_{userId}", cart);
            }


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            string userId = GetUserId();
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>($"Cart_{userId}");

            cart.RemoveAll(p => p.PhoneId == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove($"Cart_{userId}");
            }
            else
            {
                HttpContext.Session.SetJson($"Cart_{userId}", cart);
            }


            return RedirectToAction("Index");
        }


        public int GetCartItemCount()
        {
            string userId = GetUserId();
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>($"Cart_{userId}") ?? new List<CartItem>();
            return cart.Sum(item => item.Quantity);
        }

        public IActionResult GetTotalItemInCart()
        {
            int cartItem = GetCartItemCount(); 
            return Ok(cartItem);
        }


        public IActionResult Clear()
        {
            string userId = GetUserId();
            HttpContext.Session.Remove($"Cart_{userId}");

            return RedirectToAction("Index");
        }
    }
}
