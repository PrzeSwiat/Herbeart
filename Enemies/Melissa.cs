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
        private int stunTime = 2;

        public Melissa(Vector3 worldPosition, string modelFileName, string textureFileName) : base(worldPosition, modelFileName, textureFileName)
        {
            AssignParameters(8, 5, 1.5f, 1.0f);
            this.shadow.SetScale(1.35f);
            this.leaf = new Leafs.MelissaLeaf(worldPosition, "Objects/mis4", "Textures/StarSparrow_Orange");
        }

        public override void Update(float deltaTime, Player player)
        {
            base.Update(deltaTime, player);
            this.shadow.UpdatingEnemy(this.GetPosition(), new Vector3(2.5f, 0, -2.5f));
            
        }

        protected override void Attack(Player player)
        {
            actualTime = DateTime.Now;
            TimeSpan time = actualTime - lastAttackTime;
            if (time.TotalSeconds > this.ActualAttackSpeed)
            {
                if (attackCounter == attacksToStun)
                {
                    attackCounter = 0;
                    player.setStun(stunTime);
                }
                OnAttack?.Invoke(this, EventArgs.Empty);
                lastAttackTime = actualTime;
                attackCounter++;
                player.Hit(this.Strength);
            }
        }
    }
}
