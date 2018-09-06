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
        [StringLength(20)]
        [Required]
        [Display(Name = "아이디")]
        public string BowlerID { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "이름을 입력하세요")]
        [Display(Name = "이름")]
        public string Name { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "소속을 입력하세요")]
        [Display(Name = "소속")]
        public string Group { get; set; }


        [Display(Name = "탈퇴")]
        public bool InActivity { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "가입일")]
        public DateTime RegisterDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd")]
        [Display(Name = "탈퇴일")]
        public DateTime? LeaveDate { get; set; }


        [Display(Name = "비고")]
        [StringLength(50)]
        public string Bigo { get; set; }



        public BowlerAverage BowlerAverage { get; set; }
        public ICollection<TeamMember> TeamMembers { get; set; }
        public ICollection<YearAverage> YearAverages { get; set; }

    }
} 
