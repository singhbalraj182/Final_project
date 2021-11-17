using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFinalEMDAAdvanced.Data;
using ProjectFinalEMDAAdvanced.Models;

namespace ProjectFinalEMDAAdvanced.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/EventsApi")]
    public class EventsApiController : ControllerBase
    {
        private readonly StaffDbContext _context;

        public EventsApiController(StaffDbContext context)
        {
            _context = context;
        }

        // GET: api/EventsApi
        [HttpGet]
        public IEnumerable<Events> GetEvents()
        {
            return _context.Events;
        }

        // GET: api/EventsApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvents([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var events = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);

            if (events == null)
            {
                return NotFound();
            }

            return Ok(events);
        }

        // PUT: api/EventsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvents([FromRoute] int id, [FromBody] Events events)
        {
            //string UserEmail = User.Identity.Name;

            //if (!ModelState.IsValid || UserEmail == null || events.Email != UserEmail)
            //{
            //    return BadRequest(ModelState);
            //}

            if (id != events.Id)
            {
                return BadRequest();
            }

            _context.Entry(events).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!EventsExists(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
            }

            return NoContent();
        }

        // POST: api/EventsApi
        [HttpPost]
        public async Task<IActionResult> PostEvents([FromBody] Events events)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Events.Add(events);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvents", new { id = events.Id }, events);
        }

        // DELETE: api/EventsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvents([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var events = await _context.Events.SingleOrDefaultAsync(m => m.Id == id);
            if (events == null)
            {
                return NotFound();
            }

            _context.Events.Remove(events);
            await _context.SaveChangesAsync();

            return Ok(events);
        }

        private bool EventsExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}