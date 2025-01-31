using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPGSessionManager.Data;
using RPGSessionManager.Models;

namespace RPGSessionManager.Controllers
{
    public class CampaignsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CampaignsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Campaigns.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns
                .FirstOrDefaultAsync(m => m.Id == id);
            if (campaign == null)
            {
                return NotFound();
            }

            return View(campaign);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,About,HasEnded")] Campaign campaign)
        {

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Model Error: {error.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(campaign);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(campaign);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = _context.Campaigns.Find(id);
            if (campaign == null)
            {
                return NotFound();
            }
            return View(campaign);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,About,HasEnded")] Campaign campaign)
        {
            if (id != campaign.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(campaign);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampaignExists(campaign.Id))
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
            return View(campaign);
        }

        private bool CampaignExists(int id)
        {
            return _context.Campaigns.Any(e => e.Id == id);
        }
    }
}
