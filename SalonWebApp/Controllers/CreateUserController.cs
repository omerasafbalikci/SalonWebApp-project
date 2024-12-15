using Microsoft.AspNetCore.Mvc;
using SalonWebApp.Models;

namespace SalonWebApp.Controllers
{
    public class CreateUserController : Controller
    {
        private readonly SalonContext context;

        public CreateUserController(SalonContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(CreateUser createUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = this.context.Users.FirstOrDefault(u => u.Email == createUser.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Email already exists.");
                        return View(existingUser);
                    }

                    var user = new User
                    {
                        FirstName = createUser.FirstName,
                        LastName = createUser.LastName,
                        Password = createUser.Password,
                        Email = createUser.Email,
                        PhoneNumber = createUser.PhoneNumber,
                        Gender = createUser.Gender,
                        Role = Roles.MEMBER
                    };

                    this.context.Users.Add(user);
                    this.context.SaveChanges();
                    return RedirectToAction("Login", "Member");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while creating the user." + ex);
                    return View(createUser);
                }
            }
            return View(createUser);
        }
    }
}
