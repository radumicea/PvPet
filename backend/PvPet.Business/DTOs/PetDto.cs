using PvPet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPet.Business.DTOs
{
    public class PetDto
    {
        private static readonly string[] variants = { "slime", "golem" };
        public PetDto(string name)
        {
            Variant = variants[Random.Shared.Next(variants.Length)];
            Name = name;
        }
        public PetDto()
        {
            
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Variant { get; set; }

        public int Hp { get; set; } = 100;

        public int Food { get; set; } = 100;

        public int Starvation { get; set; } = 0;

        public int Attack { get; set; } = 10;

        public int Armor { get; set; } = 5;

        public double AttackSpeed { get; set; } = 1.0;

        public int Crit { get; set; } = 0;

        public ICollection<Item> Items { get; set; }
    }
}
