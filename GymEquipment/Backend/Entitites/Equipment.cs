using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEquipment.Backend.Entitites
{
    internal class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Weight { get; set; }

        public Equipment() { }

        public Equipment(int id, string name, string type, string weight)
        {
            Id = id;
            Name = name;
            Type = type;
            Weight = weight;
        }
    }
}
