﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static Assimp.Metadata;

namespace TheGame
{
    internal class InteractionEventHandler
    {
        private Player player;
        private List<Enemy> enemies;

        public InteractionEventHandler(Player player, List<Enemy> enemies)
        {
            this.player = player;
            this.enemies = enemies;

            this.player.OnAttackPressed += PlayerAttack;

            foreach (Enemy enemy in enemies)
            {
                enemy.OnAttack += EnemyAttack;
            }

        }

        public void Update(List<Enemy> enemiesList)
        {
            this.enemies = enemiesList;
        }

        #region PLAYER
        private void PlayerAttack(object sender, EventArgs e) //A
        {
            
            foreach (Enemy enemy in enemies.ToList())
            {
                if (player.boundingSphere.Intersects(enemy.boundingBox))
                {
                    enemy.Hit(player.Strength);
                }
            }
            
        }
        #endregion


        #region ENEMIES
        private void EnemyAttack(object sender, EventArgs e)
        {
            foreach(Enemy enemy in enemies)
            {
                if(sender==enemy)
                {
                    player.Hit(enemy.Strength);
                }
            }
            
        }
        #endregion

        
    }
}
