using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class Nettle : Enemy
    {

        float armortime = 10;
        int lastHealth;
        public Nettle(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
            this.leaf = new Leafs.NettleLeaf(worldPosition, "mis4", "StarSparrow_Orange");
            lastHealth = this.Health;
        }

        public override void Update(float deltaTime, Player player)
        {
            base.Update(deltaTime, player);

            if (armortime >= 0)
            {
                if (Health < lastHealth)
                {
                    player.Hit(40);
                    lastHealth = Health;
                }
            }

            if (armortime < -10)
            {
                armortime = 10;
            }
            armortime = armortime - deltaTime;
        }
    }
}
