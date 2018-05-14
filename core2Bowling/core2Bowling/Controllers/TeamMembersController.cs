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
        public async Task<IActionResult> Index(int? id)
        {
          
            if (id == null)
            {
                return NotFound();
                
            }
            
            var bowlingContext = _context.TeamMembers
                .Include(t => t.Bowler)
                    .ThenInclude(b=>b.BowlerAverage)
                .Include(t => t.Team)
                    .ThenInclude(s => s.SubGame)
                .Where(t=>t.Team.SubGame.GameID==id)
                .OrderBy(t => t.Team.SubGameID)
                .ThenBy(t => t.Team.TeamOrder)
                .ThenBy(t => t.Sequence)
                .AsNoTracking();

            ViewData["Game"] = _context.Games.Where(g => g.ID == id).SingleOrDefault();
            



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
        public async Task<IActionResult> CreateTeam(int Id)
        {
            
            var bowlers = await _context.Bowlers
                .Include(b => b.BowlerAverage)
                .AsNoTracking().ToListAsync();


            ViewData["BowlerID"] = bowlers;
            //ViewData["BowlerID"] = new SelectList(bowlers, "BowlerID", "Name");

            return View(Id);

        }

        // POST: TeamMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTeam(int Id, int[] teamCnt, string[] selectLst)
        {
            int teamcnt = 0;
            int cnt = 0;
            int teamAsc = 65;
            int round = 1;
            SubGame subgame = new SubGame();
            List<Team> team = new List<Team>();
            List<TeamMember> teamMembers = new List<TeamMember>();
            
            if (_context.SubGames.Where(s => s.GameID == Id).Count()>0)
            {
                round = _context.SubGames.Where(s => s.GameID == Id).Max(s => s.Round) + 1;
            }
            
            subgame.GameID = Id;
            subgame.Round = round;

            await _context.SubGames.AddAsync(subgame);

            foreach (var item in teamCnt)
            {
                team.Add(new Team
                {
                    SubGameID = subgame.ID,
                    TeamName = ((Char)teamAsc).ToString(),
                    TeamOrder = teamcnt


                });

                await _context.AddAsync(team[teamcnt]);

                for (int i = 0; i < item; i++)
                {
                    teamMembers.Add(new TeamMember
                    {
                        BowlerID = selectLst[cnt],
                        Score = 0,
                        TeamID = team[teamcnt].ID,
                        Sequence = i

                    });
                    cnt++;
                    
                }
                
                teamcnt++;
                teamAsc++;

            }

            await _context.AddRangeAsync(team);

            await _context.AddRangeAsync(teamMembers);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index),new { Id = Id } );
            
        }

        // GET: TeamMembers/Create
        public async Task<IActionResult> EditTeam(int? Id)
        {
            //SubGameId
            if (Id == null)
            {
                return NotFound();
            }

            //var teamMembers = await _context.TeamMembers
            //   .Include(t => t.Bowler)
            //       .ThenInclude(b => b.BowlerAverage)
            //   .Include(t => t.Team)
            //   .Where(t => t.Team.SubGameID == Id)
            //   .OrderBy(t => t.Team.TeamOrder)
            //   .ThenBy(t => t.Sequence)
            //   .AsNoTracking().ToListAsync();



            var teamMembers = await _context.TeamMembers
                .Include(t => t.Bowler)
                    .ThenInclude(b => b.BowlerAverage)
                .Include(t => t.Team)
                .Where(t => t.Team.SubGameID == Id)
                .OrderBy(t => t.TeamID)
                .ThenBy(t => t.Sequence)

                .AsNoTracking().ToListAsync();


            var teamBowlers = teamMembers.Select(t => t.Bowler.BowlerID).ToList();
            //var bowlers = _context.Bowlers.ToList();

            var  remainBowlers= new List<Bowler>();
            foreach (var item in _context.Bowlers.Include(t=>t.BowlerAverage))
            {
                if (!teamBowlers.Contains(item.BowlerID))
                {
                    remainBowlers.Add(item);
                }
                
            }


            ViewData["BowlerID"] = remainBowlers;
            //ViewData["BowlerID"] = new SelectList(gg, "BowlerID", "Name" + "  ");
            return View(teamMembers);

        }

        // POST: TeamMember/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTeam(int? Id, int[] teamCnt, string[] selectLst)
        {
            //SubGameId
            if (Id == null)
            {
                return NotFound();
            }

            var gameId = _context.SubGames.Single(g => g.ID == Id).GameID;

            var teamMembers = await _context.TeamMembers
                 .Include(t => t.Bowler)
                 //    .ThenInclude(b => b.BowlerAverage)  
                 .Include(t => t.Team)
                 .Where(t => t.Team.SubGameID == Id)
                .OrderBy(t => t.TeamID)
                .ThenBy(t => t.Sequence)
                 .ToListAsync();

            //팀
            
            var EditTeamMembers = new List<TeamMember>();
            var AddTeamMembers = new List<TeamMember>();
            var EditMember = new TeamMember();

            int cnt = 0;
            int tcnt = 0;


            var teamMemberTeamID = teamMembers.Select(t => t.Team.ID).Distinct().ToList();
            int teamMemberTeamIDCnt = teamMemberTeamID.Count();
            List<Team> team = new List<Team>();
            int teamCntCnt = teamCnt.Count();
            int teamAsc = 65;


            //팀 정리 부분

            if (teamMemberTeamIDCnt < teamCntCnt)
            {
                for (int i = teamMemberTeamIDCnt; i < teamCntCnt; i++)
                {

                    team.Add(new Team
                    {
                        SubGameID = Id ?? 0,
                        TeamName = ((Char)(teamAsc + i)).ToString(),
                        TeamOrder = i

                    });

                }

                 _context.AddRange(team);


            }else if (teamMemberTeamIDCnt > teamCntCnt)
            {
                for (int i = teamCntCnt; i < teamMemberTeamIDCnt; i++)
                {

                    team.Add(_context.Teams.Where(t => t.ID == teamMemberTeamID[i]).Single());
                    //teamMembers.RemoveRange((teamMembers.Where(t => t.TeamID == teamMemberTeamID[i]).ToList());
                    var imsi = (teamMembers.Where(t => t.TeamID == teamMemberTeamID[i]).ToList());
                    foreach (var item in imsi)
                    {
                        teamMembers.Remove(item);
                    }


                    _context.RemoveRange(team);
                }

            }



            foreach (var item in teamCnt)
            {

               
                for (int i = 0; i < item; i++)
                {
                    EditMember = teamMembers.Find(t => t.BowlerID == selectLst[cnt]);

                    if (EditMember == null)
                    {
                        AddTeamMembers.Add(new TeamMember
                        {
                            BowlerID = selectLst[cnt],
                            Score = 0,
                            TeamID = (teamMemberTeamIDCnt > tcnt) ? teamMemberTeamID[tcnt] : team[tcnt - (teamMemberTeamIDCnt)].ID,
                           
                            Sequence = i

                        });

                    }
                    else
                    {

                        if (EditMember.TeamID != ((teamMemberTeamIDCnt > tcnt) ? teamMemberTeamID[tcnt] : team[tcnt - (teamMemberTeamIDCnt)].ID) ||
                            EditMember.Sequence != i)
                        {
                            EditMember.TeamID = (teamMemberTeamIDCnt > tcnt) ? teamMemberTeamID[tcnt] : team[tcnt - (teamMemberTeamIDCnt)].ID;
                            EditMember.Sequence = i;
                            EditTeamMembers.Add(EditMember);

                           
                        }

                        teamMembers.Remove(EditMember);

                    }
                    cnt++;

                }

               
                teamAsc++;
                tcnt++;
            }

            //foreach (var item in teamMembers)
            //{
            //    DelTeamMembers.Add(new Models.Team { Id = item.id, TeamName = string.Empty, PlayOrder = 0 });

            //}

            if (AddTeamMembers.Count()>0)
            {
                _context.AddRange(AddTeamMembers);

            }

            if (EditTeamMembers.Count()>0)
            {
                _context.UpdateRange(EditTeamMembers);
            }

            if (teamMembers.Count()>0)
            {
                _context.RemoveRange(teamMembers);
            }

            



            _context.SaveChanges();

            return RedirectToAction(nameof(Index), new { Id = gameId });



        }

        // GET: TeamMembers/Create
        public async Task<IActionResult> NextTeam(int Id)
        {

            var teamMembers = await _context.TeamMembers
                .Include(t => t.Bowler)
                    .ThenInclude(b => b.BowlerAverage)
                .Include(t => t.Team)
                .Where(t => t.Team.SubGameID == Id)
                .OrderBy(t => t.TeamID)
                .ThenBy(t => t.Sequence)
                .AsNoTracking().ToListAsync();

            var gameId = _context.SubGames.Single(g => g.ID == Id).GameID;

            var round = _context.SubGames.Where(s => s.GameID == gameId).Max(s => s.Round) + 1;

            var subgame = new SubGame
            {
                GameID = gameId,
                Round = round
            };

            await _context.AddAsync(subgame);

            var team = new List<Team>();
            foreach (var item in teamMembers.Select(t=>t.Team).Distinct())
            {
                team.Add(new Team
                {
                    SubGameID = subgame.ID,
                    TeamName = item.TeamName,
                    TeamOrder = item.TeamOrder
                });

            }
            await _context.AddRangeAsync(team);

            var teamMember = new List<TeamMember>();
            int i = -1;
            int tid = 0;
            foreach (var item in teamMembers)
            {
                if (tid != item.TeamID)
                {
                    tid = item.TeamID;
                    i++;
                }
                teamMember.Add(new TeamMember
                {
                    BowlerID=item.BowlerID,
                    Score=0,
                    Sequence=item.Sequence,
                    TeamID=team[i].ID
                });

            }

            await _context.AddRangeAsync(teamMember);

            await _context.SaveChangesAsync();

            //return RedirectToAction(nameof(Edit), new { Id = subgame.ID });

            return RedirectToAction(nameof(Index), new { Id = gameId});

        }

        // GET: TeamMembers/Create
        public async Task<IActionResult> DelTeam(int Id)
        {

            var gameId = _context.SubGames.Single(g => g.ID == Id).GameID;
            var subgame = _context.SubGames.Single(g => g.ID == Id);

           
            _context.Remove(subgame);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { Id = gameId });

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
                    .ThenInclude(s => s.SubGame)
                        .ThenInclude(g=>g.Game)
                .Where(t=>t.Team.SubGameID==id)
                 .OrderBy(t => t.TeamID)
                .ThenBy(t => t.Sequence)
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
        public async Task<IActionResult> Edit(int? Id, int[] inputScores)
        {

            
            if (Id == null) 
            { 
                return NotFound(); 
            }
           
            var teamMembers = await _context.TeamMembers
                .Include(t => t.Bowler)
                    .ThenInclude(b=>b.BowlerAverage)
               .Include(t => t.Team)
                    .ThenInclude(s=>s.SubGame)
               .Where(t => t.Team.SubGameID == Id)
                .OrderBy(t => t.TeamID)
                .ThenBy(t => t.Sequence)
               .ToListAsync();

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
                return RedirectToAction(nameof(Index), new { Id = teamMembers.First().Team.SubGame.GameID });
            }
            catch (DbUpdateException)
            {

                ModelState.AddModelError("", "TeamMember_Edit_Error");
             }

            
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

        
    }
}
