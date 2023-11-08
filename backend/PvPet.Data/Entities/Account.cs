using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPet.Data.Entities
{
    public class Account : Entity
    {
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public Pet? Pet { get; set; }
        
    }
}
