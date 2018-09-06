using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace core2Bowling.Models
{
    public class BowlerAverage
    {
        [Key]
         public string BowlerID { get; set; }

        [Display(Name ="에바")]
        [Range(0,300)]
        public int Average { get; set; }

        [Display(Name ="핸디")]
        [Range(0,50)]
        public int Handicap { get; set; }

        [Display(Name = "비고")]
        [StringLength(50)]
        public string Bigo { get; set; }

        public Bowler Bowler { get; set; }
        
      
    }
}
