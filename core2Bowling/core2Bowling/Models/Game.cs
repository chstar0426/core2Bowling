using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace core2Bowling.Models
{
    public enum GameKind
    {
        정기전, 비정기전, 연습게임, 토요게임, 교류전

    }


    public class Game
    {
        public int ID { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yy-MM-dd hh:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "경기일")]
        public DateTime Playtime { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "장소")]
        public string Place { get; set; }

        [Required]
        [Display(Name = "경기분류")]
        public GameKind GameKind { get; set; }

        [StringLength(100)]
        [Display(Name = "간단설명")]
        public string GameContent { get; set; }


        [Required]
        [StringLength(30, ErrorMessage = "소속을 입력하세요")]
        [Display(Name = "소속")]
        public string Group { get; set; }


        public ICollection<SubGame> SubGames { get; set; }

        
    }
}
