using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEMDAAdvanced.Business;
using ProjectFinalEMDAAdvanced.Data;
using ProjectFinalEMDAAdvanced.Models;
using ProjectFinalEMDAAdvanced.ViewModels;

namespace ProjectFinalEMDAAdvanced.Controllers
{
    [Authorize]
    public class SignOutsController : Controller
    {
        private readonly StaffDbContext _context;
        private readonly IDbCalls _dbcalls;

        public SignOutsController(StaffDbContext context, IDbCalls dbCalls)
        {
            _context = context;
            _dbcalls = dbCalls;
        }

        // GET: SignOuts
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: SignOuts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signOuts = await _context.SignOuts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (signOuts == null)
            {
                return NotFound();
            }

            return View(signOuts);
        }

        // GET: SignOuts/Create
        public IActionResult Create()
        {
            ViewData["Staff"] = _context.Staff.Distinct()
                .OrderBy(n => n.FirstName)
                .Select(n => new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.FirstName + " " + n.LastName
                }).ToList();

            ViewData["StaffIn"] = _context.Staff.Distinct()
                .OrderBy(n => n.Id)
                .Select(n => new SelectListItem()
                {
                    Value = n.Id.ToString(),
                    Text = n.In.ToString()
                }).ToList();

            ViewData["Reasons"] = _context.Reasons.Distinct()
                .OrderByDescending(n => n.ReasonCount)
                .Select(n => new SelectListItem()
                {
                    Value = n.Id.ToString(),
                    Text = n.Reason
                }).ToList();

            return View();
        }

        // POST: SignOuts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Day,TimeOut,HoursIn,Reason,Staff,StaffIn")] CreateSignOutVM createSignOutVM)
        {
            // staff is signing in
            if (createSignOutVM.StaffIn == true)
            {
                int staffid = createSignOutVM.Staff.Id;
                Staff staff = (Staff)_context.Staff.Where(s => s.Id == staffid).SingleOrDefault();

                staff.In = true;
                staff.TimeIn = DateTime.Now;

                _context.Update(staff);
                await _context.SaveChangesAsync();
                return Redirect("~/Home/Index#UpdateStatus");
            }
            // staff is signing out
            if (ModelState.IsValid)
            {
                int reasonid = createSignOutVM.Reason.Id;
                Reasons reason = (Reasons)_context.Reasons.Where(r => r.Id == reasonid).SingleOrDefault();
                int staffid = createSignOutVM.Staff.Id;
                Staff staff = (Staff)_context.Staff.Where(s => s.Id == staffid).SingleOrDefault();

                // increment reason count
                if (reasonid != 999)
                {
                    _dbcalls.IncrementReasonCount(reasonid);
                }
                
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
                signOuts.Staff = staff;
                signOuts.Reason = reason;
                    
                // update sign outs table
                _context.Add(signOuts);
                await _context.SaveChangesAsync();
                    
                // update the staff table 'In' field and time field
                staff.In = false;
                staff.TimeOut = DateTime.Now;
                    
                _context.Update(staff);
                await _context.SaveChangesAsync();
                return Redirect("~/Home/Index#UpdateStatus");
            }
            return NotFound();
        }

        // GET: SignOuts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signOuts = await _context.SignOuts.FindAsync(id);
            if (signOuts == null)
            {
                return NotFound();
            }
            return View(signOuts);
        }

        // POST: SignOuts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Day,TimeOut,HoursIn")] SignOuts signOuts)
        {
            if (id != signOuts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(signOuts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SignOutsExists(signOuts.Id))
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
            return View(signOuts);
        }

        // GET: SignOuts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signOuts = await _context.SignOuts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (signOuts == null)
            {
                return NotFound();
            }

            return View(signOuts);
        }

        // POST: SignOuts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var signOuts = await _context.SignOuts.FindAsync(id);
            _context.SignOuts.Remove(signOuts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SignOutsExists(int id)
        {
            return _context.SignOuts.Any(e => e.Id == id);
        }
    }
}
