using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using core2Bowling.Models;

namespace core2Bowling.Controllers
{
    public class TeamMembersController : Controller
    {
        private readonly BowlingContext _context;

        public TeamMembersController(BowlingContext context)
        {
            _context = context;
        }

        // GET: TeamMembers
        public async Task<IActionResult> Index()
        {

            var bowlingContext = _context.TeamMembers
                .Include(t => t.Bowler)
                .ThenInclude(b=>b.BowlerAverage)
                .Include(t => t.Team)
                .ThenInclude(s => s.SubGame)
                .Where(t=>t.Team.SubGame.GameID==1)
                .AsNoTracking();

            return View(await bowlingContext.ToListAsync());

        }

        // GET: TeamMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMembers
                .Include(t => t.Bowler)
                .Include(t => t.Team)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (teamMember == null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        // GET: TeamMembers/Create
        public IActionResult Create()
        {
            ViewData["BowlerID"] = new SelectList(_context.Bowlers, "BowlerID", "BowlerID");
            ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "ID");
            return View();
        }

        // POST: TeamMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TeamID,BowlerID,Sequence,Score")] TeamMember teamMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teamMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BowlerID"] = new SelectList(_context.Bowlers, "BowlerID", "BowlerID", teamMember.BowlerID);
            ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "ID", teamMember.TeamID);
            return View(teamMember);
        }

        // GET: TeamMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var teamMembers = await _context.TeamMembers
                .Include(t => t.Bowler)
                .ThenInclude(b=>b.BowlerAverage)
                .Include(t => t.Team)
                .Where(t=>t.Team.SubGameID==id && t.Team.SubGame.GameID == 1)
                .AsNoTracking().ToListAsync();

            if (teamMembers == null)
            {
                return NotFound();
            }
            return View(teamMembers);
        }

        // POST: TeamMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, int[] inputScores)
        {
            if (id == null) 
            { 
                return NotFound(); 
            }

            var teamMembers = await _context.TeamMembers
                .Include(t => t.Bowler)
                .ThenInclude(b=>b.BowlerAverage)
               .Include(t => t.Team)
               .Where(t => t.Team.SubGameID == id && t.Team.SubGame.GameID == 1)
               .AsNoTracking().ToListAsync();

            if (teamMembers ==null)
            {
                return NotFound();
            }
            
            

            try
            {
                int i = 0;
                foreach (var item in teamMembers)
                {
                    if (item.Score != inputScores[i] + item.Bowler.BowlerAverage.Handicap)
                    {
                        item.Score = inputScores[i] + item.Bowler.BowlerAverage.Handicap;
                        _context.Update(item);
                    }
                    i++;

                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {

                ModelState.AddModelError("", "TeamMember_Edit_Error")
  ;           }

            
            return View(teamMembers);
        }

        // GET: TeamMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMembers
                .Include(t => t.Bowler)
                .Include(t => t.Team)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (teamMember == null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        // POST: TeamMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamMember = await _context.TeamMembers.SingleOrDefaultAsync(m => m.ID == id);
            _context.TeamMembers.Remove(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamMemberExists(int id)
        {
            return _context.TeamMembers.Any(e => e.ID == id);
        }
    }
}
