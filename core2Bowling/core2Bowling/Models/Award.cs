using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace core2Bowling.Models
{
    public enum AwardKind
    {
        점수1등, 점수2등, 에버1등, 에버2등, 남자하이, 여자하이, 행운상 

    }
    public class Award
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "시상분류")]
        public AwardKind AwardKind { get; set; }

        public int GameID { get; set; }
        public string BowlerID { get; set; }


        public Game Game { get; set; }
        public Bowler Bowler { get; set; }


    }
}
