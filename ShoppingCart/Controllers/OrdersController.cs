using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Context;
using PhoneStore.Models;
using PhoneStore.Models.ViewModels;
using System.Security.Claims;

namespace PhoneStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string pickupPoint)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var cartItems = HttpContext.Session.GetJson<List<CartItem>>($"Cart_{userId}");
            if (cartItems != null)
            {
                foreach (var item in cartItems)
                {
                    var phone = await _context.Phones.FindAsync(item.PhoneId);
                    if (phone != null)
                    {
                        phone.CountSales += item.Quantity;
                        _context.Update(phone);
                    }
                }

                await _context.SaveChangesAsync();
            }

            CartViewModel cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
            };

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = Math.Round(cartVM.GrandTotal, 2),
            };

            foreach (var item in cartVM.CartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    PhoneId = item.PhoneId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove($"Cart_{userId}");

            TempData["Success"] = "Спасибо за покупку!";

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await _context.Orders
                               .Where(o => o.UserId == userId)
                               .Include(o => o.OrderItems)
                               .ThenInclude(oi => oi.Phone)
                               .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = await _context.Orders
                                      .Include(o => o.OrderItems)
                                      .ThenInclude(oi => oi.Phone)
                                      .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
