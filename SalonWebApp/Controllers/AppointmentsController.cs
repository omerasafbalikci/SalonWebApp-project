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
            var salonContext = _context.Appointments
                .Include(a => a.Employee)
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .Include(a => a.Time)
                .Include(a => a.User);
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
            ViewBag.Salons = new SelectList(_context.Salons, "SalonId", "Name");
            ViewBag.Services = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Employees = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.Times = new SelectList(Enumerable.Empty<SelectListItem>());
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MEMBER,ADMIN")]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            var currentUserIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(currentUserIdString, out int currentUserId))
            {
                ModelState.AddModelError("", "Kullanıcı oturum açmamış.");
                return View(appointment);
            }

            appointment.UserId = currentUserId;

            // Ek İş Kuralları ve Doğrulama
            var salon = await _context.Salons.FindAsync(appointment.SalonId);
            if (salon == null)
            {
                ModelState.AddModelError("SalonId", "Geçerli bir salon seçiniz.");
            }

            var service = await _context.Services.FindAsync(appointment.ServiceId);
            if (service == null || service.SalonId != appointment.SalonId)
            {
                ModelState.AddModelError("ServiceId", "Seçilen servis geçersiz veya bu salona ait değil.");
            }

            bool canEmployeeDoService = await _context.EmployeeServices
                .AnyAsync(es => es.EmployeeId == appointment.EmployeeId && es.ServiceId == appointment.ServiceId);
            if (!canEmployeeDoService)
            {
                ModelState.AddModelError("EmployeeId", "Bu çalışan seçilen servisi gerçekleştiremiyor.");
            }

            var employee = await _context.Employees.Include(e => e.WorkingDays).FirstOrDefaultAsync(e => e.EmployeeId == appointment.EmployeeId);
            if (employee == null)
            {
                ModelState.AddModelError("EmployeeId", "Geçerli bir çalışan seçiniz.");
            }

            var selectedTime = await _context.Times.FindAsync(appointment.TimeId);
            if (selectedTime == null)
            {
                ModelState.AddModelError("TimeId", "Geçerli bir zaman dilimi seçiniz.");
            }

            if (!ModelState.IsValid)
            {
                // Dropdown listelerini yeniden doldur
                ViewBag.Salons = new SelectList(_context.Salons, "SalonId", "Name", appointment.SalonId);
                ViewBag.Services = new SelectList(await _context.Services.Where(s => s.SalonId == appointment.SalonId).ToListAsync(), "ServiceId", "Name", appointment.ServiceId);
                ViewBag.Employees = new SelectList(await _context.EmployeeServices.Where(es => es.ServiceId == appointment.ServiceId).Select(es => es.Employee).ToListAsync(), "EmployeeId", "FirstName", appointment.EmployeeId);
                ViewBag.Times = new SelectList(await _context.Times.Where(t => t.Selectable).ToListAsync(), "TimeId", "StartTime", appointment.TimeId);
                return View(appointment);
            }

            try
            {
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Appointments");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Randevu kaydedilirken bir hata oluştu.");
                ViewBag.Salons = new SelectList(_context.Salons, "SalonId", "Name", appointment.SalonId);
                ViewBag.Services = new SelectList(await _context.Services.Where(s => s.SalonId == appointment.SalonId).ToListAsync(), "ServiceId", "Name", appointment.ServiceId);
                ViewBag.Employees = new SelectList(await _context.EmployeeServices.Where(es => es.ServiceId == appointment.ServiceId).Select(es => es.Employee).ToListAsync(), "EmployeeId", "FirstName", appointment.EmployeeId);
                ViewBag.Times = new SelectList(await _context.Times.Where(t => t.Selectable).ToListAsync(), "TimeId", "StartTime", appointment.TimeId);
                return View(appointment);
            }
        }



        // GET: Appointments/Edit/5
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            var salons = _context.Salons.ToList();
            var services = _context.Services.Where(s => s.SalonId == appointment.SalonId).ToList();
            var employees = _context.EmployeeServices
                .Where(es => es.ServiceId == appointment.ServiceId)
                .Select(es => es.Employee)
                .Distinct()
                .ToList();
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
                var services = _context.Services.Where(s => s.SalonId == appointment.SalonId).ToList();
                var employees = _context.EmployeeServices
                    .Where(es => es.ServiceId == appointment.ServiceId)
                    .Select(es => es.Employee)
                    .Distinct()
                    .ToList();
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

        #region Dinamik Dropdown Helper Metotları

        /// <summary>
        /// Belirli bir salona ait servisleri JSON formatında döndürür.
        /// </summary>
        /// <param name="salonId">Salon ID'si</param>
        /// <returns>Servis listesi JSON</returns>
        [HttpGet]
        [Authorize(Roles = "MEMBER,ADMIN")]
        public async Task<IActionResult> GetServicesForSalon(int salonId)
        {
            var services = await _context.Services
                .Where(s => s.SalonId == salonId)
                .Select(s => new
                {
                    s.ServiceId,
                    s.Name
                })
                .ToListAsync();

            return Json(services);
        }

        /// <summary>
        /// Belirli bir servisi yapabilen çalışanları JSON formatında döndürür.
        /// </summary>
        /// <param name="serviceId">Servis ID'si</param>
        /// <returns>Çalışan listesi JSON</returns>
        [HttpGet]
        [Authorize(Roles = "MEMBER,ADMIN")]
        public async Task<IActionResult> GetEmployeesForService(int serviceId)
        {
            var employees = await _context.EmployeeServices
                .Where(es => es.ServiceId == serviceId)
                .Include(es => es.Employee)
                .Select(es => new
                {
                    es.Employee.EmployeeId,
                    FullName = es.Employee.FirstName + " " + es.Employee.LastName
                })
                .Distinct()
                .ToListAsync();

            return Json(employees);
        }

        /// <summary>
        /// Belirli bir çalışan, servis ve tarih için müsait zaman dilimlerini JSON formatında döndürür.
        /// </summary>
        /// <param name="employeeId">Çalışan ID'si</param>
        /// <param name="serviceId">Servis ID'si</param>
        /// <param name="date">Tarih</param>
        /// <returns>Mevcut zaman dilimleri JSON</returns>
        [HttpGet]
        [Authorize(Roles = "MEMBER,ADMIN")]
        public async Task<IActionResult> GetAvailableTimes(int employeeId, int serviceId, DateTime date)
        {
            // Çalışanın çalışma günlerini kontrol et
            var employee = await _context.Employees
                .Include(e => e.WorkingDays)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null)
                return Json(new { error = "Çalışan bulunamadı." });

            // Çalışanın belirli bir tarihte çalışıp çalışmadığını kontrol et
            var workingDay = employee.WorkingDays
                .FirstOrDefault(w => w.Date.Date == date.Date);

            if (workingDay == null)
            {
                return Json(new { error = "Çalışan bu tarihte çalışmıyor." });
            }

            // Çalışanın çalışma saatleri içinde olup olmadığını kontrol et
            // Öncelikle servis süresini al (eğer varsa)
            var service = await _context.Services.FindAsync(serviceId);
            if (service == null)
            {
                return Json(new { error = "Servis bulunamadı." });
            }

            // Çalışanın çalışma saatlerine uygun zaman dilimlerini al
            var availableTimes = await _context.Times
                .Where(t => t.Selectable &&
                            t.Date.Date == date.Date &&
                            t.StartTime >= workingDay.StartTime &&
                            t.EndTime <= workingDay.EndTime)
                .ToListAsync();

            // Çalışanın mevcut randevularını al ve çakışan zaman dilimlerini hariç tut
            var bookedTimeIds = await _context.Appointments
                .Where(a => a.EmployeeId == employeeId && a.Time.Date == date.Date)
                .Select(a => a.TimeId)
                .ToListAsync();

            var freeTimes = availableTimes
                .Where(t => !bookedTimeIds.Contains(t.TimeId))
                .Select(t => new
                {
                    t.TimeId,
                    StartTime = t.StartTime.ToString(@"hh\:mm")
                })
                .ToList();

            return Json(freeTimes);
        }

        #endregion
    }
}
