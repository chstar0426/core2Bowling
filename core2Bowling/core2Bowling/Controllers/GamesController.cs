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
    public class GamesController : Controller
    {
        private readonly BowlingContext _context;

        public GamesController(BowlingContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index(string gameGroup, int gameKind=3, bool cVal = false)
        {

             int pageIndex = 0;
             int pageSize = 10;

            //[1] 쿼리스트링에 따른 페이지 보여주기
            if (!string.IsNullOrEmpty(Request.Query["Page"].ToString()))
            {
                // Page는 보여지는 쪽은 1, 2, 3, ... 코드단에서는 0, 1, 2, ...
                pageIndex = Convert.ToInt32(Request.Query["Page"]) - 1;
            }
            
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

            //[2] 쿠키를 사용한 리스트 페이지 번호 유지 적용(Optional): 
            //    100번째 페이지 보고 있다가 다시 리스트 왔을 때 100번째 페이지 표시
            if (cVal)
            {
                if (!String.IsNullOrEmpty(Request.Cookies["PageIndex"]))
                {
                    pageIndex = Convert.ToInt32(Request.Cookies["PageIndex"]) - 1;
                }
                else
                {
                    pageIndex = 0;
                }
                
            }
            
            var games = new List<Game>();

            if (gameKind < 3)
            {
                games = await _context.Games.Where(g => g.Group.Contains(gameGroup) && g.GameKind == (GameKind)gameKind)
                .OrderByDescending(g => g.Playtime)
                 .Skip((pageIndex) * pageSize).Take(pageSize).ToListAsync();

                ViewBag.Count = _context.Games.Where(g => g.Group.Contains(gameGroup) && g.GameKind == (GameKind)gameKind).Count();

            }
            else
            {
                games = await _context.Games.Where(g => g.Group.Contains(gameGroup))
                .OrderByDescending(g => g.Playtime)
                 .Skip((pageIndex) * pageSize).Take(pageSize).ToListAsync();

                ViewBag.Count = _context.Games.Where(g => g.Group.Contains(gameGroup)).Count();

            }


            ViewBag.PageIndex = pageIndex + 1;
            ViewBag.Etc = "&gameGroup=" + gameGroup + "&gameKind=" +  gameKind;
            
            return View(games);

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
        [Authorize(Policy = "AdminGroup")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Playtime,Place,GameKind,GameContent,bCalTotal,Penalty,Group,bFine, bHandicap, GameMemo")] Game game)
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
                GameKind = GameKind.개인기록,
                Penalty = 0,
                Group = "Family",
                Place = "현대볼링장",
                bFine = false,
                bHandicap =false,
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
                Penalty = 0,
                Group = "Family",
                Place = "현대볼링장",
                bFine = false,
                bHandicap=false,
                bCalTotal = true,
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

                },
                new Team
                {
                    SubGameID = subgame.ID,
                    TeamName = "C",
                    TeamOrder = 2

                },
                new Team
                {
                    SubGameID = subgame.ID,
                    TeamName = "D",
                    TeamOrder = 3

                }

            };

            await _context.Teams.AddRangeAsync(teams);

            var teamMembers = new List<TeamMember>()
            {
                new TeamMember
                {
                    BowlerID = "fg001",
                    Score = 0,
                    Average= _context.BowlerAverages.Find("fg001").Average,
                    Sequence = 0,
                    TeamID = teams[0].ID
                },
                new TeamMember
                {
                    BowlerID = "fg002",
                    Score = 0,
                    Average= _context.BowlerAverages.Find("fg002").Average,
                    Sequence = 0,
                    TeamID = teams[1].ID

                },
                new TeamMember
                {
                    BowlerID = "fg003",
                    Score = 0,
                    Average= _context.BowlerAverages.Find("fg003").Average,
                    Sequence = 0,
                    TeamID = teams[2].ID

                },
                new TeamMember
                {
                    BowlerID = "fg004",
                    Score = 0,
                   Average= _context.BowlerAverages.Find("fg004").Average,
                    Sequence = 0,
                    TeamID = teams[3].ID

                }


            };

            await _context.TeamMembers.AddRangeAsync(teamMembers);

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));

        }



        // GET: Games/Edit/5
        [Authorize(Policy = "AdminGroup")]
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
                g=>g.Playtime, g=>g.Place, g=>g.GameKind, g => g.GameContent, g=>g.bCalTotal, g=>g.Penalty, 
                g=>g.bFine, g=>g.bHandicap, g=>g.GameMemo))
            {

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { gameGroup = game.Group, gameKind = Convert.ToInt32(Request.Cookies["GameKind"]), cVal=true });

                }
                catch (DbUpdateException)
                {

                    ModelState.AddModelError("", "Game_Edit_Error");
                }

            }
            
            return View(game);
        }

        // GET: Games/Delete/5
        [Authorize(Policy = "AdminGroup")]
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
            return RedirectToAction(nameof(Index), new { gameGroup = game.Group, cVal = true });

        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.ID == id);
        }
    }
}
