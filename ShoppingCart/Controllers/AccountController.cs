using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneStore.Context;
using PhoneStore.Models;
using PhoneStore.Models.ViewModels;
using System.Security.Claims;

namespace PhoneStore.Controllers
{
    public class AccountController : Controller
    {
        private AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUserName = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
                if (existingUserName != null)
                {
                    ModelState.AddModelError("", "Пользователь с таким именем уже существует");
                    return View(user);
                }

                var existingUserEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUserEmail != null)
                {
                    ModelState.AddModelError("", "Пользователь с такой почтой уже существует");
                    return View(user);
                }

                user.Password = user.Password;
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Вы успешно зарегистрировались! Войдите в аккаунт.";

                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName && u.Password == model.Password);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Неверное имя пользователя или пароль");
            }

            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
