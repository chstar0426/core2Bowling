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

    [Authorize(Policy = "AdminGroup")]
    public class BowlersController : Controller
    {
        private readonly BowlingContext _context;

        public BowlersController(BowlingContext context)
        {
            _context = context;
        }

        // GET: Bowlers
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

            return View(await _context.Bowlers.Where(b=>b.Group.Contains(gameGroup))
            .Include(b=>b.BowlerAverage)
            .OrderBy(b=>b.InActivity).ThenBy(b=>b.BowlerID)
            .AsNoTracking()
            .ToListAsync());
        }

        // GET: Bowlers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bowler = await _context.Bowlers
                .Include(b=>b.BowlerAverage)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.BowlerID == id);

            if (bowler == null)
            {
                return NotFound();
            }

            return View(bowler);
        }

        // GET: Bowlers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bowlers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BowlerID,Name,Group,InActivity, BowlerAverage, RegisterDate, LeaveDate, Bigo")] Bowler bowler)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bowler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bowler);
        }

        // GET: Bowlers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bowler = await _context.Bowlers
                .Include(b=>b.BowlerAverage)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.BowlerID == id);

            if (bowler == null)
            {
                return NotFound();
            }

            return View(bowler);
        }

        // POST: Bowlers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var bowler = await _context.Bowlers
                .Include(b => b.BowlerAverage)
                .SingleOrDefaultAsync(m => m.BowlerID == id);

            bowler.LeaveDate = bowler.InActivity ? null : bowler.LeaveDate;

            if (bowler == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Bowler>(bowler, "",
                b=>b.Name,b=>b.Group, b=>b.InActivity, b=>b.BowlerAverage,
                b=>b.RegisterDate, b=>b.LeaveDate, b=>b.Bigo))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Bowler_Edit_Post_Error");

                    throw;
                }

            }
            
            return View(bowler);
        }

        // GET: Bowlers/Delete/5
        public async Task<IActionResult> Delete(string id,bool? ErrorMessage=false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bowler = await _context.Bowlers
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.BowlerID == id);

            if (bowler == null)
            {
                return NotFound();
            }

            if (ErrorMessage.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "삭제중 오류발생 _확인 및 다시 실행";
            }

            return View(bowler);
        }

        // POST: Bowlers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {

            var bowler = await _context.Bowlers
                //.Include(b=>b.BowlerAverage) -- Delete Cascading
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.BowlerID == id);
           

            if (bowler == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Bowlers.Remove(bowler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (DbUpdateException)
            {

                return RedirectToAction(nameof(Delete), new { id = id, ErrorMessage = true });
            }
           
        }

        private bool BowlerExists(string id)
        {
            return _context.Bowlers.Any(e => e.BowlerID == id);
        }
    }
}
