using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPGSessionManager.Data;
using RPGSessionManager.Models;

namespace RPGSessionManager.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayersController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_context.Players.ToList());
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = _context.Players
                .FirstOrDefault(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] Player player)
        {
            if (ModelState.IsValid)
            {
                _context.Add(player);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = _context.Players.Find(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] Player player)
        {
            if (id != player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
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
            return View(player);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = _context.Players
                .FirstOrDefault(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.Include(c => c.Characters)
                                               .FirstOrDefaultAsync(c => c.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Characters.RemoveRange(player.Characters);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
