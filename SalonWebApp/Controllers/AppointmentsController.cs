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
        [Authorize(Roles = "MEMBER,ADMIN")]
        public IActionResult Create()
        {
            var salons = _context.Salons.ToList();
            var services = _context.Services.ToList();
            var employees = _context.Employees.ToList();
            var times = _context.Times.Where(t => t.Selectable).ToList();

            ViewBag.Salons = new SelectList(salons, "SalonId", "Name");
            ViewBag.Services = new SelectList(services, "ServiceId", "Name");
            ViewBag.Employees = new SelectList(employees, "EmployeeId", "FirstName");
            ViewBag.Times = new SelectList(times, "TimeId", "StartTime");

            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MEMBER,ADMIN")]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            var salon = await _context.Salons.FindAsync(appointment.SalonId);
            if (salon == null)
            {
                ModelState.AddModelError("", "Seçilen salon geçerli değil.");
            }
            else
            {
                var selectedTime = await _context.Times.FindAsync(appointment.TimeId);
                if (selectedTime == null)
                {
                    ModelState.AddModelError("", "Geçerli bir saat dilimi seçiniz.");
                }
                else
                {
                    var appointmentDateTime = selectedTime.Date.Date + selectedTime.StartTime;
                    if (!salon.IsOpen(appointmentDateTime))
                    {
                        ModelState.AddModelError("", $"Salon bu tarih ve saatte kapalı: {appointmentDateTime}");
                    }
                }
            }

            var service = await _context.Services.FindAsync(appointment.ServiceId);
            if (service == null || service.SalonId != appointment.SalonId)
            {
                ModelState.AddModelError("", "Seçilen servis bu salonda tanımlı değil veya geçersiz servis.");
            }

            bool canEmployeeDoService = await _context.EmployeeServices
                .AnyAsync(es => es.EmployeeId == appointment.EmployeeId && es.ServiceId == appointment.ServiceId);
            if (!canEmployeeDoService)
            {
                ModelState.AddModelError("", "Bu çalışan seçilen servisi gerçekleştiremiyor.");
            }

            var employee = await _context.Employees.Include(e => e.WorkingDays).FirstOrDefaultAsync(e => e.EmployeeId == appointment.EmployeeId);
            if (employee == null)
            {
                ModelState.AddModelError("", "Geçersiz çalışan seçimi.");
            }
            else
            {
                var selectedTime = await _context.Times.FindAsync(appointment.TimeId);
                if (selectedTime != null)
                {
                    DateTime apptDate = selectedTime.Date.Date;
                    var workingDay = employee.WorkingDays
                        .FirstOrDefault(w => w.Date.Date == apptDate);

                    if (workingDay == null)
                    {
                        ModelState.AddModelError("", "Çalışan bu tarihte çalışmıyor.");
                    }
                    else
                    {
                        if (!(selectedTime.StartTime >= workingDay.StartTime &&
                              selectedTime.EndTime <= workingDay.EndTime))
                        {
                            ModelState.AddModelError("", "Çalışan seçilen saat aralığında çalışmıyor.");
                        }
                    }
                }
            }
            bool hasConflict = await _context.Appointments.AnyAsync(a => a.EmployeeId == appointment.EmployeeId && a.TimeId == appointment.TimeId);
            if (hasConflict)
            {
                ModelState.AddModelError("", "Bu çalışan bu saat aralığında başka bir randevuya sahip.");
            }

            var currentUserIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(currentUserIdString, out int currentUserId))
            {
                return Forbid();
            }

            appointment.UserId = currentUserId;

            if (!ModelState.IsValid)
            {
                var salons = _context.Salons.ToList();
                var services = _context.Services.ToList();
                var employees = _context.Employees.ToList();
                var times = _context.Times.Where(t => t.Selectable).ToList();

                ViewBag.Salons = new SelectList(salons, "SalonId", "Name", appointment.SalonId);
                ViewBag.Services = new SelectList(services, "ServiceId", "Name", appointment.ServiceId);
                ViewBag.Employees = new SelectList(employees, "EmployeeId", "FirstName", appointment.EmployeeId);
                ViewBag.Times = new SelectList(times, "TimeId", "StartTime", appointment.TimeId);

                return View(appointment);
            }

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        // GET: Appointments/Edit/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            var salons = _context.Salons.ToList();
            var services = _context.Services.ToList();
            var employees = _context.Employees.ToList();
            var times = _context.Times.Where(t => t.Selectable).ToList();

            ViewBag.Salons = new SelectList(salons, "SalonId", "Name", appointment.SalonId);
            ViewBag.Services = new SelectList(services, "ServiceId", "Name", appointment.ServiceId);
            ViewBag.Employees = new SelectList(employees, "EmployeeId", "FirstName", appointment.EmployeeId);
            ViewBag.Times = new SelectList(times, "TimeId", "StartTime", appointment.TimeId);

            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.AppointmentId) return NotFound();

            if (!ModelState.IsValid)
            {
                var salons = _context.Salons.ToList();
                var services = _context.Services.ToList();
                var employees = _context.Employees.ToList();
                var times = _context.Times.Where(t => t.Selectable).ToList();

                ViewBag.Salons = new SelectList(salons, "SalonId", "Name", appointment.SalonId);
                ViewBag.Services = new SelectList(services, "ServiceId", "Name", appointment.ServiceId);
                ViewBag.Employees = new SelectList(employees, "EmployeeId", "FirstName", appointment.EmployeeId);
                ViewBag.Times = new SelectList(times, "TimeId", "StartTime", appointment.TimeId);

                return View(appointment);
            }

            try
            {
                _context.Update(appointment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(appointment.AppointmentId))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Appointments/Delete/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments
                .Include(a => a.Employee)
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .Include(a => a.Time)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);

            if (appointment == null) return NotFound();

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
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }
    }
}
