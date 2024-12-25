using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalonWebApp.Models;

namespace SalonWebApp.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly SalonContext _context;

        public AppointmentsController(SalonContext context)
        {
            _context = context;
        }

        // GET: Appointments
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index()
        {
            var salonContext = _context.Appointments.Include(a => a.Employee).Include(a => a.Salon).Include(a => a.Service).Include(a => a.Time).Include(a => a.User);
            return View(await salonContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Employee)
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .Include(a => a.Time)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            if (User.IsInRole("MEMBER"))
            {
                var currentUserIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserIdString)) return Forbid();
                if (!int.TryParse(currentUserIdString, out int currentUserId))
                {
                    return Forbid();
                }

                if (appointment.UserId != currentUserId) return Forbid();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        [Authorize(Roles = "MEMBER, ADMIN")]
        public IActionResult Create()
        {
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.Salons = _context.Salons.ToList();
            ViewBag.Services = _context.Services.ToList();
            ViewBag.Times = _context.Times.Where(t => t.Selectable).ToList();
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MEMBER, ADMIN")]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                bool hasConflict = await _context.Appointments.AnyAsync(a => a.EmployeeId == appointment.EmployeeId && a.TimeId == appointment.TimeId);
                if (hasConflict)
                {
                    ModelState.AddModelError("", "Bu çalışan bu saat aralığında başka bir randevuya sahip. Lütfen farklı bir saat veya çalışan seçin.");
                    ViewBag.Employees = _context.Employees.ToList();
                    ViewBag.Salons = _context.Salons.ToList();
                    ViewBag.Services = _context.Services.ToList();
                    ViewBag.Times = _context.Times.Where(t => t.Selectable).ToList();
                    return View(appointment);
                }

                var service = await _context.Services.FindAsync(appointment.ServiceId);
                if (service == null)
                {
                    ModelState.AddModelError("", "Geçersiz servis seçimi.");
                    return View(appointment);
                }

                var currentUserIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(currentUserIdString, out int currentUserId))
                {
                    return Forbid();
                }
                appointment.UserId = currentUserId;

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.Salons = _context.Salons.ToList();
            ViewBag.Services = _context.Services.ToList();
            ViewBag.Times = _context.Times.Where(t => t.Selectable).ToList();
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.Salons = _context.Salons.ToList();
            ViewBag.Services = _context.Services.ToList();
            ViewBag.Times = _context.Times.Where(t => t.Selectable).ToList();
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.Salons = _context.Salons.ToList();
            ViewBag.Services = _context.Services.ToList();
            ViewBag.Times = _context.Times.Where(t => t.Selectable).ToList();
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Employee)
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .Include(a => a.Time)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }
    }
}
