using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Melissa : Enemy
    {
        private DateTime lastAttackTime, actualTime;
        public event EventHandler OnAttack;
        private int attackCounter = 0;
        private int attacksToStun = 5;
        public Melissa(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
            AssignParameters(200, 10, 2);
            this.leaf = new Leafs.MelissaLeaf(worldPosition, "mis4", "StarSparrow_Orange");
        }

        protected override void Attack(Player player)
        {
            actualTime = DateTime.Now;
            TimeSpan time = actualTime - lastAttackTime;
            if (time.TotalSeconds > 1)
            {
                if (attackCounter == attacksToStun)
                {
                    attackCounter = 0;
                    player.setStun(2);
                }
                OnAttack?.Invoke(this, EventArgs.Empty);
                lastAttackTime = actualTime;
                attackCounter++;
                player.Hit(5);
            }
        }
    }
}
