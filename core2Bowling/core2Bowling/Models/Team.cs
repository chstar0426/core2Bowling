using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace core2Bowling.Models
{
    public class Team
    {
        public int ID { get; set; }

        public int SubGameID { get; set; }

        [StringLength(50)]
        [Display(Name ="팀명")]
        public string TeamName { get; set; }

        public int TeamOrder { get; set; }

        public SubGame SubGame { get; set; }
        public ICollection<TeamMember> TeamMembers { get; set; }


    }
}
