using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class MapType
    {
        public string name;
        public string type;
        public double probability;

        public MapType(string name, string type, double probability)
        {
            this.name = name;
            this.type = type;
            this.probability = probability;
        }
    }
}
