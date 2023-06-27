using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class EnemiesPositions
    {
        public Vector3 enemyPosition;
        public double probability;

        public EnemiesPositions(Vector3 enemyPosition, double probability)
        {
            this.enemyPosition = enemyPosition;
            this.probability = probability;
        }
    }
}
