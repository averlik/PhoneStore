using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PhoneStore.Context.Components
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public CategoriesViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string brandSlug)
        {
            var brands = await _context.Brands.ToListAsync();
            ViewBag.CurrentCategorySlug = brandSlug;
            return View(brands);
        }
    }
}
