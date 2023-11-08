using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPet.Data.Entities
{
    public class Pet : Entity
    {
        public string Name { get; set; }

        public string Variant { get; set; }

        public int Hp { get; set; }

        public int Food { get; set; }

        public int Starvation { get; set; }

        public int Attack { get; set; }

        public int Armor { get; set; }

        public int AttackSpeed { get; set; }

        public int Crit { get; set; }
    }
}
