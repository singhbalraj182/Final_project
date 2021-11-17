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
    public class EventsController : Controller
    {
        private readonly StaffDbContext _context;

        public EventsController(StaffDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            var events = await _context.Events
                .SingleOrDefaultAsync(m => m.Id == id);
            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EventColor,Start,End,Title,IsFullDay,Days,Weeks,Staff")] Events events)
        {
            if (ModelState.IsValid)
            {
                _context.Add(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //if (ModelState.IsValid)
            //{
            //    //check if there is a fullday or repeating weeks or days
            //    if (events.IsFullDay || events.Days > 0 || events.Weeks > 0)
            //    {
            //        foreach (var booking in DayWeeksAllDayMods.EventCalc(events))
            //        {
            //            _context.Add(booking);
            //        }
            //    }
            //    //pass through the event and look for clashes where its the same room after today
            //    DayWeeksAllDayMods.DoTheDatesOverlap(_context.Events.Where(e => e.ResourceId == events.ResourceId && e.End > DateTime.Now).ToList(), events);

            //    //We have a clash
            //    if (DayWeeksAllDayMods.WeHaveAClash == true)
            //    {
            //        return RedirectToAction("Clash");
            //    }
            //    else
            //    {//we dont have a clash
            //        _context.Add(events);
            //        await _context.SaveChangesAsync();
            //        return View();
            //    }
            //}


            return View(events);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Events.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EventColor,Start,End,Title,IsFullDay,Days,Weeks,Staff")] Events events)
        {
            if (id != events.Id)
            {
                //return NotFound();
                events.Id = id;
            }

            if (ModelState.IsValid && events.End > events.Start)
            {
                try
                {
                    _context.Update(events);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventsExists(events.Id))
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
            //return View(events);
            return RedirectToAction(nameof(Index));
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Events
                .SingleOrDefaultAsync(m => m.Id == id);
            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var events = await _context.Events.FindAsync(id);
            _context.Events.Remove(events);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventsExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
