using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Context;
using PhoneStore.Models;

namespace PhoneStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 4;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.Phones.Where(p => !p.IsDeleted).Count() / pageSize);

            return View(await _context.Phones.Where(p => !p.IsDeleted).OrderByDescending(p => p.Id)
                                                                            .Include(p => p.Brand)
                                                                            .Skip((p - 1) * pageSize)
                                                                            .Take(pageSize)
                                                                            .ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Brands = new SelectList(_context.Brands, "Id", "BrandName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Phone phone)
        {
            ViewBag.Brands = new SelectList(_context.Brands, "Id", "BrandName");

            if (ModelState.IsValid)
            {
                phone.Slug = phone.Name.ToLower().Replace(" ", "-");

                var slug = await _context.Phones.Where(p => !p.IsDeleted).FirstOrDefaultAsync(p => p.Slug == phone.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Товар уже существует.");
                    return View(phone);
                }

                if (phone.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/");
                    string imageName = Guid.NewGuid().ToString() + "_" + phone.ImageUpload.FileName;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await phone.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    phone.Image = imageName;
                }

                _context.Add(phone);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Товар был успешно создан";

                return RedirectToAction("Index");
            }

            return View(phone);
        }

        /* public async Task<IActionResult> Edit(int id)
         {
             Phone phone = await _context.Phones.FindAsync(id);

             ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name", phone.BrandId);

             return View(phone);
         }*/

        public async Task<IActionResult> Edit(int id)
        {          
            Phone phone = await _context.Phones.FindAsync(id);

            if (phone == null)
            {
                return NotFound();
            }

            ViewBag.Brands = new SelectList(_context.Brands, "Id", "BrandName");

            return View(phone);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Phone phone)
        {
            ViewBag.Brands = new SelectList(_context.Brands, "Id", "BrandName");

            if (ModelState.IsValid)
            {
                phone.Slug = phone.Name.ToLower().Replace(" ", "-");

                if (phone.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/");
                    string imageName = Guid.NewGuid().ToString() + "_" + phone.ImageUpload.FileName;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await phone.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    phone.Image = imageName;
                }

                _context.Update(phone);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Товар был успешно отредактирован";
            }

            return View(phone);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Phone phone = await _context.Phones.FindAsync(id);

            phone.IsDeleted = true;
            _context.Update(phone);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Товар был успешно удален";

            return RedirectToAction("Index");
        }
    }
}
