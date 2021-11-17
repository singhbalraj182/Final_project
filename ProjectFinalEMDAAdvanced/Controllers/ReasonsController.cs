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
    public class ReasonsController : Controller
    {
        private readonly StaffDbContext _context;

        public ReasonsController(StaffDbContext context)
        {
            _context = context;
        }

        // GET: Reasons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reasons.ToListAsync());
        }

        // GET: Reasons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reasons = await _context.Reasons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reasons == null)
            {
                return NotFound();
            }

            return View(reasons);
        }

        // GET: Reasons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Reason")] Reasons reasons)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reasons);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reasons);
        }

        // GET: Reasons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reasons = await _context.Reasons.FindAsync(id);
            if (reasons == null)
            {
                return NotFound();
            }
            return View(reasons);
        }

        // POST: Reasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Reason,ReasonCount")] Reasons reasons)
        {
            if (id != reasons.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reasons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReasonsExists(reasons.Id))
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
            return View(reasons);
        }

        // GET: Reasons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reasons = await _context.Reasons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reasons == null)
            {
                return NotFound();
            }

            return View(reasons);
        }

        // POST: Reasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reasons = await _context.Reasons.FindAsync(id);
            _context.Reasons.Remove(reasons);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReasonsExists(int id)
        {
            return _context.Reasons.Any(e => e.Id == id);
        }
    }
}
