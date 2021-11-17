using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ProjectFinalEMDAAdvanced.Data;
using ProjectFinalEMDAAdvanced.Models;

namespace ProjectFinalEMDAAdvanced.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly StaffDbContext _context;

        public HomeController(StaffDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
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
                .Where(i => i.Id != 999)
                .Select(n => new SelectListItem()
                {
                    Value = n.Id.ToString(),
                    Text = n.Reason
                }).ToList();

            List<SignOuts> StaffOut = new List<SignOuts>();
            StaffOut.AddRange(_context.SignOuts
                .Include(s => s.Staff)
                .Include(r => r.Reason)
                .GroupBy(g => g.Staff.Id)
                .Select(e => e.LastOrDefault())
                .ToList());

            List<SignOuts> StaffOutOrder = new List<SignOuts>();
            StaffOutOrder.AddRange(StaffOut
                .OrderByDescending(s => s.TimeOut.TimeOfDay)
                .ToList());
            
            ViewData["StaffOut"] = StaffOutOrder;

            List<Staff> StaffStatus = new List<Staff>();
            StaffStatus.AddRange(_context.Staff.Distinct().OrderBy(s => s.FirstName).ToList());

            ViewData["StaffStatus"] = StaffStatus;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
