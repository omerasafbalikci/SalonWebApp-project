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
        public IActionResult Index(CretateUser cretateUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = context.
                }
            }
    }
}
