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
using ProjectFinalEMDAAdvanced.ViewModels;

namespace ProjectFinalEMDAAdvanced.Controllers
{
    [Authorize]
    public class StaffsController : Controller
    {
        private readonly StaffDbContext _context;

        public StaffsController(StaffDbContext context)
        {
            _context = context;
        }

        // GET: Staffs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Staff.ToListAsync());
        }

        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Staffs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,In,TimeIn")] Staff staff)
        {
            staff.In = true;
            staff.TimeIn = DateTime.Now;
            staff.TimeOut = DateTime.Today;
            
            if (ModelState.IsValid)
            {
                _context.Add(staff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Reasons"] = _context.Reasons.Distinct()
               .OrderByDescending(n => n.ReasonCount)
               .Select(n => new SelectListItem()
               {
                   Value = n.Id.ToString(),
                   Text = n.Reason
               }).ToList();

            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,In")] Staff staff)
        {
            if (id != staff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (staff.In == true)
                    {
                        staff.TimeIn = DateTime.Now;
                        _context.Update(staff);
                    }
                    else if (staff.In == false)
                    {
                        staff.TimeOut = DateTime.Now;
                        _context.Update(staff);

                        //update sign outs table
                        
                        SignOuts signOuts = new SignOuts();
                        signOuts.Day = DateTime.Now;
                        signOuts.TimeOut = DateTime.Now;
                        // do a calculation
                        if (staff.TimeIn.Date == DateTime.Today.Date)
                        {
                            string hours = (DateTime.Now.TimeOfDay - staff.TimeIn.TimeOfDay).ToString();
                            signOuts.HoursIn = hours;
                        }
                        else
                        {
                            string hours = "n/a";
                            signOuts.HoursIn = hours;
                        }
                        int staffid = id;
                        Staff staffSelected = _context.Staff.Where(s => s.Id == staffid).SingleOrDefault();
                        signOuts.Staff = staffSelected;
                        int reasonid = 999;
                        Reasons reasonSelected = _context.Reasons.Where(r => r.Id == reasonid).SingleOrDefault();
                        signOuts.Reason = reasonSelected;

                        // update sign outs table
                        _context.Add(signOuts);
                        await _context.SaveChangesAsync();
                    }
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.Id))
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
            return View(staff);
        }

        // GET: Staffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            _context.Staff.Remove(staff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(int id)
        {
            return _context.Staff.Any(e => e.Id == id);
        }
    }
}
