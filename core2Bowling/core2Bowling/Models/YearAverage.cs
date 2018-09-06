using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace core2Bowling.Models
{
    public class YearAverage
    {
        public int ID { get; set; }

        [StringLength(20)]
        [Required]
        [Display(Name = "아이디")]
        public string  BowlerID { get; set; }

        [StringLength(4)]
        [Required]
        [Display(Name = "연도")]
        public string Year { get; set; }

        [Display(Name = "에바")]
        [Range(0, 300)]
        public int Average { get; set; }

        [StringLength(255)]
        [Display(Name = "비고")]
        public string Bigo { get; set; }

        public Bowler Bowler { get; set; }


    }
}
