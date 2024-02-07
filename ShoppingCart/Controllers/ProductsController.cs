using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Context;
using PhoneStore.Models;

namespace PhoneStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string brandSlug = "", int p = 1, string sortOrder = "")
        {
            int pageSize = 4;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.BrandSlug = brandSlug;

            ViewBag.PageTarget = "/products";

            ViewBag.AdditionalQueryParameters = $"&sortOrder={sortOrder}&brandSlug={brandSlug}";

            IQueryable<Phone> query = _context.Phones.Include(p => p.Brand).Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(brandSlug))
            {
                Brand category = await _context.Brands.Where(c => c.Slug == brandSlug).FirstOrDefaultAsync();
                if (category == null) return RedirectToAction("Index");
                query = query.Where(p => p.BrandId == category.Id);
            }

            switch (sortOrder)
            {
                case "asc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                default:
                    query = query.OrderByDescending(p => p.Id);
                    break;
            }

            ViewBag.TotalPages = (int)Math.Ceiling((decimal)query.Count() / pageSize);
            var products = await query.Skip((p - 1) * pageSize).Take(pageSize).ToListAsync();

            return View(products);
        }

        [HttpGet("phone/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var phone = await _context.Phones.Include(p => p.Brand).Where(p => !p.IsDeleted).FirstOrDefaultAsync(p => p.Id == id);
            if (phone == null) return NotFound();
            return View(phone);
        }
    }
}
