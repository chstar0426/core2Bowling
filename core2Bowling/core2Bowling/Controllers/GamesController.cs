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
    public class GamesController : Controller
    {
        private readonly BowlingContext _context;

        public GamesController(BowlingContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            return View(await _context.Games.OrderByDescending(g=>g.Playtime).ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .SingleOrDefaultAsync(m => m.ID == id);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Playtime,Place,GameKind, Panelty, GameContent,Group")] Game game)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(game);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (DbUpdateException)
            {

                ModelState.AddModelError("", "Game_Create_Error");
            }
            
            return View(game);
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AutoPractice()
        {
            
            var game = new Game
            {
                GameKind = GameKind.연습게임,
                Penalty=0,
                Group = "Family",
                Place = "현대볼링장",
                Playtime = DateTime.Now

            };

            await _context.Games.AddAsync(game);

            var subgame = new SubGame
            {
                GameID = game.ID,
                Round = 1

            };

            await _context.SubGames.AddAsync (subgame);

            var team = new Team
            {
                SubGameID = subgame.ID,
                TeamName = "A",
                TeamOrder = 0
            };

            await _context.Teams.AddAsync(team);

            var teamMember = new TeamMember
            {
                BowlerID = "fg001",
                Score = 0,
                Average = _context.BowlerAverages.Find("fg001").Average,
                Sequence = 0,
                TeamID = team.ID

            };

            await _context.TeamMembers.AddAsync(teamMember);

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));

        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AutoPlay()
        {

            var game = new Game
            {
                GameKind = GameKind.비정기전,
               Penalty=0,
                Group = "Family",
                Place = "현대볼링장",
                Playtime = DateTime.Now

            };

            await _context.Games.AddAsync(game);

            var subgame = new SubGame
            {
                GameID = game.ID,
                Round = 1

            };

            await _context.SubGames.AddAsync(subgame);

            var teams = new List<Team>()
            {
                new Team
                {
                    SubGameID = subgame.ID,
                    TeamName = "A",
                    TeamOrder = 0

                },
                new Team
                {
                    SubGameID = subgame.ID,
                    TeamName = "B",
                    TeamOrder = 1

                }

            };

            await _context.Teams.AddRangeAsync(teams);

            var teamMembers = new List<TeamMember>()
            {
                new TeamMember
                {
                    BowlerID = "rp001",
                    Score = 0,
                    Average= _context.BowlerAverages.Find("rp001").Average,
                    Sequence = 0,
                    TeamID = teams[0].ID
                    
                },
                new TeamMember
                {
                    BowlerID = "rp002",
                    Score = 0,
                   Average= _context.BowlerAverages.Find("rp002").Average,
                    Sequence = 1,
                    TeamID = teams[0].ID

                },
                new TeamMember
                {
                    BowlerID = "rp003",
                    Score = 0,
                    Average= _context.BowlerAverages.Find("rp003").Average,
                    Sequence = 0,
                    TeamID = teams[1].ID

                },
                new TeamMember
                {
                    BowlerID = "rp004",
                    Score = 0,
                   Average= _context.BowlerAverages.Find("rp004").Average,
                    Sequence = 1,
                    TeamID = teams[1].ID

                }

            };

            await _context.TeamMembers.AddRangeAsync(teamMembers);

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));

        }



        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.SingleOrDefaultAsync(m => m.ID == id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.SingleOrDefaultAsync(m => m.ID == id);

            if (await TryUpdateModelAsync<Game>(game,"",
                g=>g.Playtime, g=>g.Place, g=>g.GameKind, g=>g.Penalty, g=>g.GameContent))
            {

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                catch (DbUpdateException)
                {

                    ModelState.AddModelError("", "Game_Edit_Error");
                }

            }
            
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .SingleOrDefaultAsync(m => m.ID == id);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.SingleOrDefaultAsync(m => m.ID == id);
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.ID == id);
        }
    }
}
