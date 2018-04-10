using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace core2Bowling.Models
{
    public class Bowler
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "아이디")]
        public string BowlerID { get; set; }

        [Required]
        [StringLength(30, ErrorMessage ="이름을 입력하세요")]
        [Display(Name="이름")]
        public string Name { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "소속을 입력하세요")]
        [Display(Name = "소속")]
        public string Group { get; set; }
        
        [Display(Name = "핸디")]
        public int Handicap { get; set; }

        [Display(Name = "평균")]
        public int GameAverage { get; set; }

        [Display(Name = "탈퇴")]
        public bool InActivity { get; set; }
    }
} 
