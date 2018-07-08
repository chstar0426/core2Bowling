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
               new Bowler{ BowlerID="rp001", Name="김정일", Group="RedPin" },
               new Bowler{ BowlerID="rp002", Name="김진구", Group="RedPin" },
               new Bowler{ BowlerID="rp003", Name="권호석", Group="RedPin" },
               new Bowler{ BowlerID="rp004", Name="배언욱", Group="RedPin"},
               new Bowler{ BowlerID="rp005", Name="박미정", Group="RedPin",},
               new Bowler{ BowlerID="rp006", Name="허진범", Group="RedPin",},
               new Bowler{ BowlerID="rp007", Name="김은희", Group="RedPin",},
               new Bowler{ BowlerID="rp008", Name="강종현", Group="RedPin",},
               new Bowler{ BowlerID="rp009", Name="곽신규", Group="RedPin",},
               new Bowler{ BowlerID="rp010", Name="신창용", Group="RedPin",},
               new Bowler{ BowlerID="rp011", Name="김진영", Group="RedPin",},
               new Bowler{ BowlerID="rp012", Name="권미경", Group="RedPin",},
               new Bowler{ BowlerID="rp013", Name="김윤섭", Group="RedPin" },
               new Bowler{ BowlerID="rp014", Name="신인자", Group="RedPin" },
               new Bowler{ BowlerID="rp015", Name="서보윤", Group="RedPin" },
               new Bowler{ BowlerID="rp016", Name="노애경", Group="RedPin" },
               new Bowler{ BowlerID="rp017", Name="오정수", Group="RedPin" },
               new Bowler{ BowlerID="rp018", Name="최국헌", Group="RedPin" },
               new Bowler{ BowlerID="rp019", Name="이나경", Group="RedPin" },
               new Bowler{ BowlerID="rp020", Name="이상헌", Group="RedPin" },

               new Bowler{ BowlerID="fg001", Name="낙동강", Group="family", InActivity=true }
            };
            
            foreach (Bowler b in bowlers)
            {
                context.Bowlers.Add(b);
            }
            context.SaveChanges();

            var bowlerAverage = new BowlerAverage[]
            {

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

                new BowlerAverage{BowlerID="fg001", Average=200, Handicap=0}

            };

            foreach (var a in bowlerAverage)
            {
                context.BowlerAverages.Add(a);
            }
            context.SaveChanges();


           
            var games = new Game[]
            {
                new Game{ Playtime=DateTime.Parse("2018-04-10 20:00"), GameKind=GameKind.정기전, Penalty=100, Group="RedPin", Place="현대볼링장", GameContent="1/4"}

            };

            foreach (var item in games)
            {
                context.Games.Add(item);
            }
            context.SaveChanges();



            var subGames = new SubGame[]
            {
                new SubGame{ GameID=1, Round=1},
                new SubGame{ GameID=1, Round=2},

            };

            foreach (var item in subGames)
            {
                context.SubGames.Add(item);
            }
            context.SaveChanges();

            var teams = new Team[]
           {
                new Team{SubGameID=1, TeamName="A", TeamOrder=0},
                new Team{SubGameID=1, TeamName="B", TeamOrder=1 },

                new Team{SubGameID=2, TeamName="A", TeamOrder=0},
                new Team{SubGameID=2, TeamName="B", TeamOrder=1 },
           };

            foreach (Team t in teams)
            {
                context.Teams.Add(t);
            }
            context.SaveChanges();


            var teamMembers = new TeamMember[]
           {
                 new TeamMember{TeamID=1, BowlerID="rp001", Sequence=0, Score=175, Average=150 },
                 new TeamMember{TeamID=1, BowlerID="rp002", Sequence=1, Score=132, Average=160 },

                 new TeamMember{TeamID=2, BowlerID="rp003", Sequence=0, Score=168, Average=170},
                 new TeamMember{TeamID=2, BowlerID="rp005", Sequence=1, Score=165, Average=190},

                  new TeamMember{TeamID=3, BowlerID="rp001", Sequence=0, Score=0, Average=150},
                 new TeamMember{TeamID=3, BowlerID="rp002", Sequence=1, Score=0, Average=160},

                 new TeamMember{TeamID=4, BowlerID="rp003", Sequence=0, Score=0, Average=170},
                 new TeamMember{TeamID=4, BowlerID="rp005", Sequence=1, Score=0, Average=190},


           };


            foreach (TeamMember t in teamMembers)
            {
                context.TeamMembers.Add(t);
            }
            context.SaveChanges();


        }



    }
}
