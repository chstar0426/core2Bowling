using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace core2Bowling.Models
{
    public class TeamMember
    {
        public int ID { get; set; }

        public int TeamID { get; set; }
        public string BowlerID { get; set; }

        [Display(Name = "순서")]
        public int Sequence { get; set; }

        [Display(Name ="점수")]
        [Range(0,300)]
        public int Score { get; set; }



        public Team Team { get; set; }
        public Bowler Bowler { get; set; }



    }
}
