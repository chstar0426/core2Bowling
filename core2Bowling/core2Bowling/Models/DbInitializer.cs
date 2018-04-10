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
               new Bowler{ BowlerID="rp001", Name="백두산", Group="RedPin", Handicap=0, GameAverage=170 },
               new Bowler{ BowlerID="rp002", Name="한라산", Group="RedPin", Handicap=10, GameAverage=160 },
               new Bowler{ BowlerID="rp003", Name="설악산", Group="RedPin", Handicap=0,  GameAverage=140 },
               new Bowler{ BowlerID="rp005", Name="태백산", Group="RedPin", Handicap=0,  GameAverage=150 },
               new Bowler{ BowlerID="rp004", Name="임시", Group="RedPin", Handicap=0, InActivity=true,  GameAverage=140 },
                          
               new Bowler{ BowlerID="fg001", Name="낙동강", Group="family", Handicap=0, InActivity=false, GameAverage=160 }
            };
            
            foreach (Bowler b in bowlers)
            {
                context.Bowlers.Add(b);
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
                new TeamMember{TeamID=1, BowlerID="rp005", Sequence=1, Score=137},
                new TeamMember{TeamID=2, BowlerID="rp002", Sequence=0, Score=154 },
                new TeamMember{TeamID=2, BowlerID="rp003", Sequence=1, Score=167},


                new TeamMember{TeamID=3, BowlerID="rp001", Sequence=0},
                new TeamMember{TeamID=3, BowlerID="rp005", Sequence=1},
                new TeamMember{TeamID=4, BowlerID="rp002", Sequence=0 } ,
                new TeamMember{TeamID=4, BowlerID="rp003", Sequence=1}
           };


            foreach (TeamMember t in teamMembers)
            {
                context.TeamMembers.Add(t);
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
        }



    }
}
