using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame
{
    internal class InteractionEventHandler
    {
        Player player;
        List<Enemy> enemies;

        public InteractionEventHandler(Player player, List<Enemy> enemies)
        {
           this.player = player;
            this.enemies = enemies;

            this.player.OnAttackPressed += PlayerAttack;

        }

        private void PlayerAttack(object sender, EventArgs e) //A
        {
            foreach(Enemy enemy in enemies.ToList())
            {
                Debug.Write(enemy.position + "\n");


                if (player.boundingSphere.Intersects(enemy.boundingBox))
                {
                    enemy.Hit(player.Strength);
                }
            }
            
        }

    }
}
