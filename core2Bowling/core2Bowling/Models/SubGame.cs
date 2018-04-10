using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace core2Bowling.Models
{
    public class SubGame
    {
        public int ID { get; set; }

        public int GameID { get; set; }

        public int Round { get; set; }

        public Game Game { get; set; }
        public ICollection<Team> Teams { get; set; }

    }
}
