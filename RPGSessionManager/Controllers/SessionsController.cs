using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPGSessionManager.Data;
using RPGSessionManager.Models;

namespace RPGSessionManager.Controllers
{
    [Authorize(Roles = "admin")]
    public class SessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.Id == id);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            var applicationDbContext = _context.Sessions.Include(s => s.Campaign).Include(s => s.Team);
            return View(applicationDbContext.ToList());
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = _context.Sessions
                .Include(s => s.Campaign)
                .Include(s => s.Team)
                .FirstOrDefault(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "About");
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "About");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,About,Date,CampaignId,TeamId")] Session session)
        {
            if (ModelState.IsValid)
            {
                _context.Add(session);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "About", session.CampaignId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "About", session.TeamId);
            return View(session);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = _context.Sessions.Find(id);
            if (session == null)
            {
                return NotFound();
            }
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "About", session.CampaignId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "About", session.TeamId);
            return View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,About,Date,CampaignId,TeamId")] Session session)
        {
            if (id != session.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(session);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.Id))
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
            ViewData["CampaignId"] = new SelectList(_context.Campaigns, "Id", "About", session.CampaignId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "About", session.TeamId);
            return View(session);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = _context.Sessions
                .Include(s => s.Campaign)
                .Include(s => s.Team)
                .FirstOrDefault(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AboutPartial()
        {
            return PartialView("_About");
        }
    }
}
