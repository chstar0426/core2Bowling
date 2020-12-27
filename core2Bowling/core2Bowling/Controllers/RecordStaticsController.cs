using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core2Bowling.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace core2Bowling.Controllers
{
    [Authorize]
    public class RecordStaticsController : Controller
    {
        private readonly BowlingContext _context;

        public RecordStaticsController(BowlingContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> CalPenaltyAvg(string TeamGroup, string startDate, string endDate)
        {
            //로그인 한 사람의 권한 설정 부분
            var group = User.FindFirst("UserGroup").Value;

            if (string.IsNullOrEmpty(TeamGroup))
            {
                if (group == "All")
                {
                    TeamGroup = "RedPin";
                }
                else
                {
                    TeamGroup = group;
                }

            }

            if (group != "All" && group != TeamGroup)
            {
                return NotFound();
            }


            /////////////////// 검색 설정 부분
            ViewData["TeamGroup"] = TeamGroup;

            if (string.IsNullOrEmpty(startDate))
            {
                ViewData["StartDate"] = DateTime.Now.AddMonths(-2).ToString("yyyy-MM");
                startDate = DateTime.Now.AddMonths(-2).ToString("yyyy-MM") + "-01 00:00";
            } else
            {
                ViewData["StartDate"] = startDate;
                startDate = startDate + "-01 00:00";
            }

            if (string.IsNullOrEmpty(endDate))
            {
                ViewData["EndDate"] = DateTime.Now.ToString("yyyy-MM");

                string[] yymm = DateTime.Now.ToString("yyyy-MM").Split('-');
                string lastday = DateTime.DaysInMonth(int.Parse(yymm[0]), int.Parse(yymm[1])).ToString();

                endDate = DateTime.Now.ToString("yyyy-MM") + "-" + lastday + " 23:59";

            }
            else
            {
                ViewData["EndDate"] = endDate;
                string[] yymm = endDate.Split('-');
                string lastday = DateTime.DaysInMonth(int.Parse(yymm[0]), int.Parse(yymm[1])).ToString();

                endDate = endDate + "-" + lastday + " 23:59";
            }

            //게임리스트
            var gameList = _context.Games.Where(g => g.GameKind == 0 && g.Group == TeamGroup
                 && (DateTime.Parse(startDate) <= g.Playtime && DateTime.Parse(endDate) >= g.Playtime))
                 .OrderBy(g => g.Playtime);

            ViewData["GameContents"] = gameList.Select(g => new GameTitle {
                GameID = g.ID,
                GameContent = g.GameContent
            }).ToList();   //게임 7-1, 8-1 등  표시

            List<int> gameIds = gameList.Select(g => g.ID).ToList();  // 게임 Id만 가저욤



            int gameCnt = gameIds.Count;  // 게임수


            //게임 ID에 대해 볼러별로 그루화 하여 에버리지를 구함
            var games = new List<YearMonthAvg>();

            foreach (var item in gameIds)
            {

                var data = _context.TeamMembers
                    .Include(t => t.Bowler)
                        .ThenInclude(t => t.BowlerAverage)
                    .Include(t => t.Team)
                        .ThenInclude(s => s.SubGame)
                        .ThenInclude(s => s.Game)
                    .Where(t => t.Team.SubGame.GameID == item && t.Bowler.Group != "zGroup")  //&& (t.Bowler.LeaveDate >= DateTime.Parse(startDate) || t.Bowler.InActivity == false)
                    .GroupBy(t => t.BowlerID)
                     .Select(gt => new YearMonthAvg
                     {
                         BowlerID = gt.Key,
                         monAvg = Convert.ToInt32(gt.Average(g => g.Score)),
                         Name = gt.First().Bowler.Name,
                         Handicap = gt.First().Bowler.BowlerAverage.Handicap,
                         Period = gt.First().Team.SubGame.Game.GameContent,
                         GameID = item,
                         InActivity = gt.First().Bowler.InActivity // 여기에서는 의미가 없고 연간통계에 사용하기 위한 컬럼

                     }).AsNoTracking();

                games.AddRange(await data.ToListAsync());


            }


            //이전해 성적
            var YearAvgs = _context.YearAversges.Include(y => y.Bowler)
                .Where(y => y.Year == (int.Parse(endDate.ToString().Substring(0, 4)) - 1).ToString()
                && y.Bowler.Group == TeamGroup).OrderBy(y => y.BowlerID).ToList();


            //에버리지 순위 대로 소트하기위해 Group화 (다시 볼러ID로 그룹화)
            var returnGames = games.GroupBy(t => t.BowlerID)
                .Select(g => new GroupGames
                {
                    BowlerID = g.Key,
                    Name = g.First().Name,
                    Handicap = g.First().Handicap,
                    InActivity = g.First().InActivity,
                    beforeAvg = YearAvgs.Find(y => y.BowlerID == g.Key) == null ? 0 : YearAvgs.Find(y => y.BowlerID == g.Key).Average,
                    Games = g,
                    Total = (g.Sum(a => a.monAvg) + (gameIds.Count() > g.Count() ? (YearAvgs.Find(y => y.BowlerID == g.Key) == null ? 0 : YearAvgs.Find(y => y.BowlerID == g.Key).Average) * (gameIds.Count() - g.Count()) : 0))
                        / (YearAvgs.Find(y => y.BowlerID == g.Key) == null ? Convert.ToSingle(g.Count()) : Convert.ToSingle(gameIds.Count())), //괄호( ) 위치 주의
                    InAttend = gameIds.Count() - g.Count(),
                    HiNLow = g.Max(a => a.monAvg) - g.Min(a => a.monAvg)

                }).ToList();


            //3달동안 경기에 한번도 참여하지 못한 멤버 추가
             var bowlers = _context.Bowlers.Include(b=>b.BowlerAverage).Where(t => t.Group == TeamGroup &&
                ((t.RegisterDate <= DateTime.Parse(endDate) && t.LeaveDate >= DateTime.Parse(startDate)) || t.RegisterDate <= DateTime.Parse(endDate) && t.LeaveDate == null)
                ).ToList();

            var exceptBowlers = bowlers.Select(b => b.BowlerID).Except(returnGames.Select(r => r.BowlerID));


            foreach (var item in exceptBowlers)
            {
                var bw = bowlers.Find(b => b.BowlerID == item);
                var ye = YearAvgs.Find(y => y.BowlerID == item) == null ? 0 : YearAvgs.Find(y => y.BowlerID == item).Average;
                returnGames.Add(new GroupGames
                {
                    BowlerID = bw.BowlerID,
                    Name = bw.Name,
                    Handicap = bw.BowlerAverage.Handicap,
                    beforeAvg = ye,
                    Games = null,
                    InActivity = bw.InActivity,
                    Total = ye,
                    InAttend = gameIds.Count(),
                    HiNLow = 10000   //불참 벌점을 많이 줌
                });

            }


            //소트 하여 반환 (탈퇴자를 아래로 몰아서 관리)
            return View(returnGames.OrderBy(g => g.InActivity)  // 탈퇴자 제일 하위
                .ThenByDescending(g => g.Total)  // 평균점수로 소트
                .ThenBy(g=>g.InAttend)    // 출석자 우선
                .ThenBy(g => g.Handicap)  // 무핸디 우선
                .ThenBy(g => g.HiNLow)); //하이로우

            //return View(returnGames.OrderByDescending(g => g.Total));

        }

        [Authorize(Policy = "AdminGroup")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalPenaltyAvg(string hTeamGroup, string[] inputName, int[] inputAvg, string hendDate)
        {

            string[] yymm = hendDate.Split('-');
            hendDate = hendDate + "-" + DateTime.DaysInMonth(int.Parse(yymm[0]), int.Parse(yymm[1])).ToString() + " 23:59"; //마지막날 추가

            string startDate = DateTime.Parse(hendDate).AddMonths(-2).ToString("yyyy-MM") + "-01 00:00";


            int cnt = 0;
            var bowlerAverages = new List<BowlerAverage>();


            //들어온 자료를 list에 채워 넣음
            foreach (var item in inputName)
            {
                BowlerAverage bAvg = new BowlerAverage
                {
                    Average = inputAvg[cnt],
                    BowlerID = item

                };
                bowlerAverages.Add(bAvg);

                cnt++;
            }

            var penaltyAvg = _context.BowlerAverages.Include(b => b.Bowler)
                .Where(b => (b.Bowler.Group == hTeamGroup) &&
                    (b.Bowler.InActivity == false && b.Bowler.RegisterDate <= DateTime.Parse(hendDate) ||
                    (b.Bowler.InActivity == true && b.Bowler.LeaveDate >= DateTime.Parse(startDate))))
                .OrderBy(b => b.BowlerID);


            // 업데이트 하기
            //중요   listYearAverages와 tlist의 개수가 같아야 함 ...
            var tlist = bowlerAverages.OrderBy(b => b.BowlerID).ToList();

            cnt = 0;


            foreach (var item in penaltyAvg)
            {
                if (item.Average != tlist[cnt].Average || item.Bigo != yymm[0] + "-" + yymm[1])
                {
                    item.Average = tlist[cnt].Average;
                    item.Bigo = yymm[0] + "-" + yymm[1];

                }
                cnt++;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index", "Bowlers");
        }


        [HttpGet]
        public async Task<IActionResult> CalYearAvg(string TeamGroup, string thisYear)
        {


            //권한 설정 부분
            var group = User.FindFirst("UserGroup").Value;

            if (string.IsNullOrEmpty(TeamGroup))
            {
                if (group == "All")
                {
                    TeamGroup = "RedPin";
                }
                else
                {
                    TeamGroup = group;
                }

            }

            if (group != "All" && group != TeamGroup)
            {
                return NotFound();
            }


            /////////////////////검색 설정 부분
            string startDate = string.Empty;
            string endDate = string.Empty;

            ViewData["TeamGroup"] = TeamGroup;


            if (string.IsNullOrEmpty(thisYear))
            {
                ViewData["ThisYear"] = DateTime.Now.ToString("yyyy");

                startDate = DateTime.Now.Year.ToString() + "-01-01 00:00";
                endDate = DateTime.Now.Year.ToString() + "-12-31 23:59";

            }
            else
            {
                ViewData["ThisYear"] = thisYear;
                startDate = thisYear + "-01-01 00:00";
                endDate = thisYear + "-12-31 23:59";
            }

            /////////게임리스트
            var gameList = _context.Games.Where(g => g.GameKind == 0 && g.Group == TeamGroup
                 && (DateTime.Parse(startDate) <= g.Playtime && DateTime.Parse(endDate) >= g.Playtime))
                 .OrderBy(g => g.Playtime);


            ViewData["GameContents"] = gameList.Select(g => new GameTitle
            {
                GameID = g.ID,
                GameContent = g.GameContent
            }).ToList();   //게임 7-1, 8-1 등  표시

            List<int> gameIds = gameList.Select(g => g.ID).ToList();  //게임 Id만 가져옴


            var games = new List<YearMonthAvg>();


            //게임 ID에 대해 볼러별로 그루화 하여 에버리지를 구함
            foreach (var item in gameIds)
            {

                var data = _context.TeamMembers
                    .Include(t => t.Bowler)
                        .ThenInclude(t => t.BowlerAverage)
                    .Include(t => t.Team)
                        .ThenInclude(s => s.SubGame)
                        .ThenInclude(s => s.Game)
                    .Where(t => t.Team.SubGame.GameID == item && t.Bowler.Group == TeamGroup)  //&& (t.Bowler.LeaveDate >= DateTime.Parse("2018-01-01") || t.Bowler.InActivity==false))
                    .GroupBy(t => t.BowlerID)
                     .Select(gt => new YearMonthAvg
                     {
                         BowlerID = gt.Key,
                         monAvg = Convert.ToInt32(gt.Average(g => g.Score)),
                         Name = gt.First().Bowler.Name,
                         Handicap = gt.First().Bowler.BowlerAverage.Handicap,
                         Period = gt.First().Team.SubGame.Game.GameContent,
                         GameID = item,
                         InActivity=gt.First().Bowler.InActivity

                     }).AsNoTracking();

                games.AddRange(await data.ToListAsync());


            }

            //1년동안 경기에 한번도 참여하지 못한 멤버 추가 
            //var bowlers = _context.Bowlers.Include(b=>b.BowlerAverage).Where(t => (t.Group == TeamGroup && t.InActivity == false) 
            //        || (t.Group == TeamGroup && t.LeaveDate >= DateTime.Parse(startDate))).ToList();
            var bowlers = _context.Bowlers.Include(b => b.BowlerAverage).Where(t => (t.Group == TeamGroup
                      && t.RegisterDate < DateTime.Parse(endDate) && (t.LeaveDate >= DateTime.Parse(startDate) || string.IsNullOrEmpty(t.LeaveDate.ToString())))).ToList();

            var exceptBowlers = bowlers.Select(b => b.BowlerID).Except(games.Select(r => r.BowlerID).Distinct());

            foreach (var item in exceptBowlers)
            {
                var bw = bowlers.Find(b => b.BowlerID == item);
                games.Add(new YearMonthAvg
                {
                    BowlerID = bw.BowlerID,
                    Name = bw.Name,
                    Handicap = bw.BowlerAverage.Handicap,
                    //beforeAvg = ,
                    //Games = ,
                    InActivity = bw.InActivity
                    //Total = 
                });

            }

            
            //그룹화 새로 조정
            //에버리지 순위 대로 소티(탈퇴자를 아래로 몰아서 관리)
            var groupGames = games.GroupBy(t => t.BowlerID)
                .OrderBy(g=>g.First().InActivity)
                .ThenByDescending(g => g.Average(t => t.monAvg))
                .ThenByDescending(g=>g.Count())
                .ThenBy(g=>g.First().Handicap)
                .ThenBy(g=>g.Max(t=>t.monAvg) - g.Min(t=>t.monAvg));

            //var groupGames = games.GroupBy(t => t.BowlerID).OrderByDescending(g => g.Average(t => t.monAvg));


            return View(groupGames.ToList());

        }


        [Authorize(Policy = "AdminGroup")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalYearAvg(string hTeamGroup, string[] inputName, int[] inputAvg, string hthisYear)
        {
            hthisYear = string.IsNullOrEmpty(hthisYear) ? DateTime.Now.ToString("yyyy") : hthisYear.Substring(0,4);

            int cnt = 0;
            var listYearAverages = new List<YearAverage>();
            

            //Form으로 전달된 자료를 list에 채워 넣음 
            //탈퇴 표시를 위해 탈퇴자는 음수(-)사용, 탈퇴 0점은 확인이 어려워 1점으로 표시
            foreach (var item in inputName)
            {
                var avg = inputAvg[cnt];

                YearAverage yAvg = new YearAverage
                {
                    
                    Average = Math.Abs(avg)==1 ? 0 : Math.Abs(avg),
                    BowlerID = item,
                    Year = hthisYear,
                    Bigo = avg > 0 ? "" : "탈퇴"

                };
                listYearAverages.Add(yAvg);
               
                cnt++;
            }

            //기존 자료 불러오옴 (기존자료 가 있으면 업데이트, 없으면 insert
            var yearBowlers = _context.YearAversges.Include(y => y.Bowler)
               .Where(y => y.Bowler.Group == hTeamGroup && y.Year == hthisYear)
               .OrderBy(y => y.BowlerID).ToList();

            
            if (yearBowlers.Count() > 0)
            {
                //정렬 필요가 없을 것 같음
                //listYearAverages = listYearAverages.OrderBy(y => y.BowlerID).ToList();
                int i = 0;

                foreach (var item in listYearAverages)
                {
                    //YearAverages에 자료가 있는지 찾음
                    var yb = yearBowlers.FirstOrDefault(y => y.BowlerID == item.BowlerID);
                    
                    if (yb == null)
                    {
                        //없으면, insert, 새로추가
                        _context.YearAversges.Add(new YearAverage
                        {
                            Average = item.Average,
                            BowlerID = item.BowlerID,
                            Bigo = item.Bigo,
                            Year= item.Year
                        });
                    }
                    else
                    {
                        //있으면, update
                        if (item.Average != yb.Average || item.Bigo != yb.Bigo)
                        {
                            yb.Average = item.Average;
                            yb.Bigo = item.Bigo;
                        }
                    }
                     

                    i++;
                }
                
            }
            else
            {
                //한번도 입력된적이 없으면 일괄 입력 하기
                _context.YearAversges.AddRange(listYearAverages);

            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index", "YearAverages");
        }
    }
}