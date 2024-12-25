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
    [Authorize(Roles = "ADMIN")]
    public class WorkingDaysController : Controller
    {
        private readonly SalonContext _context;

        public WorkingDaysController(SalonContext context)
        {
            _context = context;
        }

        // GET: WorkingDays
        public async Task<IActionResult> Index()
        {
            var workingDays = _context.WorkingDays.Include(w => w.Employee);
            return View(await workingDays.ToListAsync());
        }

        // GET: WorkingDays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workingDay = await _context.WorkingDays
                .Include(w => w.Employee)
                .FirstOrDefaultAsync(m => m.WorkingDayId == id);
            if (workingDay == null)
            {
                return NotFound();
            }

            return View(workingDay);
        }

        // GET: WorkingDays/Create
        public IActionResult Create()
        {
            ViewBag.Employees = _context.Employees.ToList();
            return View();
        }

        // POST: WorkingDays/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkingDay workingDay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workingDay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workingDay);
        }

        // GET: WorkingDays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workingDay = await _context.WorkingDays.FindAsync(id);
            if (workingDay == null)
            {
                return NotFound();
            }
            ViewBag.Employees = _context.Employees.ToList();
            return View(workingDay);
        }

        // POST: WorkingDays/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorkingDay workingDay)
        {
            if (id != workingDay.WorkingDayId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workingDay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkingDayExists(workingDay.WorkingDayId))
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
            return View(workingDay);
        }

        // GET: WorkingDays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workingDay = await _context.WorkingDays
                .Include(w => w.Employee)
                .FirstOrDefaultAsync(m => m.WorkingDayId == id);
            if (workingDay == null)
            {
                return NotFound();
            }

            return View(workingDay);
        }

        // POST: WorkingDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workingDay = await _context.WorkingDays.FindAsync(id);
            if (workingDay != null)
            {
                _context.WorkingDays.Remove(workingDay);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkingDayExists(int id)
        {
            return _context.WorkingDays.Any(e => e.WorkingDayId == id);
        }
    }
}
