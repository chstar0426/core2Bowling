using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace core2Bowling.Models
{
    public enum GameKind
    {
        정기전, 비정기전, 개인기록

    }


    public class Game
    {
        public int ID { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm}")]
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

        [Display(Name = "우승팀 차감")]
        public int Penalty { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "소속을 입력하세요")]
        [Display(Name = "소속")]
        public string Group { get; set; }

        [Display(Name = "벌금표시")]
        public bool bFine { get; set; }

        [Display(Name = "핸디적용")]
        public bool bHandicap { get; set; }

        [StringLength(4096)]
        [Display(Name = "경기메모")]
        public string GameMemo { get; set; }


        public ICollection<SubGame> SubGames { get; set; }

        
    }
}
