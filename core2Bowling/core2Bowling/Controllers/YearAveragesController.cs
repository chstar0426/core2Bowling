using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using core2Bowling.Models;
using Microsoft.AspNetCore.Authorization;

namespace core2Bowling.Controllers
{
    [Authorize]
    public class YearAveragesController : Controller
    {
        
        private readonly BowlingContext _context;

        public YearAveragesController(BowlingContext context)
        {
            _context = context;
        }

        // GET: YearAverages
        public async Task<IActionResult> Index(string gameGroup)
        {

            var group = User.FindFirst("UserGroup").Value;

            if (string.IsNullOrEmpty(gameGroup))
            {
                if (group == "All")
                {
                    gameGroup = "RedPin";
                }
                else
                {
                    gameGroup = group;
                }

            }

            if (group != "All" && group != gameGroup)
            {
                return NotFound();
            }

            var bowlingContext = _context.YearAversges.Include(y => y.Bowler)
                .Where(y=>y.Bowler.Group == gameGroup)
                .OrderByDescending(y=>y.Year).ThenByDescending(y=>y.Average);

            return View(await bowlingContext.ToListAsync());
        }

        // GET: YearAverages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yearAverage = await _context.YearAversges
                .Include(y => y.Bowler)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (yearAverage == null)
            {
                return NotFound();
            }

            return View(yearAverage);
        }

        [Authorize(Policy = "AdminGroup")]
        // GET: YearAverages/Create
        public IActionResult Create()
        {
            ViewData["BowlerID"] = new SelectList(_context.Bowlers, "BowlerID", "BowlerID");
            return View();
        }

        // POST: YearAverages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,BowlerID,Year,Average,Bigo")] YearAverage yearAverage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(yearAverage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BowlerID"] = new SelectList(_context.Bowlers, "BowlerID", "BowlerID", yearAverage.BowlerID);
            return View(yearAverage);
        }

        [Authorize(Policy = "AdminGroup")]
        // GET: YearAverages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yearAverage = await _context.YearAversges.SingleOrDefaultAsync(m => m.ID == id);
            if (yearAverage == null)
            {
                return NotFound();
            }
            ViewData["BowlerID"] = new SelectList(_context.Bowlers, "BowlerID", "BowlerID", yearAverage.BowlerID);
            return View(yearAverage);
        }

        // POST: YearAverages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,BowlerID,Year,Average,Bigo")] YearAverage yearAverage)
        {
            if (id != yearAverage.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(yearAverage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YearAverageExists(yearAverage.ID))
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
            ViewData["BowlerID"] = new SelectList(_context.Bowlers, "BowlerID", "BowlerID", yearAverage.BowlerID);
            return View(yearAverage);
        }

        [Authorize(Policy = "AdminGroup")]
        // GET: YearAverages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yearAverage = await _context.YearAversges
                .Include(y => y.Bowler)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (yearAverage == null)
            {
                return NotFound();
            }

            return View(yearAverage);
        }

        // POST: YearAverages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var yearAverage = await _context.YearAversges.SingleOrDefaultAsync(m => m.ID == id);
            _context.YearAversges.Remove(yearAverage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YearAverageExists(int id)
        {
            return _context.YearAversges.Any(e => e.ID == id);
        }
    }
}
