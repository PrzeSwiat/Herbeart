using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class EnemyType
    {
        public string enemyType;
        public double probability;

        public EnemyType(string enemyType, double probability)
        {
            this.enemyType = enemyType;
            this.probability = probability;
        }
    }
}
