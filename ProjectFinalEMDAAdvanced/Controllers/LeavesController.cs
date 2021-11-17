using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEMDAAdvanced.Data;
using ProjectFinalEMDAAdvanced.Models;

namespace ProjectFinalEMDAAdvanced.Controllers
{
    [Authorize]
    public class LeavesController : Controller
    {
        private readonly StaffDbContext _context;

        public LeavesController(StaffDbContext context)
        {
            _context = context;
        }

        // GET: Leaves
        public async Task<IActionResult> Index()
        {
            List<Leave> StaffNames = new List<Leave>();
            StaffNames.AddRange(_context.Leave
                .Include(s => s.Staff)
                .ToList());

            ViewData["StaffNames"] = StaffNames;

            return View();
        }

        // GET: Leaves/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leave = await _context.Leave
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leave == null)
            {
                return NotFound();
            }

            return View(leave);
        }

        // GET: Leaves/Create
        public IActionResult Create()
        {
            ViewData["Staff"] = _context.Staff.Distinct()
                .OrderBy(n => n.FirstName)
                .Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.FirstName + " " + n.LastName
                }).ToList();

            return View();
        }

        // POST: Leaves/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Staff,StartDate,EndDate,TotalDays,Accepted")] Leave leave)
        {
            int staffid = leave.Staff.Id;
            Staff staff = _context.Staff.Where(s => s.Id == staffid).SingleOrDefault();

            leave.Staff = staff;
            // set leave accepted to false
            leave.Accepted = false;
            
            if (ModelState.IsValid)
            {
                _context.Add(leave);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leave);
        }

        // GET: Leaves/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leave = await _context.Leave.FindAsync(id);
            if (leave == null)
            {
                return NotFound();
            }

            int staffid = _context.Leave
                .Include(s => s.Staff)
                .Where(l => l.Id == id)
                .Select(s => s.Staff.Id)
                .SingleOrDefault();

            ViewData["StaffId"] = staffid;

           return View(leave);
        }

        // POST: Leaves/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,EndDate,TotalDays,Accepted")] Leave leave)
        {
            if (id != leave.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (leave.Accepted == true)
                    {
                        int staffid = _context.Leave
                            .Include(s => s.Staff)
                            .Where(l => l.Id == id)
                            .Select(s => s.Staff.Id)
                            .SingleOrDefault();

                        Staff staff = (Staff)_context.Staff.Where(s => s.Id == staffid).SingleOrDefault();

                        Events events = new Events();
                        events.IsFullDay = true;
                        events.Start = leave.StartDate;
                        events.End = leave.EndDate;
                        events.Days = leave.TotalDays;
                        events.Staff = staff;
                        events.Title = staff.FirstName + " " + staff.LastName + " Annual Leave";

                        _context.Add(events);
                    }
                    _context.Update(leave);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveExists(leave.Id))
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
            return View(leave);
        }

        // GET: Leaves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leave = await _context.Leave
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leave == null)
            {
                return NotFound();
            }

            return View(leave);
        }

        // POST: Leaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leave = await _context.Leave.FindAsync(id);
            _context.Leave.Remove(leave);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveExists(int id)
        {
            return _context.Leave.Any(e => e.Id == id);
        }
    }
}
