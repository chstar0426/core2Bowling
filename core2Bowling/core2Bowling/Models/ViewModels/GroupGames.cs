using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core2Bowling.Models
{
    public class GroupGames
    {
        public string BowlerID { get; set; }
        public string Name { get; set; }
        public bool InActivity { get; set; }
        public int beforeAvg { get; set; }
        public IGrouping<string, YearMonthAvg> Games{ get; set; }
        public float Total { get; set; }

    }
}
