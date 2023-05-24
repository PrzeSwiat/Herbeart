using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Mint : Enemy
    {

        private DateTime lastAttackTime, actualTime;
        public event EventHandler OnAttack;

        public Mint(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
            AssignParameters(75, 20, 2);
            this.leaf = new Leafs.MintLeaf(worldPosition, "Objects/mis4", "Textures/StarSparrow_Orange");
        }

        protected override void Attack(Player player)
        {
            actualTime = DateTime.Now;
            TimeSpan time = actualTime - lastAttackTime;
            if (time.TotalSeconds > 1)
            {
                OnAttack?.Invoke(this, EventArgs.Empty);
                lastAttackTime = actualTime;
                player.Hit(this.Strength);
            }
        }
    }
}
