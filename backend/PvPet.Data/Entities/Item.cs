using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPet.Data.Entities
{
    public class Item : Entity
    {
        public Guid PetGuid { get; set; }

        public int Attack { get; set; }

        public int Armor { get; set; }

        public int AttackSpeed { get; set; }

        public int Crit { get; set; }

    }
}
