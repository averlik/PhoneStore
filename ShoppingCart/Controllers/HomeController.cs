using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Context;
using PhoneStore.Models.ViewModels;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string brandId, string sortOrder, string sterm)
        {
            var query = _context.Phones
                .Include(p => p.Brand)
                .Where(p => !p.IsDeleted);

            int? brandIdValue = null;

            if (!string.IsNullOrEmpty(brandId) && int.TryParse(brandId, out var parsedBrandIdValue))
            {
                brandIdValue = parsedBrandIdValue;
                query = query.Where(p => p.BrandId == brandIdValue);
            }

            switch (sortOrder)
            {
                case "price_asc":
                    {
                        query = query.OrderBy(p => p.Price);
                        break;
                    }
                case "price_desc":
                    {
                        query = query.OrderByDescending(p => p.Price);
                        break;
                    }
            }

            var phones = await query.ToListAsync();
            var brands = await _context.Brands.ToListAsync();

            var viewModel = new PhoneViewModel
            {
                Phones = phones,
                Brands = brands,
                STerm = sterm,
                BrandId = brandIdValue ?? 0,
                SortOrder = sortOrder,

            };

            return View(viewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
