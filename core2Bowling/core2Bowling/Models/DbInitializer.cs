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
               new Bowler{ BowlerID="rp001", Name="백두산", Group="RedPin" },
               new Bowler{ BowlerID="rp002", Name="한라산", Group="RedPin" },
               new Bowler{ BowlerID="rp003", Name="설악산", Group="RedPin"},
               new Bowler{ BowlerID="rp004", Name="임시", Group="RedPin", InActivity=true },

               new Bowler{ BowlerID="rp005", Name="태백산", Group="RedPin" },

               new Bowler{ BowlerID="fg001", Name="낙동강", Group="family", InActivity=true }
            };
            
            foreach (Bowler b in bowlers)
            {
                context.Bowlers.Add(b);
            }
            context.SaveChanges();

            var bowlerAverage = new BowlerAverage[]
            {

                new BowlerAverage{BowlerID="rp001", Average=150, Handicap=0},
                new BowlerAverage{BowlerID="rp002", Average=160, Handicap=15},
                new BowlerAverage{BowlerID="rp003", Average=170, Handicap=0},
                new BowlerAverage{BowlerID="rp004", Average=180, Handicap=0},
                new BowlerAverage{BowlerID="rp005", Average=190, Handicap=15},

                new BowlerAverage{BowlerID="fg001", Average=200, Handicap=0}

            };

            foreach (var a in bowlerAverage)
            {
                context.BowlerAverages.Add(a);
            }
            context.SaveChanges();


           
            var games = new Game[]
            {
                new Game{ Playtime=DateTime.Parse("2018-04-10 20:00"), GameKind=GameKind.정기전, Group="RedPin", Place="현대볼링장", GameContent="1/4"}

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
                new Team{SubGameID=1, TeamName="AAA", TeamOrder=0},
                new Team{SubGameID=1, TeamName="BBB", TeamOrder=1 },

                new Team{SubGameID=2, TeamName="AAA", TeamOrder=0},
                new Team{SubGameID=2, TeamName="BBB", TeamOrder=1 },
           };

            foreach (Team t in teams)
            {
                context.Teams.Add(t);
            }
            context.SaveChanges();


            var teamMembers = new TeamMember[]
           {
                 new TeamMember{TeamID=1, BowlerID="rp001", Sequence=0, Score=175 },
                 new TeamMember{TeamID=1, BowlerID="rp002", Sequence=1, Score=132 },

                 new TeamMember{TeamID=2, BowlerID="rp003", Sequence=0, Score=168 },
                 new TeamMember{TeamID=2, BowlerID="rp005", Sequence=1, Score=165 },

                  new TeamMember{TeamID=3, BowlerID="rp001", Sequence=0, Score=0 },
                 new TeamMember{TeamID=3, BowlerID="rp002", Sequence=1, Score=0 },

                 new TeamMember{TeamID=4, BowlerID="rp003", Sequence=0, Score=0 },
                 new TeamMember{TeamID=4, BowlerID="rp005", Sequence=1, Score=0 },


           };


            foreach (TeamMember t in teamMembers)
            {
                context.TeamMembers.Add(t);
            }
            context.SaveChanges();


        }



    }
}
