using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RPGSessionManager.Data;
using RPGSessionManager.Models;

namespace RPGSessionManager.Controllers
{
    public class CharactersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharactersController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool CharacterExists(int id)
        {
            return _context.Characters.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var applicationDbContext = _context.Characters.Include(c => c.Player).Include(c => c.Team);
            return View(applicationDbContext.ToList());
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = _context.Characters
                .Include(c => c.Player)
                .Include(c => c.Team)
                .FirstOrDefault(m => m.Id == id);
            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Name");
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "About");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,FirstName,LastName,Profession,TeamId,PlayerId")] Character character)
        {
            if (ModelState.IsValid)
            {
                _context.Add(character);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Name", character.PlayerId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "About", character.TeamId);
            return View(character);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = _context.Characters.Find(id);
            if (character == null)
            {
                return NotFound();
            }
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Name", character.PlayerId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "About", character.TeamId);
            return View(character);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,FirstName,LastName,Profession,TeamId,PlayerId")] Character character)
        {
            if (id != character.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(character);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterExists(character.Id))
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
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Name", character.PlayerId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "About", character.TeamId);
            return View(character);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var character = _context.Characters
                .Include(c => c.Player)
                .Include(c => c.Team)
                .FirstOrDefault(m => m.Id == id);
            if (character == null)
            {
                return NotFound();
            }

            return View(character);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character != null)
            {
                _context.Characters.Remove(character);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
