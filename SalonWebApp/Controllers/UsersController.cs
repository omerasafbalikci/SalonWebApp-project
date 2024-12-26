using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalonWebApp.Models;
using System.Security.Cryptography;
using System.Text;

namespace SalonWebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly SalonContext _context;

        public UsersController(SalonContext context)
        {
            _context = context;
        }

        // GET: Users
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.Include(u => u.Appointments)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/CreateMember
        [AllowAnonymous]
        public IActionResult CreateMember()
        {
            return View();
        }

        // POST: Users/CreateMember
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> CreateMember(CreateUser user)
        {
            if (ModelState.IsValid)
            {
                bool emailExists = _context.Users.Any(u => u.Email == user.Email);
                if (emailExists)
                {
                    ModelState.AddModelError("", "Bu email zaten mevcut.");
                    return View(user);
                }

                string hashedPassword = HashPassword(user.Password);

                var u = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = hashedPassword,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Gender = user.Gender,
                    Role = Roles.MEMBER
                };

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "User");
            }
            return View(user);
        }

        // GET: User/CreateAdmin
        [Authorize(Roles = "ADMIN")]
        public IActionResult CreateAdmin()
        {
            return View();
        }

        // POST: User/CreateAdmin
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(CreateUser model)
        {
            if (ModelState.IsValid)
            {
                bool emailExists = _context.Users.Any(u => u.Email == model.Email);
                if (emailExists)
                {
                    ModelState.AddModelError("", "Bu email zaten mevcut.");
                    return View(model);
                }

                string hashedPassword = HashPassword(model.Password);

                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = hashedPassword,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    Role = Roles.ADMIN
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: User/Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var hashedPassword = HashPassword(model.Password);
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == hashedPassword);

            if (user == null)
            {
                ModelState.AddModelError("", "Email veya parola yanlış.");
                return View(model);
            }

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("Role", user.Role.ToString());

            return RedirectToAction("Index", "Home");
        }

        // GET: User/Logout
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var currentUserIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserIdString)) return Forbid();
            if (!int.TryParse(currentUserIdString, out int currentUserId))
            {
                return Forbid();
            }

            if (User.IsInRole("ADMIN") && user.UserId != currentUserId) return Forbid();

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.Password = HashPassword(user.Password);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}
