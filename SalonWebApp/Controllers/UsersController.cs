using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalonWebApp.Models;

namespace SalonWebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly SalonContext _context;

        public UsersController(SalonContext context)
        {
            _context = context;
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUser createUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = _context.Users.FirstOrDefault(u => u.Email == createUser.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılmaktadır.");
                        return View(createUser);
                    }

                    var user = new User
                    {
                        FirstName = createUser.FirstName,
                        LastName = createUser.LastName,
                        Password = createUser.Password,
                        Email = createUser.Email,
                        PhoneNumber = createUser.PhoneNumber,
                        Gender = createUser.Gender,
                        Role = Roles.MEMBER,
                        Appointments = null
                    };
                    _context.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("Login", "Member");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyin.");
                    return View(createUser);
                }
            }
            return View(createUser);
        }
    }
}
