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
    public class TeamMembersController : Controller
    {
        private readonly BowlingContext _context;

        public TeamMembersController(BowlingContext context)
        {
            _context = context;
        }

        // GET: TeamMembers
        public async Task<IActionResult> IndexVertical(int? id)
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
                .OrderBy(t => t.Team.SubGame.Round)
                .ThenBy(t => t.Team.TeamOrder)
                .ThenBy(t => t.Sequence)
                .AsNoTracking();

            ViewData["Game"] = _context.Games.Include(g=>g.SubGames).Where(g => g.ID == id).SingleOrDefault();
            return View(await bowlingContext.ToListAsync());

        }

        // GET: TeamMembers
        public async Task<IActionResult> Index(int? id)
        {
            
            if (id == null)
            {
                return NotFound();

            }

            //가로 방식으로 입력되므로 Sequence를 먼저 소트후 TeamOrder을 소트
            var bowlingContext = _context.TeamMembers
                .Include(t => t.Bowler)
                    .ThenInclude(b => b.BowlerAverage)
                .Include(t => t.Team)
                    .ThenInclude(s => s.SubGame)
                .Where(t => t.Team.SubGame.GameID == id)
                .OrderBy(t => t.Team.SubGame.Round)
                .ThenBy(t => t.Sequence)
                .ThenBy(t => t.Team.TeamOrder)
                .AsNoTracking();
            
            ViewData["Game"] = _context.Games.Include(g => g.SubGames).Where(g => g.ID == id).SingleOrDefault();
            ViewData["Award"] = _context.Awdards.Include(a => a.Bowler)
                                    .Where(a => a.GameID == id)
                                    .OrderBy(a=>a.AwardKind).ToList();

            return View(await bowlingContext.ToListAsync());

        }

        // GET: TeamMembers
        public async Task<IActionResult> IndexGame(int? id, string sortOrder)
        {

            if (id == null)
            {
                return NotFound();

            }

            var game = _context.Games.Where(g => g.ID == id).Include(s => s.SubGames).SingleOrDefault();
            bool notGuest = game.bFine==true ? true : false;  //벌금표시시 게스트는 제외함


            var bowlingContext = new List<TeamMember>();

            if (notGuest)
            {
                bowlingContext = await _context.TeamMembers
               .Include(t => t.Bowler)
                   .ThenInclude(b => b.BowlerAverage)
               .Include(t => t.Team)
                   .ThenInclude(s => s.SubGame)
               .Where(t => t.Team.SubGame.GameID == id && t.Bowler.Group != "zGroup").AsNoTracking().ToListAsync();

                
            }
            else
            {
                bowlingContext = await _context.TeamMembers
                .Include(t => t.Bowler)
                    .ThenInclude(b => b.BowlerAverage)
                .Include(t => t.Team)
                    .ThenInclude(s => s.SubGame)
                .Where(t => t.Team.SubGame.GameID == id).AsNoTracking().ToListAsync();


            }

            IOrderedEnumerable<IGrouping<string, TeamMember>> returnValue = null;

            switch (sortOrder)
            {
                case "preAvg":
                   returnValue = bowlingContext.OrderBy(t => t.Team.SubGame.Round).GroupBy(t => t.BowlerID)
                        .OrderByDescending(g => g.Average(a => a.Score) - (g.FirstOrDefault().Average))
                        .ThenBy(g => g.First().Bowler.BowlerAverage.Handicap) 
                        .ThenBy(g => g.Max(a => a.Score) - g.Min(a => a.Score)); 
                    break;
                default:
                    returnValue = bowlingContext.OrderBy(t => t.Team.SubGame.Round).GroupBy(t => t.BowlerID)
                        .OrderByDescending(g => g.Average(a => a.Score))    //동점처리 1. 총점
                        .ThenBy(g => g.First().Bowler.BowlerAverage.Handicap) //2.무핸디
                        .ThenBy(g => g.Max(a => a.Score) - g.Min(a => a.Score)); //하이로우
                    break;
            }

            //var returnValue = bowlingContext.OrderBy(t => t.Team.SubGame.Round).GroupBy(t => t.BowlerID)
            //.OrderByDescending(g => g.Average(a => a.Score) - (g.FirstOrDefault().Average));
            //.OrderByDescending(g => g.Average(a => a.Score));

            // 위의 두 명령을 합쳐서 표시하니 GroupBy에 에러 발생하여 위와같이 분리함
            //var returnValue = _context.TeamMembers
            //    .Include(t => t.Bowler)
            //        .ThenInclude(b => b.BowlerAverage)
            //    .Include(t => t.Team)
            //        .ThenInclude(s => s.SubGame)
            //    .Where(t => t.Team.SubGame.GameID == id)
            //    .OrderBy(t => t.Team.SubGame.Round)
            //    .AsNoTracking().ToList().GroupBy(t => t.BowlerID)
            //    .OrderBy(g => (g.Sum(a => a.Score)));

            //var bowlingContext = _context.TeamMembers
            //    .Include(t => t.Bowler)
            //        .ThenInclude(b => b.BowlerAverage)
            //    .Include(t => t.Team)
            //        .ThenInclude(s => s.SubGame)
            //    .Where(t => t.Team.SubGame.GameID == id)
            //    .OrderBy(t => t.BowlerID)
            //    .ThenBy(t => t.Team.SubGame.Round)
            //    .AsNoTracking();


            ViewData["Game"] = game;
            ViewData["bowlingContext"] = bowlingContext.OrderByDescending(t => t.Score).Take(15);



            //Award 부분
            var maxGameID = (id > 174) ? _context.Awdards.Where(a=>a.GameID < id).Max(a=>a.GameID):0;
            
            List<Award> AwardList = null;

            switch (sortOrder)
            {
                case "preAvg":
                    AwardList = _context.Awdards.Include(a => a.Bowler).Where(a => a.GameID == id && a.AwardKind > (AwardKind)1).ToList();
                    
                    break;
                default:
                    AwardList = _context.Awdards.Include(a => a.Bowler).Where(a => a.GameID == id && a.AwardKind < (AwardKind)2).ToList();
                    break;
            }

            ViewData["prevAward"] = _context.Awdards.Where(a=> (int)a.AwardKind>1 && a.GameID==maxGameID).Select(a=>a.BowlerID).ToArray();
            ViewData["nowAward"] = AwardList;


            ViewData["slectList"] = await _context.Bowlers
                            .Where(b=>(b.Group == game.Group || b.BowlerID=="00000")&& b.InActivity == false)
                            .OrderBy(b=>b.Name).ToListAsync();
            //return View(await bowlingContext.ToListAsync());
            return View(returnValue);
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
        [Authorize(Policy = "AdminGroup")]
        public async Task<IActionResult> CreateTeam(int Id)
        {

            var game = _context.Games.Include(g => g.SubGames).Where(g => g.ID == Id).SingleOrDefault();
            var group =game.Group;
            

            var bowlers = await _context.Bowlers
                .Include(b => b.BowlerAverage)
                .Where(b=>b.Group==group && b.InActivity == false|| b.Group=="zGroup")
                .OrderBy(b => b.Group)
                .ThenBy(b=>b.Name)
                .AsNoTracking().ToListAsync();


            ViewData["BowlerID"] = bowlers;
            //ViewData["BowlerID"] = new SelectList(bowlers, "BowlerID", "Name");
            ViewData["Game"] = game;

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


            int halfcnt = teamCnt.Count() % 2 == 0 ? teamCnt.Count() / 2 : teamCnt.Count() / 2  + 1;

            foreach (var item in teamCnt)
            {
                int idx = 0;
                   
                if (teamcnt < halfcnt)
                {
                    idx = teamcnt * 2;

                }
                else
                {
                    idx = (teamcnt - halfcnt) * 2 + 1;


                }

                team.Add(new Team
                {
                    SubGameID = subgame.ID,
                    TeamName = ((Char)(65 + idx)).ToString(),
                    TeamOrder = idx
                    
                });

                await _context.AddAsync(team[teamcnt]);
                
                for (int i = 0; i < item; i++)
                {
                    
                    
                    teamMembers.Add(new TeamMember
                    {
                        BowlerID = selectLst[cnt],
                        Score = 0,
                        Average = _context.BowlerAverages.Find(selectLst[cnt]).Average,
                        TeamID = team[teamcnt].ID,
                        Sequence = i

                    });

                    cnt++;
                    
                }
                
                teamcnt++;
                
            }

            await _context.AddRangeAsync(team);

            await _context.AddRangeAsync(teamMembers);

            await _context.SaveChangesAsync();

          
            return RedirectToAction(nameof(Index),new { Id = Id } );
            
        }

        // GET: TeamMembers/Create
        [Authorize(Policy = "AdminGroup")]
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
                .ThenInclude(s => s.SubGame)
                        .ThenInclude(g => g.Game)
                .Where(t => t.Team.SubGameID == Id)
                .OrderBy(t => t.Team.TeamOrder)
                .ThenBy(t => t.Sequence)
                .AsNoTracking().ToListAsync();
            
            var teamBowlers = teamMembers.Select(t => t.Bowler.BowlerID).ToList();
            //var bowlers = _context.Bowlers.ToList();

            var  remainBowlers= new List<Bowler>();


            var group = teamMembers.First().Bowler.Group;

            var bowlers = await _context.Bowlers
                .Include(b => b.BowlerAverage)
                .Where(b => b.Group == group || b.Group == "zGroup")
                 .OrderBy(b => b.Group)
                .ThenBy(b => b.Name)
                .AsNoTracking().ToListAsync();


            foreach (var item in bowlers)
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

            var subGame = _context.SubGames.Single(g => g.ID == Id);  //gameID, round

            var teamMembers = await _context.TeamMembers
                 .Include(t => t.Bowler)
                 //    .ThenInclude(b => b.BowlerAverage)  
                 .Include(t => t.Team)
                 .Where(t => t.Team.SubGameID == Id)
                .OrderBy(t => t.Team.TeamOrder)
                .ThenBy(t => t.Sequence)
                 .ToListAsync();

            //팀수정 
            var AddTeamMembers = new List<TeamMember>();
            var EditTeamMembers = new List<TeamMember>();
            var EditMember = new TeamMember();


            var imsiTMId = teamMembers.Select(t => t.Team.ID).Distinct().ToList();
            int teamMemberTeamIDCnt = imsiTMId.Count();
            List<Team> team = new List<Team>();
            

            int cnt = 0;
            int tcnt = 0;

            int teamCntCnt = teamCnt.Count();


            //팀 추가 시 정리 부분
            if (teamMemberTeamIDCnt < teamCntCnt)
            {
                for (int i = teamMemberTeamIDCnt; i < teamCntCnt; i++)
                {

                    team.Add(new Team
                    {
                        SubGameID = Id ?? 0,
                        TeamName = ((Char)(65 + i)).ToString(),
                        TeamOrder = i

                    });

                }
                
                 _context.AddRange(team);


                //팀 추가에 따른 팀 순서 새로 정리
                foreach (var item in team)
                {
                    imsiTMId.Add(item.ID);
                }
                
            }



            //팀 삭제 시 정리 부분
            if (teamMemberTeamIDCnt > teamCntCnt)
            {
                for (int i = teamCntCnt; i < teamMemberTeamIDCnt; i++)
                {
                    team.Add(_context.Teams.Where(t => t.ID == imsiTMId[i]).Single());
                    //teamMembers.RemoveRange((teamMembers.Where(t => t.TeamID == teamMemberTeamID[i]).ToList());
                    var imsi = (teamMembers.Where(t => t.TeamID == imsiTMId[i]).ToList());
                    foreach (var item in imsi)
                    {
                        teamMembers.Remove(item);
                    }


                    _context.RemoveRange(team);
                }

                //팀 추가에 따른 팀 순서 새로 정리
                foreach (var item in team)
                {
                    imsiTMId.Remove(item.ID);
                }

                
            }


            //팀 추가 삭제에 따른 정리 부분
            teamMemberTeamIDCnt = imsiTMId.Count();
            int[] teamMemberTeamID = new int[teamMemberTeamIDCnt];


            int halfcnt = teamMemberTeamIDCnt % 2 == 0 ? teamMemberTeamIDCnt / 2 : teamMemberTeamIDCnt / 2 + 1;


            for (int i = 0; i < teamMemberTeamIDCnt; i++)
            {
                int idx = 0;

                if (i < halfcnt)
                {
                    idx = i * 2;
                }
                else
                {
                    idx = (i - halfcnt) * 2 + 1;
                }
                teamMemberTeamID[i] = imsiTMId[idx];
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
                            TeamID = teamMemberTeamID[tcnt], //(teamMemberTeamIDCnt > tcnt) ? teamMemberTeamID[tcnt] : team[tcnt - (teamMemberTeamIDCnt)].ID,
                            Average = _context.BowlerAverages.Find(selectLst[cnt]).Average,
                            Sequence = i

                        });

                    }
                    else
                    {

                        if (EditMember.TeamID != teamMemberTeamID[tcnt] || //((teamMemberTeamIDCnt > tcnt) ? teamMemberTeamID[tcnt] : team[tcnt - (teamMemberTeamIDCnt)].ID) ||
                            EditMember.Sequence != i)
                        {
                            EditMember.TeamID = teamMemberTeamID[tcnt]; //(teamMemberTeamIDCnt > tcnt) ? teamMemberTeamID[tcnt] : team[tcnt - (teamMemberTeamIDCnt)].ID;
                            EditMember.Sequence = i;
                            EditTeamMembers.Add(EditMember);

                           
                        }

                        teamMembers.Remove(EditMember);

                    }
                    cnt++;

                }
                
                tcnt++;
                
            }

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


            return RedirectToAction(nameof(Index), new { Id = subGame.GameID, game = subGame.Round });


        }

        // GET: TeamMembers/Create
        [Authorize(Policy = "AdminGroup")]
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
                    Average=item.Average,
                    Sequence=item.Sequence,
                    TeamID=team[i].ID
                });

            }

            await _context.AddRangeAsync(teamMember);

            await _context.SaveChangesAsync();

            //return RedirectToAction(nameof(Edit), new { Id = subgame.ID });

            return RedirectToAction(nameof(Index), new { Id = gameId, game=round});

        }

        // GET: TeamMembers/Create
        [Authorize(Policy = "AdminGroup")]
        public async Task<IActionResult> DelTeam(int Id)
        {

            var gameId = _context.SubGames.Single(g => g.ID == Id).GameID;
            
            var delSubgame = _context.SubGames.Single(g => g.ID == Id);

            var updateSubgames = _context.SubGames.Where(t => t.GameID == delSubgame.GameID && t.Round > delSubgame.Round);
            
            foreach (var us in updateSubgames)
            {
                us.Round--;
            }

            //_context.Update(updateSubgames);  //필요업음  
            _context.Remove(delSubgame);
            

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { Id = gameId, game = delSubgame.Round >1? --delSubgame.Round : 1});     

        }


        // GET: TeamMembers/Edit/5
        [Authorize(Policy = "AdminGroup")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["subGame"] = _context.SubGames.Include(s=>s.Game).Where(s => s.ID == id).SingleOrDefault();
            
            if (id == null)
            {
                return NotFound();
            }

            var teamMembers = await _context.TeamMembers
                .Include(t => t.Bowler)
                    .ThenInclude(b=>b.BowlerAverage)
                .Include(t => t.Team)
                    //.ThenInclude(s => s.SubGame)
                    //    .ThenInclude(g=>g.Game)
                .Where(t=>t.Team.SubGameID==id)
                 .OrderBy(t => t.Team.TeamOrder)
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

            var subGame = _context.SubGames.Include(s => s.Game).Where(s => s.ID == Id).SingleOrDefault();
            bool inHandi = subGame.Game.bHandicap;


            if (Id == null) 
            { 
                return NotFound(); 
            }
           
            var teamMembers = await _context.TeamMembers
                .Include(t => t.Bowler)
                //   .ThenInclude(b=>b.BowlerAverage)  핸디를 클라이언트 자바스트립트로 처리
               .Include(t => t.Team)
               .Where(t => t.Team.SubGameID == Id)
                .OrderBy(t => t.Team.TeamOrder)
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
                    if (item.Score != inputScores[i])
                    {
                        item.Score = inputScores[i];   //핸디 포함 점수 (클라이언트 자바스트립트에서 핸디계산을 하여 전송 함)
                        _context.Update(item);

                    }
                   
                    i++;

                }

                await _context.SaveChangesAsync();

                
                return RedirectToAction(nameof(Index), new { Id = subGame.GameID, game= subGame.Round });
               
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JumsuAward (int Id, string[] JumsuPar)
        {
            var awardObj = _context.Awdards.Where(a=> a.GameID == Id && a.AwardKind < (AwardKind)2).ToList();

            if (awardObj.Count == 0)
            {
                List<Award> listAward = new List<Award>();
                for (int i = 0; i < JumsuPar.Length; i++)
                {
                    listAward.Add(new Award()
                    {
                        AwardKind = (AwardKind)i,
                        GameID = Id,
                        BowlerID = JumsuPar[i]
                    });

                }
               
                await _context.AddRangeAsync(listAward);
                

            }
            else
            {

                for (int i = 0; i < JumsuPar.Length; i++)
                {
                    awardObj[i].BowlerID = JumsuPar[i];
                    
                }

                _context.UpdateRange(awardObj);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexGame), new { id = Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AvgAward(int Id, string[] JumsuPar)
        {
            var awardObj = _context.Awdards.Where(a => a.GameID == Id && a.AwardKind > (AwardKind)1).ToList();

            if (awardObj.Count == 0)
            {
                List<Award> listAward = new List<Award>();
                for (int i = 0; i < JumsuPar.Length; i++)
                {
                    listAward.Add(new Award()
                    {
                        AwardKind = (AwardKind)(i+2), //AwardKind 2~6
                        GameID = Id,
                        BowlerID = JumsuPar[i]
                    });

                }

                await _context.AddRangeAsync(listAward);


            }
            else
            {

                for (int i = 0; i < JumsuPar.Length; i++)
                {
                    awardObj[i].BowlerID = JumsuPar[i];

                }

                _context.UpdateRange(awardObj);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexGame), new { id = Id, sortOrder = "preAvg" });
        }



    }
}
