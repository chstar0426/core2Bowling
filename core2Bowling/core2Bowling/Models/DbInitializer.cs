using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core2Bowling.Models
{
    public static class DbInitializer
    {
        public static void Initialize(BowlingContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Bowlers.Any())
            {
                return;   // DB has been seeded
            }
        
            var bowlers = new Bowler[]
            {

               new Bowler{ BowlerID="fg001", Name="최국헌",  Group="Family", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="fg002", Name="소정란",  Group="Family", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="fg003", Name="최창헌",  Group="Family", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="fg004", Name="최승필",  Group="Family", RegisterDate=DateTime.Parse("2018-01-01")},

               new Bowler{ BowlerID="rp001", Name="김정일", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp002", Name="김진구", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp003", Name="권호석", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp004", Name="배언욱", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp005", Name="박미정", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp006", Name="허진범", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp007", Name="김은희", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp008", Name="강종현", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp009", Name="곽신규", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp010", Name="신창용", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp011", Name="김진영", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp012", Name="권미경", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp013", Name="김윤섭", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp014", Name="신인자", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp015", Name="서보윤", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp016", Name="노애경", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp017", Name="오정수", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp018", Name="최국헌", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp019", Name="이나경", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp020", Name="이상헌", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp021", Name="윤정남", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="rp022", Name="최우정", Group="RedPin", RegisterDate=DateTime.Parse("2018-01-01")},

               new Bowler{ BowlerID="gu001", Name="게스트1", Group="zGroup", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="gu002", Name="게스트2", Group="zGroup", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="gu003", Name="게스트3", Group="zGroup", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="gu004", Name="게스트4", Group="zGroup", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="gu005", Name="게스트5", Group="zGroup", RegisterDate=DateTime.Parse("2018-01-01")},

               new Bowler{ BowlerID="hd001", Name="핸디1", Group="zGroup", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="hd002", Name="핸디2", Group="zGroup", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="hd003", Name="핸디3", Group="zGroup", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="hd004", Name="핸디4", Group="zGroup", RegisterDate=DateTime.Parse("2018-01-01")},
               new Bowler{ BowlerID="hd005", Name="핸디5", Group="zGroup", RegisterDate=DateTime.Parse("2018-01-01")}


            };
            
            foreach (Bowler b in bowlers)
            {
                context.Bowlers.Add(b);
            }
            context.SaveChanges();

            var bowlerAverage = new BowlerAverage[]
            {

                new BowlerAverage{BowlerID="fg001", Average=165, Handicap=0},
                new BowlerAverage{BowlerID="fg002", Average=165, Handicap=15},
                new BowlerAverage{BowlerID="fg003", Average=165, Handicap=0},
                new BowlerAverage{BowlerID="fg004", Average=130, Handicap=30},

                new BowlerAverage{BowlerID="rp001", Average=202, Handicap=0},
                new BowlerAverage{BowlerID="rp002", Average=194, Handicap=0},
                new BowlerAverage{BowlerID="rp003", Average=187, Handicap=0},
                new BowlerAverage{BowlerID="rp004", Average=186, Handicap=0},
                new BowlerAverage{BowlerID="rp005", Average=180, Handicap=15},
                new BowlerAverage{BowlerID="rp006", Average=180, Handicap=0},
                new BowlerAverage{BowlerID="rp007", Average=180, Handicap=15},
                new BowlerAverage{BowlerID="rp008", Average=176, Handicap=0},
                new BowlerAverage{BowlerID="rp009", Average=175, Handicap=0},
                new BowlerAverage{BowlerID="rp010", Average=174, Handicap=0},
                new BowlerAverage{BowlerID="rp011", Average=173, Handicap=15},
                new BowlerAverage{BowlerID="rp012", Average=172, Handicap=15},
                new BowlerAverage{BowlerID="rp013", Average=170, Handicap=0},
                new BowlerAverage{BowlerID="rp014", Average=166, Handicap=15},
                new BowlerAverage{BowlerID="rp015", Average=161, Handicap=0},
                new BowlerAverage{BowlerID="rp016", Average=161, Handicap=15},
                new BowlerAverage{BowlerID="rp017", Average=158, Handicap=0},
                new BowlerAverage{BowlerID="rp018", Average=156, Handicap=0},
                new BowlerAverage{BowlerID="rp019", Average=155, Handicap=15},
                new BowlerAverage{BowlerID="rp020", Average=131, Handicap=0},
                new BowlerAverage{BowlerID="rp021", Average=180, Handicap=0},
                new BowlerAverage{BowlerID="rp022", Average=180, Handicap=0},


                new BowlerAverage{BowlerID="gu001", Average=0, Handicap=0},
                new BowlerAverage{BowlerID="gu002", Average=0, Handicap=0},
                new BowlerAverage{BowlerID="gu003", Average=0, Handicap=0},
                new BowlerAverage{BowlerID="gu004", Average=0, Handicap=0},
                new BowlerAverage{BowlerID="gu005", Average=0, Handicap=0},

                new BowlerAverage{BowlerID="hd001", Average=0, Handicap=0},
                new BowlerAverage{BowlerID="hd002", Average=0, Handicap=0},
                new BowlerAverage{BowlerID="hd003", Average=0, Handicap=0},
                new BowlerAverage{BowlerID="hd004", Average=0, Handicap=0},
                new BowlerAverage{BowlerID="hd005", Average=0, Handicap=0}

            };

            foreach (var a in bowlerAverage)
            {
                context.BowlerAverages.Add(a);
            }
            context.SaveChanges();


            var yearAverages = new YearAverage[]
            {

               new YearAverage{ BowlerID="fg001", Year="2017", Bigo="", Average=165},
               new YearAverage{ BowlerID="fg002", Year="2017", Bigo="", Average=165},
               new YearAverage{ BowlerID="fg003", Year="2017", Bigo="", Average=165},
               new YearAverage{ BowlerID="fg004", Year="2017", Bigo="", Average=120},

               new YearAverage{ BowlerID="rp001", Year="2017", Bigo="", Average=189},
               new YearAverage{ BowlerID="rp002", Year="2017", Bigo="", Average=196},
               new YearAverage{ BowlerID="rp003", Year="2017", Bigo="", Average=189},
               new YearAverage{ BowlerID="rp004", Year="2017", Bigo="", Average=180},
               new YearAverage{ BowlerID="rp005", Year="2017", Bigo="", Average=171},
               new YearAverage{ BowlerID="rp006", Year="2017", Bigo="", Average=181},
               new YearAverage{ BowlerID="rp007", Year="2017", Bigo="", Average=186},
               new YearAverage{ BowlerID="rp008", Year="2017", Bigo="", Average=191},
               new YearAverage{ BowlerID="rp009", Year="2017", Bigo="", Average=184},
               new YearAverage{ BowlerID="rp010", Year="2017", Bigo="", Average=191},
               new YearAverage{ BowlerID="rp011", Year="2017", Bigo="", Average=170},
               new YearAverage{ BowlerID="rp012", Year="2017", Bigo="", Average=172},
               new YearAverage{ BowlerID="rp013", Year="2017", Bigo="", Average=168},
               new YearAverage{ BowlerID="rp014", Year="2017", Bigo="", Average=167},
               new YearAverage{ BowlerID="rp015", Year="2017", Bigo="", Average=170},
               new YearAverage{ BowlerID="rp016", Year="2017", Bigo="", Average=167},
               new YearAverage{ BowlerID="rp017", Year="2017", Bigo="", Average=168},
               new YearAverage{ BowlerID="rp018", Year="2017", Bigo="", Average=154},
               new YearAverage{ BowlerID="rp019", Year="2017", Bigo="", Average=150},
               new YearAverage{ BowlerID="rp020", Year="2017", Bigo="", Average=148},
               new YearAverage{ BowlerID="rp021", Year="2017", Bigo="", Average=180},
               new YearAverage{ BowlerID="rp022", Year="2017", Bigo="", Average=180},

            };

            foreach (YearAverage y in yearAverages)
            {
                context.YearAversges.Add(y);
            }
            context.SaveChanges();



            var games = new Game[]
            {
                new Game{ Playtime=DateTime.Parse("2018-06-13 20:00"), GameKind=GameKind.정기전, Penalty=100, Group="RedPin", Place="현대볼링장", GameContent="6-1",
                            bFine=true, bHandicap=true}
            };

            foreach (var item in games)
            {
                context.Games.Add(item);
            }
            context.SaveChanges();


            var subGames = new SubGame[]
            {
                new SubGame{ Round=1, GameID = games.Single( g => g.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},
                new SubGame{ Round=2, GameID = games.Single( g => g.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},
                new SubGame{ Round=3, GameID = games.Single( g => g.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},



            };

            foreach (var item in subGames)
            {
                context.SubGames.Add(item);
            }
            context.SaveChanges();

            var teams = new Team[]
           {

                new Team{TeamName="A", TeamOrder=0, SubGameID = subGames.Single( s => s.Round==1 && s.Game.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},
                new Team{TeamName="B", TeamOrder=1, SubGameID = subGames.Single( s => s.Round==1 && s.Game.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},
                new Team{TeamName="C", TeamOrder=2, SubGameID = subGames.Single( s => s.Round==1 && s.Game.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},
                new Team{TeamName="D", TeamOrder=3, SubGameID = subGames.Single( s => s.Round==1 && s.Game.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},

                new Team{TeamName="A", TeamOrder=0, SubGameID = subGames.Single( s => s.Round==2 && s.Game.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},
                new Team{TeamName="B", TeamOrder=1, SubGameID = subGames.Single( s => s.Round==2 && s.Game.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},
                new Team{TeamName="C", TeamOrder=2, SubGameID = subGames.Single( s => s.Round==2 && s.Game.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},
                new Team{TeamName="D", TeamOrder=3, SubGameID = subGames.Single( s => s.Round==2 && s.Game.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},

                new Team{TeamName="A", TeamOrder=0, SubGameID = subGames.Single( s => s.Round==3 && s.Game.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},
                new Team{TeamName="B", TeamOrder=1, SubGameID = subGames.Single( s => s.Round==3 && s.Game.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},
                new Team{TeamName="C", TeamOrder=2, SubGameID = subGames.Single( s => s.Round==3 && s.Game.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},
                new Team{TeamName="D", TeamOrder=3, SubGameID = subGames.Single( s => s.Round==3 && s.Game.Playtime == DateTime.Parse("2018-06-13 20:00")).ID},

           };

            foreach (Team t in teams)
            {
                context.Teams.Add(t);
            }
            context.SaveChanges();


            var teamMembers = new TeamMember[]
           {
                 new TeamMember{Average=175, BowlerID="rp009", Score=151,  Sequence=0, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=161, BowlerID="rp016", Score=142,  Sequence=1, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=161, BowlerID="rp015", Score=151,  Sequence=2, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=0,   BowlerID="gu001", Score=233,  Sequence=3, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=194, BowlerID="rp002", Score=196,  Sequence=4, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=174, BowlerID="rp010", Score=191,  Sequence=0, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=180, BowlerID="rp005", Score=186,  Sequence=1, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=158, BowlerID="rp017", Score=180,  Sequence=2, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=187, BowlerID="rp003", Score=202,  Sequence=3, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=180, BowlerID="rp007", Score=159,  Sequence=4, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=176, BowlerID="rp008", Score=187,  Sequence=0, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=155, BowlerID="rp019", Score=125,  Sequence=1, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=156, BowlerID="rp018", Score=166,  Sequence=2, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=180, BowlerID="rp006", Score=184,  Sequence=3, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=172, BowlerID="rp012", Score=192,  Sequence=4, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=186, BowlerID="rp004", Score=143,  Sequence=0, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=131, BowlerID="rp020", Score=170,  Sequence=1, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=0,   BowlerID="gu002", Score=190,  Sequence=2, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=173, BowlerID="rp011", Score=167,  Sequence=3, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=202, BowlerID="rp001", Score=148,  Sequence=4, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 1 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=175, BowlerID="rp009", Score=140,  Sequence=0, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=161, BowlerID="rp016", Score=177,  Sequence=1, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=161, BowlerID="rp015", Score=171,  Sequence=2, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=0,   BowlerID="gu001", Score=146,  Sequence=3, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=194, BowlerID="rp002", Score=166,  Sequence=4, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=174, BowlerID="rp010", Score=228,  Sequence=0, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=180, BowlerID="rp005", Score=207,  Sequence=1, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=158, BowlerID="rp017", Score=177,  Sequence=2, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=187, BowlerID="rp003", Score=167,  Sequence=3, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=180, BowlerID="rp007", Score=176,  Sequence=4, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=176, BowlerID="rp008", Score=179,  Sequence=0, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=155, BowlerID="rp019", Score=152,  Sequence=1, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=156, BowlerID="rp018", Score=135,  Sequence=2, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=180, BowlerID="rp006", Score=186,  Sequence=3, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=172, BowlerID="rp012", Score=144,  Sequence=4, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=186, BowlerID="rp004", Score=145,  Sequence=0, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=131, BowlerID="rp020", Score=113,  Sequence=1, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=0,   BowlerID="gu002", Score=176,  Sequence=2, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=173, BowlerID="rp011", Score=155,  Sequence=3, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=202, BowlerID="rp001", Score=227,  Sequence=4, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 2 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=175, BowlerID="rp009", Score=186,  Sequence=0, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=161, BowlerID="rp016", Score=154,  Sequence=1, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=161, BowlerID="rp015", Score=214,  Sequence=2, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=0,   BowlerID="gu001", Score=151,  Sequence=3, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=194, BowlerID="rp002", Score=173,  Sequence=4, TeamID=teams.Single(t=>t.TeamOrder==0 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=174, BowlerID="rp010", Score=166,  Sequence=0, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=180, BowlerID="rp005", Score=174,  Sequence=1, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=158, BowlerID="rp017", Score=157,  Sequence=2, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=187, BowlerID="rp003", Score=162,  Sequence=3, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=180, BowlerID="rp007", Score=171,  Sequence=4, TeamID=teams.Single(t=>t.TeamOrder==1 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=176, BowlerID="rp008", Score=179,  Sequence=0, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=155, BowlerID="rp019", Score=153,  Sequence=1, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=156, BowlerID="rp018", Score=126,  Sequence=2, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=180, BowlerID="rp006", Score=153,  Sequence=3, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=172, BowlerID="rp012", Score=169,  Sequence=4, TeamID=teams.Single(t=>t.TeamOrder==2 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=186, BowlerID="rp004", Score=206,  Sequence=0, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=131, BowlerID="rp020", Score=130,  Sequence=1, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=0,   BowlerID="gu002", Score=166,  Sequence=2, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=173, BowlerID="rp011", Score=194,  Sequence=3, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID },
                 new TeamMember{Average=202, BowlerID="rp001", Score=224,  Sequence=4, TeamID=teams.Single(t=>t.TeamOrder==3 && t.SubGame.Round == 3 && t.SubGame.Game.Playtime==DateTime.Parse("2018-06-13 20:00")).ID }


           };


            foreach (TeamMember t in teamMembers)
            {
                context.TeamMembers.Add(t);
            }
            context.SaveChanges();


        }



    }
}

